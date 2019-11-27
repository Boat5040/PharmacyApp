using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PharmacyApp.Models
{
    [Table("product")]
    public class Product: ModelBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int ProductId { get; set; }
        public string Name { get; set; }

        public int? CategoryId { get; set; }

        public decimal PurchasedPrice { get; set; }
        public decimal SellingPrice { get; set; }
        public int? Quantity { get; set; }
        public string GenericName { get; set; }
        public string CompanyName { get; set; }
        public string Effect { get; set; }
        public DateTime? MufDate { get; set; }
        public DateTime? ExpiredDate { get; set; }
        public string Manufacturer { get; set; }
        [Column(TypeName = "image")]
        public byte[] DrugImage { get; set; }
        public int? InstitutionId { get; set; }

        public virtual Institution Institution { get; set; }

        public DrugCategory DrugCategory { get; set; }

    }
}