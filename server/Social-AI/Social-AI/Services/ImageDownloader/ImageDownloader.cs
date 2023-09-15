using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace Social_AI.Services.ImageDownloader;

public class ImageDownloader
{
    private readonly HttpClient _httpClient;

    public ImageDownloader()
    {
        _httpClient = new HttpClient();
    }

    public async Task<byte[]> DownloadImageAsync(string imageUrl)
    {
        try
        {
            var response = await _httpClient.GetAsync(imageUrl);
            
            if (response.IsSuccessStatusCode)
            {
                var imageBytes = await response.Content.ReadAsByteArrayAsync();
                return imageBytes;
            }
            else
            {
                throw new Exception($"Failed to download image. Status code: {response.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Error downloading image", ex);
        }
    }
}