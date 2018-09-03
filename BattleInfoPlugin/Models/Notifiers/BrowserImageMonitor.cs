using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Reactive.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using BattleInfoPlugin.Win32;
using mshtml;
using IServiceProvider = BattleInfoPlugin.Win32.IServiceProvider;
using WebBrowser = CefSharp.Wpf.ChromiumWebBrowser;
using BattleInfoPlugin.Properties;
using Grabacr07.KanColleWrapper;
using BattleInfoPlugin.Models.Notifiers._Internal;
using MetroRadiance.UI;
using Grabacr07.KanColleViewer.Models.Cef;

namespace BattleInfoPlugin.Models.Notifiers
{
	internal class BrowserImageMonitor
	{
		private static readonly Settings settings = Settings.Default;
		private WebBrowser kanColleBrowser;

		private bool isConfirmPursuitNotified;

		private bool _isInCombat;
		private bool isInCombat
		{
			get { return this._isInCombat; }
			set
			{
				if (this._isInCombat == value) return;
				this._isInCombat = value;
				if (!value)
				{
					ThemeService.Current.ChangeAccent(Accent.Blue);
					ThemeService.Current.ChangeTheme(Theme.Dark);
				}
			}
		}

		public event Action ConfirmPursuit;

		public BrowserImageMonitor()
		{
			Observable.Interval(TimeSpan.FromMilliseconds(1000))
				.ObserveOn(SynchronizationContext.Current)
				.Subscribe(_ => this.CheckImage());

			var proxy = KanColleClient.Current.Proxy;

			proxy.api_start2_getData
				.ObserveOn(SynchronizationContext.Current)
				.Subscribe(_ => this.FindKanColleBrowser());

			proxy.ApiSessionSource.Subscribe(_ => this.isConfirmPursuitNotified = false);

			proxy.ApiSessionSource
				.Where(x => x.Request.PathAndQuery == "/kcsapi/api_req_practice/battle")
				.Subscribe(_ => this.isInCombat = true);
			proxy.api_req_map_start
				.Subscribe(_ => this.isInCombat = true);
			proxy.api_port
				.Subscribe(_ => this.isInCombat = false);
		}

		private void FindKanColleBrowser()
		{
			// 強引！
			var host = Application.Current.MainWindow?.FindName("kanColleHost");
			this.kanColleBrowser = host?.GetType().GetProperty("WebBrowser")?.GetValue(host) as WebBrowser;
		}

		private void CheckImage()
		{
			if (!this.isInCombat) return;
			if (this.isConfirmPursuitNotified) return;
			if (!settings.IsPursuitEnabled) return;

			this.kanColleBrowser?.GetImage(image =>
			{
				if (image == null) return;

				// 雑比較
				var browserImage = image.Resize().GetBitmapBytes();
				var confirmPursuitImage = Resources.ConfirmPursuit.Resize().GetBitmapBytes();
				var diff = browserImage.Zip(confirmPursuitImage, (a, b) => Math.Abs(a - b)).Average();
				// System.Diagnostics.Debug.WriteLine(diff); Shut up
				if (diff < 0.9)
				{
					// ボタンマウスオーバー状態とかでも大体0.5くらいまでに収まる
					// 戦闘終了の瞬間は 1.2 位の事も
					this.ConfirmPursuit?.Invoke();
					this.isConfirmPursuitNotified = true;
				}

				image.Dispose();
			});
		}
	}
}

namespace BattleInfoPlugin.Models.Notifiers._Internal
{
	internal static class Extensions
	{
		public static void GetImage(this WebBrowser browser, Action<Bitmap> callback)
		{
			CefSharp.IFrame targetCanvas;
			if (!browser.TryGetKanColleCanvas(out targetCanvas))
				return;

			var request = new ScreenshotRequest(x=> callback?.Invoke(x));
			var script = $@"
(async function()
{{
	await CefSharp.BindObjectAsync('{request.Id}');

	var canvas = document.querySelector('canvas');
	requestAnimationFrame(() =>
	{{
		var dataUrl = canvas.toDataURL('image/jpeg');
		{request.Id}.complete(dataUrl);
	}});
}})();
";
			browser.JavascriptObjectRepository.Register(request.Id, request, true);
			targetCanvas.ExecuteJavaScriptAsync(script);
		}

		private static Bitmap GetScreenshot(IViewObject viewObject, int width, int height)
		{
			var image = new Bitmap(width, height, PixelFormat.Format24bppRgb);
			var rect = new RECT { left = 0, top = 0, width = width, height = height, };
			var tdevice = new DVTARGETDEVICE { tdSize = 0, };

			using (var graphics = Graphics.FromImage(image))
			{
				var hdc = graphics.GetHdc();
				viewObject.Draw(1, 0, IntPtr.Zero, tdevice, IntPtr.Zero, hdc, rect, null, IntPtr.Zero, IntPtr.Zero);
				graphics.ReleaseHdc(hdc);
			}
			return image;
		}

		public static Bitmap Resize(this Bitmap bmp)
		{
			var result = new Bitmap(80, 48);
			using (var g = Graphics.FromImage(result))
			{
				g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Low;
				g.DrawImage(bmp, 0, 0, result.Width, result.Height);
			}
			return result;
		}

		public static byte[] GetBitmapBytes(this Bitmap bmp)
		{
			var rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
			var data = bmp.LockBits(rect, ImageLockMode.ReadOnly, bmp.PixelFormat);
			try
			{
				var ptr = data.Scan0;
				var stride = Math.Abs(data.Stride);
				var bytes = new byte[stride * bmp.Height];
				Marshal.Copy(ptr, bytes, 0, bytes.Length);
				return bytes;
			}
			finally
			{
				bmp.UnlockBits(data);
			}
		}
	}
}
