using Apex.Services.Logs;
using Apex.Websites.ViewModels.DataTables;
using Apex.Websites.ViewModels.Logs;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Apex.Websites.Controllers
{
    public class LogController : BaseAdminController
    {
        private readonly ILogService _logService;

        public LogController(ILogService logService)
        {
            _logService = logService;
        }

        public IActionResult Index()
        {
            PopulateLevels();

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Search(SearchLogViewModel viewModel)
        {
            viewModel.ParseFormData(Request.Form);

            var pagedList = await _logService.GetPagedListAsync(
                viewModel.FromDate,
                viewModel.ToDate,
                viewModel.Levels,
                viewModel.SortColumnName,
                viewModel.SortDirection,
                viewModel.Start,
                viewModel.Length);

            return viewModel.CreateResponse(
                pagedList.TotalRecords,
                pagedList.TotalRecordsFiltered,
                pagedList);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int[] ids)
        {
            var entities = await _logService.FindAsync(ids);

            if (entities.Any())
            {
                await _logService.DeleteAsync(entities);
            }

            return Ok();
        }

        private void PopulateLevels()
        {
            IEnumerable<SelectListItem> levels = new List<SelectListItem>
            {
                new SelectListItem { Value = "Fatal", Text = "Fatal" },
                new SelectListItem { Value = "Error", Text = "Error", Selected = true },
                new SelectListItem { Value = "Warn", Text = "Warn" },
                new SelectListItem { Value = "Info", Text = "Info" },
                new SelectListItem { Value = "Debug", Text = "Debug" },
                new SelectListItem { Value = "Trace", Text = "Trace" }
            };

            ViewData["Levels"] = levels;
        }
    }
}
