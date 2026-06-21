using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace tubes_kpl.Services;

public class DonationPaymentApi
{
    private readonly HttpClient _httpClient;

    public DonationPaymentApi(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<CampaignDto>> GetCampaignsAsync(CancellationToken cancellationToken = default)
    {
        return await _httpClient.GetFromJsonAsync<List<CampaignDto>>("api/campaign", cancellationToken) ?? [];
    }

    public async Task<CreateDonationResponseDto> CreateDonationAsync(
        CreateDonationRequestDto request,
        CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PostAsJsonAsync("api/donation", request, cancellationToken);
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<CreateDonationResponseDto>(cancellationToken))!;
    }

    public async Task<CreateInvoiceResponseDto> CreateInvoiceAsync(
        CreateInvoiceRequestDto request,
        CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PostAsJsonAsync("api/payment/create-invoice", request, cancellationToken);
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<CreateInvoiceResponseDto>(cancellationToken))!;
    }

    public async Task<PaymentStatusDto?> GetPaymentStatusAsync(
        string externalId,
        CancellationToken cancellationToken = default)
    {
        return await _httpClient.GetFromJsonAsync<PaymentStatusDto>($"api/payment/status/{Uri.EscapeDataString(externalId)}", cancellationToken);
    }
}

public class CampaignDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal TargetAmount { get; set; }
    public decimal CollectedAmount { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}

public class CreateDonationRequestDto
{
    public int CampaignId { get; set; }
    public decimal Amount { get; set; }
    public string DonorName { get; set; } = string.Empty;
    public string DonorEmail { get; set; } = string.Empty;
}

public class CreateDonationResponseDto
{
    public int DonationId { get; set; }
    public int CampaignId { get; set; }
    public decimal DonationAmount { get; set; }
    public decimal UpdatedCampaignTotal { get; set; }
    public DateTime CreatedDate { get; set; }
}

public class CreateInvoiceRequestDto
{
    public string ExternalId { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string PayerEmail { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}

public class CreateInvoiceResponseDto
{
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("external_id")]
    public string ExternalId { get; set; } = string.Empty;

    public decimal Amount { get; set; }
    public string Status { get; set; } = string.Empty;

    [JsonPropertyName("invoice_url")]
    public string InvoiceUrl { get; set; } = string.Empty;

    [JsonPropertyName("expiry_date")]
    public DateTimeOffset? ExpiryDate { get; set; }
}

public class PaymentStatusDto
{
    public string ExternalId { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string PayerEmail { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string InvoiceUrl { get; set; } = string.Empty;
    public DateTime? PaidAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ExpiryDate { get; set; }
}
