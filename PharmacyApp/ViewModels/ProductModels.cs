using PharmacyApp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PharmacyApp.ViewModels
{
    public class ProductViewModel
    {
        [Key]
        public int ProductId { get; set; }
        public string Name { get; set; }

        [Display(Name ="Category")]
        public int CategoryId { get; set; }

        public int Quantity { get; set; }
        [Display(Name = "Purchase Price")]
        public decimal PurchasedPrice { get; set; }
        [Display(Name = "Selling Price")]
        public decimal SellingPrice { get; set; }
        [Display(Name = "Generic Name")]
        public string GenericName { get; set; }
        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }
        public string Effect { get; set; }
        [Display(Name = "Manufacturing Date")]
        public DateTime MufDate { get; set; }
        [Display(Name = "Expire Date")]
        public DateTime ExpiredDate { get; set; }
        [Display(Name = "Manufacturer")]
        public string Manufacturer { get; set; }


    }

    public class NewProductViewModel
    {
        [Required(AllowEmptyStrings = false)]
        public string Name { get; set; }
        [Display(Name = "Category")]
        public int CategoryId { get; set; }
        [Display(Name = "Purchase Price")]
        public decimal PurchasedPrice { get; set; }
        [Display(Name = "Selling Price")]
        public decimal SellingPrice { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Please enter valid integer Number")]
        public int Quantity { get; set; }
        [Display(Name = "Generic Name")]
        public string GenericName { get; set; }
        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }
        public string Effect { get; set; }
        [Display(Name = "Manufacturing Date")]
        public DateTime MufDate { get; set; }
        [Display(Name = "Expire Date")]
        public DateTime ExpiredDate { get; set; }
        [Display(Name = "Manufacturer")]
        public string Manufacturer { get; set; }

        [Display(Name = "Photo")]
        public HttpPostedFileBase File { get; set; }


    }

    public class EditProductViewModel
    {
        [HiddenInput(DisplayValue = false)]
        public int ProductId { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Name { get; set; }

        [Display(Name ="Category")]
        public int CategoryId { get; set; }
        [Display(Name = "Purchase Price")]
        public decimal PurchasedPrice { get; set; }
        [Display(Name = "Selling Price")]
        public decimal SellingPrice { get; set; }
        public int Quantity { get; set; }
        [Display(Name = "Generic Name")]
        public string GenericName { get; set; }
        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }
        public string Effect { get; set; }
        [Display(Name = "Manufacturing Date")]
        public DateTime MufDate { get; set; }
        [Display(Name = "Expire Date")]
        public DateTime ExpiredDate { get; set; }
        [Display(Name = "Manufacturer")]
        public string Manufacturer { get; set; }


    }
}