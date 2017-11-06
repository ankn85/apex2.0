using Apex.Services.Logs;
using Apex.Websites.ViewModels.DataTables;
using Apex.Websites.ViewModels.Logs;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace Apex.Websites.Controllers
{
    public class ActivityLogController : BaseAdminController
    {
        private readonly IActivityLogService _activityLogService;

        public ActivityLogController(IActivityLogService activityLogService)
        {
            _activityLogService = activityLogService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Search(SearchActivityLogViewModel viewModel)
        {
            viewModel.ParseFormData(Request.Form);

            var pagedList = await _activityLogService.GetPagedListAsync(
                viewModel.FromDate,
                viewModel.ToDate,
                viewModel.UserName,
                viewModel.IP,
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
            var entities = await _activityLogService.FindAsync(ids);

            if (entities.Any())
            {
                await _activityLogService.DeleteAsync(entities);
            }

            return Ok();
        }
    }
}