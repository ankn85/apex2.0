using Apex.Data.Entities.Emails;
using Apex.Services.Emails;
using Apex.Websites.ViewModels.DataTables;
using Apex.Websites.ViewModels.Emails;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace Apex.Websites.Controllers
{
    public class EmailAccountController : BaseAdminController
    {
        private readonly IMapper _mapper;
        private readonly IEmailAccountService _emailAccountService;
        //private readonly IActivityLogService _activityLogService;

        public EmailAccountController(
            IMapper mapper,
            IEmailAccountService emailAccountService/*,
            IActivityLogService activityLogService*/)
        {
            _mapper = mapper;
            _emailAccountService = emailAccountService;
            //_activityLogService = activityLogService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Search(DataTablesRequest viewModel)
        {
            viewModel.ParseFormData(Request.Form);

            var pagedList = await _emailAccountService.GetPagedListAsync(
                viewModel.SortColumnName,
                viewModel.SortDirection);

            return viewModel.CreateResponse(
                pagedList.TotalRecords,
                pagedList.TotalRecordsFiltered,
                pagedList);
        }

        public IActionResult Create()
        {
            return View("Update", new UpdateEmailAccountViewModel());
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UpdateEmailAccountViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var entity = _mapper.Map<EmailAccount>(viewModel);
                await _emailAccountService.CreateAsync(entity);

                // var activityLog = GetActivityLog(entity.GetType(), newValue: emailAccount);
                // await _activityLogService.CreateAsync(GetSystemKeyword(), activityLog);

                return RedirectToAction(nameof(Index));
            }

            return View("Update", viewModel);
        }

        public async Task<IActionResult> Update(int id)
        {
            var entity = await _emailAccountService.FindAsync(id);

            if (entity == null)
            {
                return RedirectToAction(nameof(Index));
            }

            var viewModel = _mapper.Map<UpdateEmailAccountViewModel>(entity);

            return View("Update", viewModel);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(UpdateEmailAccountViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                EmailAccount entity = await _emailAccountService.FindAsync(viewModel.Id);

                if (entity == null)
                {
                    return RedirectToAction(nameof(Index));
                }

                //var activityLog = GetActivityLog(entity.GetType(), oldValue: emailAccount);

                var updatedEntity = _mapper.Map(viewModel, entity);
                await _emailAccountService.UpdateAsync(updatedEntity);

                // activityLog.NewValue = ObjectToJson(updatedEntity);
                // await _activityLogService.CreateAsync(GetSystemKeyword(), activityLog);

                return RedirectToAction(nameof(Index));
            }

            return View("Update", viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int[] ids)
        {
            var entities = await _emailAccountService.FindAsync(ids);

            if (entities.Any())
            {
                await _emailAccountService.DeleteAsync(entities);

                // var activityLog = GetActivityLog(emailAccount.GetType(), newValue: emailAccount);
                // await _activityLogService.CreateAsync(GetSystemKeyword(), activityLog);
            }

            return Ok();
        }
    }
}