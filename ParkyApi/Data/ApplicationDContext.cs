using Microsoft.EntityFrameworkCore;
using ParkyApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyApi.Data
{
    public class ApplicationDContext : DbContext
    {
        public ApplicationDContext(DbContextOptions<ApplicationDContext> options) : base(options)
        {
        }
        DbSet<NationalPark> NationalParks { get; set; }
    }
}
