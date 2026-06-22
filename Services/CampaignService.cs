using System.Net.Http.Json;
using tubes_kpl.Models;

namespace tubes_kpl.Services;

// Memenuhi Ketentuan 3: Integrasi GUI dengan backend (mengakses hasil implementasi CLO2 melalui API)
// Memenuhi Ketentuan 4: Menerapkan Clean Code. Prinsip Single Responsibility diterapkan (Service hanya fokus pada fetching data)
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

    public async Task<bool> PostCampaignUpdate(int campaignId, CampaignUpdate update, string token)
    {
        var client = _httpClientFactory.CreateClient("BackendAPI");
        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        
        var response = await client.PostAsJsonAsync($"api/campaign/{campaignId}/updates", update);
        return response.IsSuccessStatusCode;
    }

    public async Task<IEnumerable<CampaignUpdate>?> GetCampaignUpdates(int campaignId)
    {
        var client = _httpClientFactory.CreateClient("BackendAPI");
        
        return await client.GetFromJsonAsync<IEnumerable<CampaignUpdate>>($"api/campaign/{campaignId}/updates");
    }
}