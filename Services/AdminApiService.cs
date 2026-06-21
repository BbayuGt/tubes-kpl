using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace tubes_kpl.Services;

public class CampaignModel
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; } = "";

    [JsonPropertyName("description")]
    public string Description { get; set; } = "";

    [JsonPropertyName("targetAmount")]
    public decimal TargetAmount { get; set; }

    [JsonPropertyName("collectedAmount")]
    public decimal CollectedAmount { get; set; }

    [JsonPropertyName("imageUrl")]
    public string ImageUrl { get; set; } = "";

    [JsonPropertyName("isActive")]
    public bool IsActive { get; set; } = true;

    [JsonPropertyName("createdAt")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

public class DonationModel
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("userId")]
    public int UserId { get; set; }

    [JsonPropertyName("campaignId")]
    public int CampaignId { get; set; }

    [JsonPropertyName("createdDate")]
    public DateTime CreatedDate { get; set; }

    [JsonPropertyName("amount")]
    public decimal Amount { get; set; }

    [JsonPropertyName("status")]
    public string Status { get; set; } = "Pending";

    [JsonPropertyName("user")]
    public UserModel? User { get; set; }

    [JsonPropertyName("campaign")]
    public CampaignModel? Campaign { get; set; }
}

public class UserModel
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = "";

    [JsonPropertyName("email")]
    public string Email { get; set; } = "";

    [JsonPropertyName("role")]
    public string Role { get; set; } = "User";
}

public class PaymentStatusModel
{
    [JsonPropertyName("externalId")]
    public string ExternalId { get; set; } = "";

    [JsonPropertyName("amount")]
    public decimal Amount { get; set; }

    [JsonPropertyName("payerEmail")]
    public string PayerEmail { get; set; } = "";

    [JsonPropertyName("description")]
    public string Description { get; set; } = "";

    [JsonPropertyName("status")]
    public string Status { get; set; } = "PENDING";

    [JsonPropertyName("invoiceUrl")]
    public string InvoiceUrl { get; set; } = "";

    [JsonPropertyName("paidAt")]
    public DateTime? PaidAt { get; set; }

    [JsonPropertyName("createdAt")]
    public DateTime CreatedAt { get; set; }

    [JsonPropertyName("expiryDate")]
    public DateTime? ExpiryDate { get; set; }
}

public class LoginRequest
{
    [JsonPropertyName("email")]
    public string Email { get; set; } = "";

    [JsonPropertyName("password")]
    public string Password { get; set; } = "";
}

public class LoginResponse
{
    [JsonPropertyName("message")]
    public string Message { get; set; } = "";

    [JsonPropertyName("token")]
    public string Token { get; set; } = "";
}

public class UserResponse
{
    [JsonPropertyName("user")]
    public UserResponseData? User { get; set; }
}

public class UserResponseData
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = "";

    [JsonPropertyName("email")]
    public string Email { get; set; } = "";
}

public class AdminApiService
{
    private readonly HttpClient _httpClient;
    private string _jwtToken = "";

    public AdminApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public string JwtToken
    {
        get => _jwtToken;
        set
        {
            _jwtToken = value;
            if (!string.IsNullOrEmpty(value))
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", value);
            }
            else
            {
                _httpClient.DefaultRequestHeaders.Authorization = null;
            }
        }
    }

    public bool IsAuthenticated => !string.IsNullOrEmpty(_jwtToken);

    public async Task<LoginResponse?> LoginAsync(string email, string password)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/login", new LoginRequest
            {
                Email = email,
                Password = password
            });

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
                if (result != null && !string.IsNullOrEmpty(result.Token))
                {
                    JwtToken = result.Token;
                }
                return result;
            }
            return null;
        }
        catch
        {
            return null;
        }
    }

    public async Task<UserResponseData?> GetCurrentUserAsync()
    {
        try
        {
            var response = await _httpClient.GetFromJsonAsync<UserResponse>("api/auth/me");
            return response?.User;
        }
        catch
        {
            return null;
        }
    }

    public async Task<List<CampaignModel>> GetAllCampaignsAsync()
    {
        try
        {
            var result = await _httpClient.GetFromJsonAsync<List<CampaignModel>>("api/campaign");
            return result ?? new List<CampaignModel>();
        }
        catch
        {
            return new List<CampaignModel>();
        }
    }

    public async Task<CampaignModel?> GetCampaignByIdAsync(int id)
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<CampaignModel>($"api/campaign/{id}");
        }
        catch
        {
            return null;
        }
    }

    public async Task<bool> UpdateCampaignAsync(int id, CampaignModel campaign)
    {
        try
        {
            var response = await _httpClient.PutAsJsonAsync($"api/campaign/{id}", campaign);
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> DeleteCampaignAsync(int id)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"api/campaign/{id}");
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

    public async Task<CampaignModel?> CreateCampaignAsync(CampaignModel campaign)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/campaign", campaign);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<CampaignModel>();
            }
            return null;
        }
        catch
        {
            return null;
        }
    }

    public async Task<List<DonationModel>> GetAllDonationsAsync()
    {
        try
        {
            var result = await _httpClient.GetFromJsonAsync<List<DonationModel>>("api/donation");
            return result ?? new List<DonationModel>();
        }
        catch
        {
            return new List<DonationModel>();
        }
    }

    public async Task<DonationModel?> GetDonationByIdAsync(int id)
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<DonationModel>($"api/donation/{id}");
        }
        catch
        {
            return null;
        }
    }

    public async Task<bool> DeleteDonationAsync(int id)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"api/donation/{id}");
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> UpdateDonationStatusAsync(int id, string status)
    {
        try
        {
            var response = await _httpClient.PutAsJsonAsync($"api/donation/{id}/status", new { status = status });
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

    public async Task<PaymentStatusModel?> GetPaymentStatusAsync(string externalId)
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<PaymentStatusModel>($"api/payment/status/{externalId}");
        }
        catch
        {
            return null;
        }
    }
}
