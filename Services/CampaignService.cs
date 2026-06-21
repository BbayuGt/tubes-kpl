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
}