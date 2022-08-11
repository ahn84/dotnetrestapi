
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

using DotNetRestApi.Controllers.Dto;

namespace DotNetRestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlockpassController : ControllerBase
    {
        private static string BLOCK_PASS_WEBHOOK_SECRET = "DStarterSecret";
        private HMACSHA256 hmac = new HMACSHA256(Encoding.UTF8.GetBytes(BLOCK_PASS_WEBHOOK_SECRET));
        private readonly IHttpClientFactory _httpClientFactory;

        public BlockpassController(IHttpClientFactory factory)
        {
            _httpClientFactory = factory;
        }
        [HttpGet("GetInfo")]
        public ActionResult<ApiResponse> GetInfo()
        {
            ApiResponse resp = new ApiResponse();
            resp.Message = "OK";
            return resp;
        }
        /// Receive and verify Blockpass webhook
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
        /// Get Blockpass profile by refId 
        // refId example 0xd0b770da072fce24a59cc0a15bc4e3a9e27b1d52
        [HttpGet("Profile/{id}")]
        public async Task<ActionResult<ApiResponse>> GetProfile(string id)
        {
            var httpClient = _httpClientFactory.CreateClient("BLOCKPASS");
            var httpResponseMessage = await httpClient.GetAsync($"refId/{id}");

            ApiResponse resp = new ApiResponse();
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                using var contentStream = await httpResponseMessage.Content.ReadAsStreamAsync();
                var responseData = await JsonSerializer.DeserializeAsync<BlockpassProfileResponse>(contentStream);

                resp.ErrorCode = 0;
                resp.Data = responseData?.data?.identities?.family_name?.value
                              + responseData?.data?.identities?.given_name?.value;
                //base64 photo: identites.selfie.value, identities.selfie_national_id.value, identities.passport.value
            }

            return resp;
        }
    }
}