using ParkyApi.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyApi.Models.DTOs
{
    public class TrailCreateDto
    {  
        [Required]
        public string Name { get; set; }
        [Required]
        public double Distance { get; set; }
        [Required]
        public double Elevation { get; set; }
        public DifficultyType Difficulty { get; set; }
        public int NationalParkId { get; set; }  
    }
}
