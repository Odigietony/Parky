using Microsoft.EntityFrameworkCore;
using ParkyApi.Data;
using ParkyApi.Models;
using ParkyApi.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyApi.Repository.Implementations
{
    public class TrailRepository : ITrailRepository
    {
        private readonly ApplicationDbContext _context;
        public TrailRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public bool CreateTrail(Trail trail)
        {
            _context.Trails.Add(trail);
            return Save();
        }

        public bool DeleteTrail(Trail trail)
        {
            _context.Trails.Remove(trail);
            return Save();
        }

        public Trail GetTrail(int trailId)
        {
            return _context.Trails.Include(trail => trail.NationalPark).FirstOrDefault(trail => trail.Id == trailId);
        }

        public ICollection<Trail> GetTrails()
        {
            return _context.Trails.Include(trail => trail.NationalPark).OrderBy(trail => trail.Name).ToList();
        }

        public bool TrailExists(string name)
        {
            return _context.Trails.Any(trail => trail.Name.ToLower().Trim() == name.ToLower().Trim());
        }

        public bool TrailExists(int id)
        {
            return _context.Trails.Any(trail => trail.Id == id);
        }

        public bool Save()
        {
            return _context.SaveChanges() >= 0 ? true : false;
        }

        public bool UpdateTrail(Trail trail)
        {
            _context.Trails.Update(trail);
            return Save();
        }

        public ICollection<Trail> GetTrailsInNationalPark(int parkId)
        {
            return _context.Trails.Include(trail => trail.NationalPark)
                .Where(trail => trail.NationalParkId == parkId).ToList();
        }
    }
}
