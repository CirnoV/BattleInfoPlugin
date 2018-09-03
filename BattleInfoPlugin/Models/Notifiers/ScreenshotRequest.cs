using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleInfoPlugin.Models.Notifiers
{
	public class ScreenshotRequest
	{
		private readonly Action<Bitmap> callback;

		public string Id { get; }

		public ScreenshotRequest(Action<Bitmap> callback)
		{
			this.Id = $"ssReq{DateTimeOffset.Now.Ticks}";
			this.callback = callback;
		}

		public void Complete(string dataUrl)
		{
			try
			{
				var array = dataUrl.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
				if (array.Length != 2) throw new Exception($"無効な形式: {dataUrl}");

				var base64 = array[1];
				var bytes = Convert.FromBase64String(base64);

				using (var ms = new MemoryStream())
				{
					ms.Write(bytes, 0, bytes.Length);
					this.callback?.Invoke(Image.FromStream(ms) as Bitmap);
				}
			}
			catch { }
		}
	}
}
