using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BSystems.Web.Security.SVS.JQueryDataTable
{
    public class DataTableInputExtractor
    {
        private HttpRequestBase _request;

        public DataTableInputExtractor(HttpRequestBase request)
        {
            _request = request;
        }

        public string SearchTerm
        {
            get
            {
                return _request.Form.GetValues("search[value]").FirstOrDefault();
            }
        }

        public int SortColumn
        {
            get
            {
                string sortColumn = _request.Form.GetValues("order[0][column]").FirstOrDefault();
                return string.IsNullOrWhiteSpace(sortColumn) ? -1 : int.Parse(sortColumn);
            }
        }

        public string SortDirection
        {
            get
            {
                string sortDirection = _request.Form.GetValues("order[0][dir]").FirstOrDefault();
                return string.IsNullOrWhiteSpace(sortDirection) ? "asc" : sortDirection;
            }
        }
    }
}