using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PharmacyApp.Models
{
    [Table("sales")]
    public class Sale : ModelBase
    {
        public int Id { get; set; }        
        public string TransactionNumber { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal Balance { get; set; }

        public int? CustomerId { get; set; }
        public Customer Customer { get; set; }
        public ICollection<SaleItem> SaleItems { get; set; }
    }

    [Table("customers")]
    public class Customer : ModelBase
    {
        public int CustomerId { get; set; }
        public string Fullname { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }

    }

    [Table("saleItems")]
    public class SaleItem
    {
        public int Id { get; set; }
        public int ProductId{get;set;}
        public decimal Price { get; set; }
        public int SaleId { get; set; }
        public Product Prouct { get; set; }
        public Sale Sale { get; set; }
    }
}