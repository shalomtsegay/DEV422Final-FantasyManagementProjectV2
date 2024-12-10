using System.ComponentModel.DataAnnotations;

namespace PlayerManagementService.Model
{
    public class Player
    {
        [Key]
        public int PlayerId { get; set; }

        [Required]
        public string PlayerName { get; set; } = string.Empty;

        public string TeamId { get; set; } = string.Empty;
        public bool IsDrafted { get; set; } = false;
        public string Details { get; set; } = string.Empty;
    }
}
