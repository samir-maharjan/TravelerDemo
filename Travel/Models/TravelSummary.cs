using System.ComponentModel.DataAnnotations;

namespace Travel.Models
{
    public class TravelSummary
    {
        [Display(Name = "EmpolyeeId")]
        public int Id { get; set; }
        [Required]
        [MaxLength(255)]
        [Display(Name = "Empolyee Name")]
        public string EmpName { get; set; }
        [Required]
        [Display(Name = "Empolyee Code")]
        //[MaxLength(50)]
        public int EmpCode { get; set; }
        [Required]
        [MaxLength(255)]
        public string Department { get; set; }

        [Required, Display(Name = "From")]
        public DateTime FromDate { get; set; }
        [Required, Display(Name = "To")]
        public DateTime ToDate { get; set; }

        [Required]
        [MaxLength(255)]
        public string Subject { get; set; }

        [Required]
        public string Description { get; set; }
        [Required, Display(Name = "Estimated Budget")]
        public long EstiBud { get; set; }
        public string? Image { get; set; }

        [Required]
        [ScaffoldColumn(false)]
        public bool Approved { get; set; }
        [Required]
        [ScaffoldColumn(false)]
        public bool Status { get; set; }
        [Required]
        [ScaffoldColumn(false)]
        public bool Deleted { get; set; }
        [Required, Display(Name = "Created Name")]
        [ScaffoldColumn(false)]
        [MaxLength(255)]
        public string CreatedName { get; set; }
        [Required, Display(Name = "Updated Name")]
        [ScaffoldColumn(false)]
        [MaxLength(255)]
        public string UpdatedName { get; set; }
        [Required, Display(Name = "Create Date")]
        [ScaffoldColumn(false)]
        public DateTime CreatedDate { get; set; }
        [Required, Display(Name = "Updated Date")]
        [ScaffoldColumn(false)]
        public DateTime UpdatedDate { get; set; }
        [ScaffoldColumn(false)]
        public virtual ICollection<TravelItinenaryDetail> TravelItinenaryDetail { get; set; }
        public TravelSummary()
        {
            TravelItinenaryDetail = new HashSet<TravelItinenaryDetail>();
        }
    }
}
