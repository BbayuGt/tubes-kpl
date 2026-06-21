namespace tubes_kpl.Models;

// Memenuhi Ketentuan 4: Menerapkan Clean Code. Penamaan menggunakan PascalCase untuk properti dan Class sesuai standar C#.
public class Campaign
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal TargetAmount { get; set; }
    public decimal CollectedAmount { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}
