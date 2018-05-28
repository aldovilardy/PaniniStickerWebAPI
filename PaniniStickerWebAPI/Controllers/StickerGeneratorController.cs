﻿using RestSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Xml.Linq;

namespace PaniniWebAPI.Controllers
{
    public class StickerGeneratorController : ApiController
    {
        public HttpResponseMessage Post(Models.StickerRequest stickerRequest)
        {
            byte[] imgFileBytes = (new RestClient(stickerRequest.PhotoUrl)).DownloadData(new RestRequest(Method.GET));
            MemoryStream imgFileStream = new MemoryStream(imgFileBytes);
            Image photo = Image.FromStream(imgFileStream, true, true);
            Image frame = Image.FromFile(HttpContext.Current.Server.MapPath($@"~/../Content/images/Frame/{stickerRequest.Frame}.png"));
            Image position = Image.FromFile(HttpContext.Current.Server.MapPath($@"~/../Content/images/Position/{stickerRequest.Position}.png"));

            Image imgCanva = Overlay(photo, frame);
            imgCanva = Overlay(imgCanva, position);

            PrivateFontCollection collection = new PrivateFontCollection();
            collection.AddFontFile(HttpContext.Current.Server.MapPath($@"~/../fonts/Whitney-Semibld.ttf"));
            FontFamily fontFamily = new FontFamily("Whitney Semibold", collection);
            Font font1 = new Font(fontFamily, 19);
            Font font2 = new Font(fontFamily, 15);

            Graphics graphics = Graphics.FromImage(imgCanva);

            graphics.DrawString(
                stickerRequest.FullName,
                font1,
                Brushes.Black,
                new Point(120, 703));
            graphics.DrawString(
                stickerRequest.DateOfBirthday.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture),
                font2,
                Brushes.Black,
                new Point(189, 640));
            imgCanva.Save(HttpContext.Current.Server.MapPath($@"~/../Content/images/Public/{DateTime.Now.ToString()}"));
            return new HttpResponseMessage(HttpStatusCode.Accepted);
        }
        private Image Overlay(Image image1, Image image2)
        {
            MemoryStream uploadfile = new MemoryStream(), uploadfile2 = new MemoryStream();
            image1.Save(uploadfile, image1.RawFormat);
            image2.Save(uploadfile2, image2.RawFormat);

            var client = new RestClient("https://www.imgonline.com.ua/eng/impose-picture-on-another-picture-result.php");
            var request = new RestRequest(Method.POST);
            //request.AddFile("uploadfile", imgPath, mimeType);
            request.AddFile("uploadfile", uploadfile.ToArray(), image1.RawFormat.ToString());
            //request.AddFile("uploadfile2", "D:\\aldov\\Pictures\\panini\\Frame\\7025.png", "image/png");
            request.AddFile("uploadfile2", uploadfile2.ToArray(), image2.RawFormat.ToString());
            request.AddParameter("efset", "2");
            request.AddParameter("efset2", "50");
            request.AddParameter("efset3", "4");
            request.AddParameter("efset4", "1");
            request.AddParameter("efset5", "100");
            request.AddParameter("efset6", "5");
            request.AddParameter("efset7", "0");
            request.AddParameter("efset8", "0");
            request.AddParameter("efset9", "2");
            request.AddParameter("efset9", "2");
            request.AddParameter("jpegtype", "1");
            request.AddParameter("jpegqual", "92");
            request.AddParameter("outformat", "3");
            request.AddParameter("jpegmeta", "2");


            IRestResponse response = client.Execute(request);
            string htnlResponse = response.Content.Replace("1\">", "1\"/>").Replace("/favicon.ico\">", "/favicon.ico\"/>").Replace("design.css\">", "design.css\"/>").Replace("charset=utf-8\">", "charset=utf-8\"/>").Replace("<br>", "<br/>").Replace("&copy;", string.Empty);

            var hrefLink = XElement.Parse(htnlResponse)
                   .Descendants("a")
                   .Select(x => x.Attribute("href").Value);
            string imgResultUrl = hrefLink.ElementAt(8);
            string imgDownloadUrl = hrefLink.ElementAt(9);

            return Image.FromStream(
                new MemoryStream(
                    (new RestClient(imgDownloadUrl))
                    .DownloadData(new RestRequest(Method.GET))),
                true,
                true);
        }
    }
}
