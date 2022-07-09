
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using System.Security.Cryptography;
using System.Text;

using DotNetRestApi.Controllers.Dto;

namespace DotNetRestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CallbackController : ControllerBase
    {
        private HMACSHA256 hmac = new HMACSHA256(Encoding.UTF8.GetBytes("abcdef"));
        public CallbackController() { }
        [HttpGet("GetInfo")]
        public ActionResult<ApiResponse> GetInfo()
        {
            ApiResponse resp = new ApiResponse();
            resp.Message = "OK";
            return resp;
        }
        [HttpPost("Event")]
        public async Task<ActionResult<ApiResponse>> OnEvent(BlockpassKYCEvent ev)
        {
            Request.EnableBuffering();
            Request.Body.Position = 0;
            // var rawRequestBody = await new StreamReader(Request.Body).ReadToEndAsync();
            var hashValue = await hmac.ComputeHashAsync(Request.Body);


            StringValues signatureValues;
            ApiResponse resp = new ApiResponse();
            if (Request.Headers.TryGetValue("X-Hub-Signature", out signatureValues))
            {
                string signature = signatureValues.First();
                if (signature == Convert.ToHexString(hashValue).ToLower())
                {
                    resp.Message = "Signature is verified";
                }
                else
                {
                    resp.ErrorCode = -1;
                    resp.Message = "Invalid signature";
                }
            }
            else
            {
                resp.ErrorCode = -2;
                resp.Message = "No signature";
            }

            resp.Data = ev;
            return resp;
        }
    }
}