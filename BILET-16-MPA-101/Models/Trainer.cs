namespace BILET_16_MPA_101.Models
{
    public class Trainer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; } = string.Empty;
        public Department Department { get; set; }
        public int DepartmentId { get; set; }
        public string ImagePath { get; set; } = string.Empty;
    }
}
