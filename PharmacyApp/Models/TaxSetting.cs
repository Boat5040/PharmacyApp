using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PharmacyApp.Models
{
    [Table("TaxSetting")]

    public class TaxSetting: ModelBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int TaxId { get; set; }
        public string Name { get; set; }
        public double Percentage { get; set; }
        public int Status { get; set; }
        public int? InstitutionId { get; set; }

        public virtual Institution Institution { get; set; }

    }
}