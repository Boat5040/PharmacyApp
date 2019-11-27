using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PharmacyApp.ViewModels
{
    public class TaxViewModel
    {
        [Key]

        public int TaxId { get; set; }
        public string Name { get; set; }
        public double Percentage { get; set; }
        public int Status { get; set; }
    }

    public class NewTaxViewModel
    {

        public int TaxId { get; set; }
        public string Name { get; set; }
        public double Percentage { get; set; }
        public int Status { get; set; }
    }

    public class EditTaxViewModel
    {
        [HiddenInput(DisplayValue = false)]
        public int TaxId { get; set; }
        public string Name { get; set; }
        public double Percentage { get; set; }
        public int Status { get; set; }
    }


}