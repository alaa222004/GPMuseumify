using GPMuseumify.BL.DTOs.News;
using GPMuseumify.BL.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GPMuseumify.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private readonly INewsService _newsService;

        public NewsController(INewsService newsService)
        {
            _newsService = newsService;
        }
        [HttpGet]
        public async Task<ActionResult<AllContentResponseDto>> GetAllNews()
        {
            var result = await _newsService.GetAllNewsAsync();
            return Ok(result);
        }
    }
}
