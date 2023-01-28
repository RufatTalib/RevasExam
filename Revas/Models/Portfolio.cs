using System.ComponentModel.DataAnnotations.Schema;

namespace Revas.Models
{
    public class Portfolio : BaseEntity
    {
        public string? ImageUrl { get; set; }

        [NotMapped]
        public IFormFile? Image { get; set; }
    }
}
