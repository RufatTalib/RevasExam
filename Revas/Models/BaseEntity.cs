using System.ComponentModel.DataAnnotations;

namespace Revas.Models
{
    public class BaseEntity
    {
        public int Id { get; set; }

        [StringLength(maximumLength: 30, MinimumLength = 3)]
        public string Name { get; set; }
        public bool IsDeleted { get; set; }
    }
}
