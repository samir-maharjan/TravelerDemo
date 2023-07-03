using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Travel.Models
{
    public class TravelItinenaryDetail
    {
        public int Id { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public DateTime Time { get; set; }
        [Required]
        [MaxLength(255)]
        public string Hotel { get; set; }
        [Required]
        [MaxLength(255)]
        public string Location { get; set; }
        [Required]
        [MaxLength(255)]
        public string Task { get; set; }

        [Required]
        public string Description { get; set; }
        [Required]
        public long EstimatedBudget { get; set; }
        [Required]
        public DateTime FromDate { get; set; }
        [Required]
        public DateTime ToDate { get; set; }
        
        [MaxLength(255)]
        public string? HotelPhoto { get; set; }
        [Required]
        [ScaffoldColumn(false)]
        public DateTime UpdatedDate { get; set; }
        [Required]
        [ScaffoldColumn(false)]
        public DateTime UpdatedName { get; set; }
        [Required]
        [ScaffoldColumn(false)]
        public bool Deleted { get; set; }
        public int EmpolyeeId { get; set; }
        [ForeignKey("EmpolyeeId")]
        public virtual TravelSummary Empolyee { get; set; }
    }
}
