using Apex.Websites.ViewModels.DataTables;
using Microsoft.AspNetCore.Http;
using System;

namespace Apex.Websites.ViewModels.Logs
{
    public sealed class SearchActivityLogViewModel : DataTablesRequest
    {
        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public string UserName { get; set; }

        public string IP { get; set; }

        public override void ParseFormData(IFormCollection formData)
        {
            base.ParseFormData(formData);

            if (string.IsNullOrEmpty(SortColumnName))
            {
                SortColumnName = "createdon";
            }
        }
    }
}
