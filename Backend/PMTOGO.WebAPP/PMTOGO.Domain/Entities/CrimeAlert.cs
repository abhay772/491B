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
        public string X { get; set; } = string.Empty;
        public string Y { get; set; } = string.Empty;

        public CrimeAlert() { }
    }
}
