using Newtonsoft.Json.Linq;
using PaniniWebAPI.Models;
using System.Drawing;

namespace PaniniStickerWebAPI.Interfaces
{
    public interface IImageProcessor
    {
        /// <summary>
        /// Processed image after chaining methodology
        /// </summary>
        Image StickerImage { get; }
        /// <summary>
        /// Json result that imgbb.com provides after upload an image
        /// </summary>
        JObject APIResult { get; }

        //Chainning process for tranforming an image

        /// <summary>
        /// Generate Panini overlay over an specific image
        /// </summary>
        /// <param name="frontImage">Image used as frame</param>
        /// <param name="backImage">Image that will be framed by frontImage</param>
        /// <returns>Current object</returns>
        IImageProcessor CreateOverlay(Image frontImage, Image backImage);
        /// <summary>
        /// Add corresponding text to the picture, using an specific font
        /// </summary>
        /// <param name="request">API request to be processed</param>
        /// <param name="fontToUse">Font family used for the text in the picture</param>
        /// <returns>Current object</returns>
        IImageProcessor AddTextToImage(StickerRequest request, FontFamily fontToUse);
        /// <summary>
        /// Store the resulting image
        /// </summary>
        /// <returns></returns>
        IImageProcessor StoreImage();
    }
}
