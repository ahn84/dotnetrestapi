
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using DotNetRestApi.Controllers.Dto;

namespace DotNetRestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CallbackController : ControllerBase
    {
        public CallbackController() { }
        [HttpGet("GetInfo")]
        public ActionResult<ApiResponse> GetInfo()
        {
            ApiResponse resp = new ApiResponse();
            resp.Message = "OK";
            return resp;
        }
        [HttpPost("Event")]
        public ActionResult<ApiResponse> OnEvent()
        {
            ApiResponse resp = new ApiResponse();
            resp.Message = "OK";
            return resp;
        }
    }
}