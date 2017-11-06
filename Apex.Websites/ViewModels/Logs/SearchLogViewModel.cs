using Apex.Websites.ViewModels.DataTables;
using Microsoft.AspNetCore.Http;
using System;

namespace Apex.Websites.ViewModels.Logs
{
    public sealed class SearchLogViewModel : DataTablesRequest
    {
        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public string[] Levels { get; set; }

        public override void ParseFormData(IFormCollection formData)
        {
            base.ParseFormData(formData);

            if (string.IsNullOrEmpty(SortColumnName))
            {
                SortColumnName = "logged";
            }
        }
    }
}
