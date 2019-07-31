using RestSharp;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace PaniniStickerWebAPI.Helpers
{
    public static class ImageHelper
    {
        public static Image GetImageFromArrayOfBytes(byte[] byteImage)
        {
            Image result = null;

            MemoryStream stream = new MemoryStream(byteImage);

            result = Image.FromStream(stream, true, true);


            return result;
        }

        public static byte[] DownloadImageFromURLAndGetBytes(string url)
        {
            var client = new RestClient(url);
            var httpReq = new RestRequest(Method.GET);
            byte[] result = client.DownloadData(httpReq);
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

        public static bool IsURL(string url)
        {
            Regex rx = new Regex(@"^(http:\/\/www\.|https:\/\/www\.|http:\/\/|https:\/\/)?[a-z0-9]+([\-\.]{1}[a-z0-9]+)*\.[a-z]{2,5}(:[0-9]{1,5})?(\/.*)?$");
            return rx.IsMatch(url);
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