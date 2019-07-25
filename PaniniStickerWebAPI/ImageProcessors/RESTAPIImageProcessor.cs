using Newtonsoft.Json.Linq;
using PaniniStickerWebAPI.Constants;
using PaniniStickerWebAPI.Helpers;
using PaniniStickerWebAPI.Interfaces;
using PaniniWebAPI.Models;
using RestSharp;
using System;
using System.Drawing;
using System.Globalization;
using System.IO;

namespace PaniniStickerWebAPI.ImageProcessors
{
    public class RESTAPIImageProcessor : IImageProcessor
    {
        private static readonly string OVERLAY_PROCESSOR_API = "https://www.imgonline.com.ua/eng/impose-picture-on-another-picture-result.php";
        private static readonly string OVERLAY_HTML_RESULT = "https://www.imgonline.com.ua/";

        public Image StickerImage { get; private set; }

        public JObject APIResult { get; private set; }

        public IImageProcessor AddTextToImage(StickerRequest request, FontFamily fontToUse)
        {
            if (StickerImage == null)
            {
                throw new InvalidOperationException("ERROR: this method should be call after Create the overlay");
            }
            //Define the font and its size for each seaction under fontFamily
            Font clubNameFont = new Font(fontToUse, 19);
            Font dateOfBirthFont = new Font(fontToUse, 15);
            Font fullNameFont = new Font(fontToUse, 24);

            Graphics graphics = Graphics.FromImage(StickerImage);

            //add club name
            graphics.DrawString(
                request.Club,
                clubNameFont,
                Brushes.Black,
                new Point(120, 703));
            //add date of birth
            graphics.DrawString(
                request.DateOfBirthday.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture),
                dateOfBirthFont,
                Brushes.Black,
                new Point(189, 639));
            //add player's name
            graphics.DrawString(
                request.FullName,
                fullNameFont,
                Brushes.Black,
                new Point(122, 671));

            return this;
        }

        public IImageProcessor CreateOverlay(Image frontImage, Image backImage)
        {
            try
            {
                using (MemoryStream backImageStream = new MemoryStream())
                using (MemoryStream frontImageStream = new MemoryStream())
                {
                    backImage.Save(backImageStream, backImage.RawFormat);
                    frontImage.Save(frontImageStream, frontImage.RawFormat);

                    var client = new RestClient(OVERLAY_PROCESSOR_API);
                    var request = new RestRequest(Method.POST);
                    request.AddFile("uploadfile", backImageStream.ToArray(), MimeTypeHelper.GetMimeType(backImage.RawFormat))
                           .AddFile("uploadfile2", frontImageStream.ToArray(), MimeTypeHelper.GetMimeType(frontImage.RawFormat))
                           .AddParameter("efset", "2")
                           .AddParameter("efset2", "50")
                           .AddParameter("efset3", "4")
                           .AddParameter("efset4", "1")
                           .AddParameter("efset5", "100")
                           .AddParameter("efset6", "5")
                           .AddParameter("efset7", "0")
                           .AddParameter("efset8", "0")
                           .AddParameter("efset9", "2")
                           .AddParameter("efset9", "2")
                           .AddParameter("jpegtype", "1")
                           .AddParameter("jpegqual", "92")
                           .AddParameter("outformat", "3")
                           .AddParameter("jpegmeta", "2");
                    //call the api
                    IRestResponse response = client.Execute(request);

                    string downloadImageUrl = ImageHelper.ScrapeImageFromHTML(response.Content, OVERLAY_HTML_RESULT);

                    byte[] imgFileBytes = (new RestClient(downloadImageUrl)).DownloadData(new RestRequest(Method.GET));
                    StickerImage = ImageHelper.GetImageFromArrayOfBytes(imgFileBytes);
                }
            }
            catch (Exception ex)
            {
                //TODO: save exception inside a log
                Console.WriteLine($"ERROR: {ex.Message}");
            }

            return this;
        }

        public IImageProcessor StoreImage()
        {
            if (StickerImage == null)
            {
                throw new InvalidOperationException("ERROR: this method should be called after CreateOverlay and AddTextToImage if you wanna text on it. ");
            }

            string fileName = $"{DateTime.Now.ToString("ddMMyyyHHmmssfffffff")}_Sticker.png";

            using (MemoryStream source = new MemoryStream())
            {
                StickerImage.Save(source, StickerImage.RawFormat);

                var client = new RestClient(ImgBBConstants.IMGBB_ENDPOINT);
                var request = new RestRequest(Method.POST);
                request.AddCookie("PHPSESSID", "e1337f77d57c68fc747a8cb52e3325ca")
                       .AddFile("source", source.ToArray(), fileName, MimeTypeHelper.GetMimeType(StickerImage.RawFormat))
                       .AddParameter("type", "file")
                       .AddParameter("action", "upload")
                       .AddParameter("privacy", "undefined")
                       .AddParameter("timestamp", ImgBBConstants.TIME_STAMP)
                       .AddParameter("auth_token", ImgBBConstants.AUTH_TOKEN)
                       .AddParameter("nsfw", ImgBBConstants.NFSW);
                IRestResponse response = client.Execute(request);

                APIResult = JObject.Parse(response.Content);

                return this;
            }
        }
    }
}