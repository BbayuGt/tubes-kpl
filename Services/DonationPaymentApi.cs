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

    public async Task<CreateInvoiceResponseDto> CreateInvoiceAsync(
        CreateInvoiceRequestDto request,
        CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PostAsJsonAsync("api/payment/create-invoice", request, cancellationToken);
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<CreateInvoiceResponseDto>(cancellationToken: cancellationToken))!;
    }
    public async Task<PaymentStatusDto?> GetPaymentStatusAsync(string externalId, CancellationToken cancellationToken = default)
    {
        return await _httpClient.GetFromJsonAsync<PaymentStatusDto>($"api/payment/status/{Uri.EscapeDataString(externalId)}", cancellationToken);
    }
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
    [JsonPropertyName("invoice_url")]
    public string InvoiceUrl { get; set; } = string.Empty;
}

public class PaymentStatusDto
{
    public string ExternalId { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
}
