using System.Collections.Generic;

namespace CityInfo.API.Models
{
    public class CityDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int NumberOfPointsOfInterest => PointsOfInterest.Count;
        public ICollection<PointOfInterestDto> PointsOfInterest { get; set; } = new List<PointOfInterestDto>();

        public static CityDto CreateFromCity(Entities.City cityEntity)
        {
            var returnResult = new CityDto
            {
                Id = cityEntity.Id,
                Description = cityEntity.Description,
                Name = cityEntity.Name
            };

            foreach (var poi in cityEntity.PointsOfInterest)
            {
                returnResult.PointsOfInterest.Add(PointOfInterestDto.CreateFromPointOfInterest(poi));
            }

            return returnResult;
        }
    }
}