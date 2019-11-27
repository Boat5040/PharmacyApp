using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BSystems.Web.Security.SVS.JQueryDataTable
{
    /// <summary>
    /// Represents data to fed to jQuery data table
    /// </summary>
    /// <typeparam name="T">Data type to feed jQuery data table with</typeparam>
    public class DataTableData<T>
    {
        public int draw { get; set; }
        public int recordsTotal { get; set; }
        public int recordsFiltered { get; set; }
        public List<T> data { get; set; }
    }
}