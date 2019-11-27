using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PharmacyApp.Models
{
    [Table("Branch")]
    public class Branch: ModelBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BranchId { get; set; }

        public int InstitutionId { get; set; }

        [Required]
        public string Name { get; set; }

        public string Phone { get; set; }


        public virtual Institution Institution { get; set; }

    }
}