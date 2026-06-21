using System.ComponentModel.DataAnnotations;

namespace tubes_kpl.Models
{
    public class CampaignUpdate
    {
        public int Id { get; set; }
        
        [Required]
        public int CampaignId { get; set; }
        
        [Required]
        public string Title { get; set; } = "";
        
        [Required]
        public string Description { get; set; } = "";
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
