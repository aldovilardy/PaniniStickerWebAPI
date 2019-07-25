using System;
using System.Drawing.Imaging;
using System.Linq;

namespace PaniniStickerWebAPI.Helpers
{
    public static class MimeTypeHelper
    {
        public static string GetMimeType(ImageFormat imageFormat)
        {
            ImageCodecInfo[] imageCodecs = ImageCodecInfo.GetImageEncoders();
            string mimeType = imageCodecs.FirstOrDefault(c => c.FormatID == imageFormat.Guid).MimeType;
            return mimeType;
        }
    }
}