using System.ComponentModel.DataAnnotations;

namespace Revas.VMs
{
    public class RegisterVM
    {
        [Required]
        [StringLength(maximumLength: 50, MinimumLength = 8)]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [StringLength(maximumLength: 50, MinimumLength = 8)]
        public string FullName { get; set; }

    }
}
