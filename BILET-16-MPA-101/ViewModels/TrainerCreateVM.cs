using System.ComponentModel.DataAnnotations;

namespace BILET_16_MPA_101.ViewModels
{
    public class TrainerCreateVM
    {
        [Required, MaxLength(256), MinLength(3)]
        public string Name { get; set; } = string.Empty;
        [Required, MaxLength(1024), MinLength(3)]
        public string Description { get; set; } = string.Empty;
        [Required]
        public string ImagePath { get; set; } = string.Empty;
        [Required]
        public int DepartmentId { get; set; }
    }
}
