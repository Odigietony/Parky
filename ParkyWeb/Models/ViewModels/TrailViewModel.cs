using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyWeb.Models.ViewModels
{
    public class TrailViewModel
    {
        public IEnumerable<SelectListItem> NationalParks { get; set; }
        public Trail Trail { get; set; }
    }
}
