using jQuery.DataTables.Mvc;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmacyApp.DataTableCollections
{
   public interface IEntityCollection<T> where T : class, new()
    {
        IList<T> GetEntityData(int startIndex,
            int pageSize,
            ReadOnlyCollection<SortedColumn> sortedColumns,
            out int totalRecordCount,
            out int searchRecordCount,
            string searchString);
    }
}
