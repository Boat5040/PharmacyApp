using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PharmacyApp.ViewModels
{
    public class CategoryViewModel
    {
        [Key]
        public int CategoryId { get; set; }

        public string Name { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

    }

    public class NewCategoryViewModel
    {
        [Required(AllowEmptyStrings = false)]
        public string Name { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

    }

    public class EditCategoryViewModel
    {
        [HiddenInput(DisplayValue = false)]
        public int CategoryId { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Name { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

    }
}