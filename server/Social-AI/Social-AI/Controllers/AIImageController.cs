using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using OpenAI;
using OpenAI.Images;

namespace Social_AI.Controllers;

public class AIImageController : ControllerBase
{
    string APIKEY = string.Empty;
    
    public AIImageController(IConfiguration conf)
    {
        APIKEY = conf.GetSection("OPENAI_API_KEY").Value;
    }
    
    [Route("/api/GenerateAIImage")]
    [HttpPost]
    public async Task<IActionResult> GenerateAIImage([FromBody] string prompt)
    {
        
        var _openAIClient = new OpenAIClient(new OpenAIAuthentication(APIKEY));
        var result = await _openAIClient.ImagesEndPoint.GenerateImageAsync(prompt, 1, ImageSize.Large);
        return Ok(result);
    }
}