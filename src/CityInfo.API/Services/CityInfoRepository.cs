using System.Collections.Generic;
using System.Linq;
using CityInfo.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace CityInfo.API.Services
{
    public class CityInfoRepository : ICityInfoRepository
    {
        private readonly CityInfoContext _context;

        public CityInfoRepository(CityInfoContext context)
        {
            _context = context;
        }

        public bool CityExists(int cityId)
        {
            return _context.Cities
                    .Any(i => i.Id == cityId);
        }

        public IEnumerable<City> GetCities()
        {
            return _context.Cities
                .OrderBy(c => c.Name);
        }

        public City GetCity(int cityId, bool includePointsOfInterest)
        {
            if (includePointsOfInterest)
            {
                return _context.Cities
                    .Include(c => c.PointsOfInterest)
                    .Where(i => i.Id == cityId)
                    .FirstOrDefault();
            }

            return _context.Cities
                    .Where(i => i.Id == cityId)
                    .FirstOrDefault();
        }

        public PointOfInterest GetPointOfInterestForCity(int cityId, int pointOfInterestId)
        {
            return _context.PointsOfInterest
                     .Where(i => i.CityId == cityId && i.Id == pointOfInterestId)
                     .FirstOrDefault();
        }

        public IEnumerable<PointOfInterest> GetPointsOfInterestForCity(int cityId)
        {
            return _context.PointsOfInterest
                     .Where(i => i.CityId == cityId);
        }
    }
}