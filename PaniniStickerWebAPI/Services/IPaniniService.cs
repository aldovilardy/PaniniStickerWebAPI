using Newtonsoft.Json.Linq;
using PaniniWebAPI.Models;

namespace PaniniStickerWebAPI.Services
{
    public interface IPaniniService
    {
        JObject MakePaniniFromImage(StickerRequest request);
    }
}
