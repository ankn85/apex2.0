using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Apex.Websites.ViewModels.Securities
{
    public sealed class UpdateFunctionViewModel
    {
        [HiddenInput]
        [Range(0, int.MaxValue)]
        public int Id { get; set; }

        [Display(Name = "Parent")]
        public int? ParentId { get; set; }

        [Required]
        public string Name { get; set; }

        public int Priority { get; set; }

        public bool Enabled { get; set; }

        public IEnumerable<SelectListItem> HierarchicalFunctions { get; set; }
    }
}