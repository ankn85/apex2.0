using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Apex.Websites.ViewModels.Logs
{
    public sealed class UpdateActivityLogTypeViewModel
    {
        [HiddenInput]
        [Range(1, int.MaxValue)]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public bool Enabled { get; set; }
    }
}
