namespace BILET_16_MPA_101.Models
{
    public class Department
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public ICollection<Trainer> Trainers { get; set; } 
    }
}
