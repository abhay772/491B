using System.ComponentModel.DataAnnotations;

namespace AA.PMTOGO.Models.Entities
{
    public class CrimeAlert
    {
        [Key]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string ID { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Time { get; set; } = string.Empty;
        public string Date { get; set; } = string.Empty;
        public double X { get; set; }
        public double Y { get; set; }

        public CrimeAlert() { }
    }
}
