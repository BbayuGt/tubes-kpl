using System.Net.Http.Json;
using tubes_kpl.Models;

namespace tubes_kpl.Services;

public class CampaignService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public CampaignService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<Campaign?> GetCampaignById(int id)
    {
        var client = _httpClientFactory.CreateClient("BackendAPI");

        return await client.GetFromJsonAsync<Campaign>(
            $"api/campaign/{id}");
    }
}