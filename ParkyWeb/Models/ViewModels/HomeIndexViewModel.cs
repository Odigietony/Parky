using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyWeb.Models.ViewModels
{
    public class HomeIndexViewModel
    {
        public IEnumerable<NationalPark> NationalParks { get; set; }
        public IEnumerable<Trail> Trails { get; set; }
    }
}
