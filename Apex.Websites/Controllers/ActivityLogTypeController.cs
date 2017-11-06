using Apex.Services.Logs;
using Apex.Websites.ViewModels.DataTables;
using Apex.Websites.ViewModels.Logs;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Apex.Websites.Controllers
{
    public class ActivityLogTypeController : BaseAdminController
    {
        private readonly IMapper _mapper;
        private readonly IActivityLogTypeService _activityLogTypeService;

        public ActivityLogTypeController(
            IMapper mapper,
            IActivityLogTypeService activityLogTypeService)
        {
            _mapper = mapper;
            _activityLogTypeService = activityLogTypeService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Search(DataTablesRequest viewModel)
        {
            viewModel.ParseFormData(Request.Form);

            var pagedList = await _activityLogTypeService.GetPagedListAsync(
                viewModel.SortColumnName,
                viewModel.SortDirection);

            return viewModel.CreateResponse(
                pagedList.TotalRecords,
                pagedList.TotalRecordsFiltered,
                pagedList);
        }

        public async Task<IActionResult> Update(int id)
        {
            var entity = await _activityLogTypeService.FindAsync(id);

            if (entity == null)
            {
                return RedirectToAction(nameof(Index));
            }

            var viewModel = _mapper.Map<UpdateActivityLogTypeViewModel>(entity);

            return View("Update", viewModel);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(UpdateActivityLogTypeViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var entity = await _activityLogTypeService.FindAsync(viewModel.Id);

                if (entity == null)
                {
                    return RedirectToAction(nameof(Index));
                }

                var updatedEntity = _mapper.Map(viewModel, entity);
                await _activityLogTypeService.UpdateAsync(updatedEntity);

                return RedirectToAction(nameof(Index));
            }

            return View("Update", viewModel);
        }
    }
}