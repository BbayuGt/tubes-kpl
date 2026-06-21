using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;
using tubes_kpl.Models;

namespace tubes_kpl.Components.Pages.Campaign;

public partial class CampaignDetail
{
	[Inject]
	public HttpClient Http { get; set; }

	[Parameter]
	public int Id { get; set; }

	public int DonationAmount { get; set; }
	// Menggunakan nama lengkap agar tidak konflik dengan folder Campaign
	public tubes_kpl.Models.Campaign Campaign { get; set; }

	protected override async Task OnInitializedAsync()
	{
		try
		{
			// Panggil backend Anda
			Campaign = await Http.GetFromJsonAsync<tubes_kpl.Models.Campaign>($"http://localhost:8080/api/Campaign/{Id}");
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Error fetching data: {ex.Message}");
		}
	}
}