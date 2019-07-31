using PaniniStickerWebAPI.Helpers;
using PaniniStickerWebAPI.Services;
using PaniniWebAPI.Models;
using System.Web.Http;

namespace PaniniStickerWebAPI.Controllers
{
    [RoutePrefix("api/v2")]
    public class PaniniController : ApiController
    {
        private readonly IPaniniService _paniniService;

        public PaniniController() : this(new PaniniService()) { }
        public PaniniController(IPaniniService paniniService)
        {
            _paniniService = paniniService;
        }

        [Route("StickerGenerator")]
        [HttpPost]
        public IHttpActionResult Post(StickerRequest request)
        {
            if (!ImageHelper.IsURL(request.PhotoUrl))
            {
                return BadRequest("You don't pass a correct PhotoURL, please check your request");
            }

            var processResult = _paniniService.MakePaniniFromImage(request);
            if (processResult != null)
            {
                return Ok(processResult);
            }
            else
            {
                return InternalServerError();
            }

        }
    }
}