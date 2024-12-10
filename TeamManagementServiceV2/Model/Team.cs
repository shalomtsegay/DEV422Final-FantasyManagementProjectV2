using System.ComponentModel.DataAnnotations;

namespace TeamManagementServiceV2.Model
{
    public class Team
    {
        [Key]
        public int TeamId { get; set; }

        [Required]
        public string TeamName { get; set; } = string.Empty;

        public List<int> PlayerIds { get; set; } = new List<int>();
    }
}
