using Newtonsoft.Json.Linq;
using PaniniStickerWebAPI.Helpers;
using PaniniStickerWebAPI.ImageProcessors;
using PaniniStickerWebAPI.Interfaces;
using PaniniWebAPI.Models;
using System;
using System.Drawing;
using System.Drawing.Text;
using System.Web;

namespace PaniniStickerWebAPI.Services
{
    public class PaniniService : IPaniniService
    {

        private readonly IImageProcessor _imageProcessor;
        private readonly PrivateFontCollection _fontCollection;
        private readonly FontFamily _font;

        public PaniniService() : this(new RESTAPIImageProcessor()) { }

        public PaniniService(IImageProcessor imageProcessor)
        {
            _imageProcessor = imageProcessor;
            _fontCollection = new PrivateFontCollection();
            _fontCollection.AddFontFile(HttpContext.Current.Server.MapPath(@"~/fonts/Whitney-Semibld.ttf"));
            _font = new FontFamily("Whitney Semibold", _fontCollection);
        }

        public JObject MakePaniniFromImage(StickerRequest request)
        {
            //Compose frame and position images from request
            try
            {
                Image photoToFrame = ImageHelper.GetImageFromArrayOfBytes(
                            ImageHelper.DownloadImageFromURLAndGetBytes(request.PhotoUrl)
                        );
                Image playerPosition = Image.FromFile(HttpContext.Current.Server.MapPath($"~/Content/images/Position/{request.Position}.png"));
                Image frame = Image.FromFile(HttpContext.Current.Server.MapPath($"~/Content/images/Frame/{request.Frame}.png"));

                var processorResult = _imageProcessor.CreateOverlay(photoToFrame, frame);
                processorResult = processorResult
                    .CreateOverlay(processorResult.StickerImage, playerPosition)
                    .AddTextToImage(request, _font)
                    .StoreImage();

                photoToFrame.Dispose();
                playerPosition.Dispose();
                frame.Dispose();
                return processorResult.APIResult;
            }
            catch (Exception ex)
            {
                //TODO: Save exceptions inside a log
                Console.WriteLine(ex.Message);
                return null;
            }


        }
    }
}