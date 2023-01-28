using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Revas.Models
{
    public class Employee : BaseEntity
    {
        [Required]
        [StringLength(maximumLength: 50, MinimumLength = 3)]
        public string Surname { get; set; }

        [Required]
        [StringLength(maximumLength: 160, MinimumLength = 10)]
        public string Description { get; set; }

        [Required]
        [StringLength(maximumLength: 255, MinimumLength = 1)]
        public string Link1 { get; set; }
        [Required]
        [StringLength(maximumLength: 255, MinimumLength = 1)]
        public string Link2 { get; set; }
        [Required]
        [StringLength(maximumLength: 255, MinimumLength = 1)]
        public string Link3 { get; set; }

        public string? ImageUrl { get; set; }

        [NotMapped]
        public IFormFile? Image { get; set; }
    }
}
