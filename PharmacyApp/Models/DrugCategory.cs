using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PharmacyApp.Models
{
    [Table("DrugCategory")]
    public class DrugCategory:ModelBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CategoryId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        public int? InstitutionId { get; set; }

        public virtual Institution Institution { get; set; }
        public virtual ICollection<Product> Products { get; set; }


    }
}