using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace PaniniStickerWebAPI.Helpers
{
    public static class ImageHelper
    {
        public static Image GetImageFromArrayOfBytes(byte[] byteImage)
        {
            Image result = null;

            using (MemoryStream stream = new MemoryStream(byteImage))
            {
                result = Image.FromStream(stream, true, true);
            }

            return result;
        }

        public static string ScrapeImageFromHTML(string html, string parentURL)
        {
            string scrapping =
                html
                    .Replace("1\">", "1\"/>")
                    .Replace("/favicon.ico\">", "/favicon.ico\"/>")
                    .Replace("design.css\">", "design.css\"/>")
                    .Replace("charset=utf-8\">", "charset=utf-8\"/>")
                    .Replace("<br>", "<br/>")
                    .Replace("&copy;", string.Empty);

            var hrefLink = GetSourceFromAnchorTag(scrapping);
            string finalImageUrl = hrefLink.ElementAt(9).Replace("..", parentURL);

            return finalImageUrl;

        }

        private static IEnumerable<string> GetSourceFromAnchorTag(string html)
        {
            var src = XElement.Parse(html)
                           .Descendants("a")
                           .Select(x => x.Attribute("href").Value);
            return src;
        }
    }
}