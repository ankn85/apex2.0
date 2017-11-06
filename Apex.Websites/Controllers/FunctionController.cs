using Apex.Data.Entities.Securities;
using Apex.Services.Securities;
using Apex.Websites.ViewModels.Securities;
using AutoMapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Apex.Websites.Controllers
{
    public class FunctionController : BaseAdminController
    {
        private readonly IMapper _mapper;
        private readonly IFunctionService _functionService;

        public FunctionController(
            IMapper mapper,
            IFunctionService functionService)
        {
            _mapper = mapper;
            _functionService = functionService;
        }

        public async Task<IActionResult> Index()
        {
            var functions = await _functionService.GetHierarchicalListAsync();

            return View(functions);
        }

        public async Task<IActionResult> Create()
        {
            var viewModel = new UpdateFunctionViewModel
            {
                Enabled = true
            };

            await AssignHierarchicalFunctionsAsync(viewModel);

            return View("Update", viewModel);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UpdateFunctionViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var entity = _mapper.Map<Function>(viewModel);

                if (entity.ParentId == 0)
                {
                    entity.ParentId = null;
                }

                await _functionService.CreateAsync(entity);

                return RedirectToAction(nameof(Index));
            }

            await AssignHierarchicalFunctionsAsync(viewModel);

            return View("Update", viewModel);
        }

        public async Task<IActionResult> Update(int id)
        {
            var entity = await _functionService.FindAsync(id);

            if (entity == null)
            {
                return RedirectToAction(nameof(Index));
            }

            var viewModel = _mapper.Map<UpdateFunctionViewModel>(entity);
            await AssignHierarchicalFunctionsAsync(viewModel);

            return View("Update", viewModel);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(UpdateFunctionViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var entity = await _functionService.FindAsync(viewModel.Id);

                if (entity == null)
                {
                    return RedirectToAction(nameof(Index));
                }

                var updatedEntity = _mapper.Map(viewModel, entity);

                if (updatedEntity.ParentId == 0)
                {
                    updatedEntity.ParentId = null;
                }

                await _functionService.UpdateAsync(updatedEntity);

                return RedirectToAction(nameof(Index));
            }

            await AssignHierarchicalFunctionsAsync(viewModel);

            return View("Update", viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int[] ids)
        {
            var entities = await _functionService.FindAsync(ids);

            if (entities.Any())
            {
                foreach (var item in entities)
                {
                    var hasSubFunctions = await _functionService.HasSubFunctions(item.Id);
                    
                    if (hasSubFunctions)
                    {
                        ModelState.TryAddModelError(
                            string.Empty,
                            $"Cannot delete function {item.Name} because it has sub functions.");
                    }
                    else
                    {
                        await _functionService.DeleteAsync(item);
                    }
                }
            }

            return Ok();
        }

        private async Task AssignHierarchicalFunctionsAsync(UpdateFunctionViewModel viewModel)
        {
            IList<SelectListItem> functionItems = new List<SelectListItem>
            {
                new SelectListItem { Value = "0", Text = "None" }
            };

            var hierarchicalFunctions = await _functionService.GetHierarchicalListAsync();

            foreach (var item in hierarchicalFunctions)
            {
                functionItems.Add(new SelectListItem { Value = item.Id.ToString(), Text = item.Name });

                BuildFunctionItems(item, functionItems, item.Name);
            }

            viewModel.HierarchicalFunctions = functionItems;
        }

        private void BuildFunctionItems(Function parent, IList<SelectListItem> functionItems, string parentName)
        {
            parentName += " » ";
            var subFunctions = parent.SubFunctions.OrderBy(m => m.Priority);
            SelectListItem selectListItem;

            foreach (var item in subFunctions)
            {
                selectListItem = new SelectListItem
                {
                    Value = item.Id.ToString(),
                    Text = parentName + item.Name
                };

                functionItems.Add(selectListItem);

                BuildFunctionItems(item, functionItems, selectListItem.Text);
            }
        }
    }
}
