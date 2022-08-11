
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
  public class BlockpassController : ControllerBase
  {
    private string BLOCK_PASS_WEBHOOK_SECRET = "abcdef";
    private HMACSHA256 hmac = new HMACSHA256(Encoding.UTF8.GetBytes(BLOCK_PASS_WEBHOOK_SECRET));
    private readonly IHttpClientFactory _httpClientFactory;

    public BlockpassController() { }
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

>
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

    [HttpGet("Profile/{id}")]
    public async Task<ActionResult<ApiResponse>> GetProfile(string id)
    {
      var httpClient = _httpClientFactory.CreateClient("BLOCKPASS");
      var httpResponseMessage = await httpClient.GetAsync($"refId/{id}");

      if (httpResponseMessage.IsSuccessStatusCode)
      {
        using var contentStream = await httpResponseMessage.Content.ReadAsStreamAsync();
        // TODO Parse json content
        System.Console.Write(contentStream);
      }
      ApiResponse resp = new ApiResponse();
      resp.ErrorCode = 0;

      return resp;
    }
  }
}