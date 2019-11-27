using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PharmacyApp.ViewModels
{
    public class POSViewModel
    {
        [Key]
        [Display(Name = "Actions")]
        public int PosId { get; set; }
        public string ScannerCode { get; set; }
        public string Products { get; set; }
        public string Product { get; set; }
        public decimal SalesPrice { get; set; }
        public float Discount { get; set; }
        public int Quantity { get; set; }
        public float Tax { get; set; }
        public decimal Paid { get; set; }
        public string PaymentType { get; set; }
        public decimal Total { get; set; }
        public decimal Due { get; set; }
        public string RefNumber { get; set; }

        public int? CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerAddress { get; set; }


    }
}