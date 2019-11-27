using PharmacyApp.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PharmacyApp.DataTableCollections
{
    public class BaseEntityCollection
    {
        protected readonly ApplicationDbContext DataContext;

        public BaseEntityCollection(ApplicationDbContext context)
        {
            DataContext = context;
        }

    }
}