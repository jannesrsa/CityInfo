namespace CityInfo.API.Models
{
    public class CityWithoutPointsOfInterestDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        //public static CityWithoutPointsOfInterestDto CreateFromCity(Entities.City cityEntity)
        //{
        //    return new CityWithoutPointsOfInterestDto
        //    {
        //        Id = cityEntity.Id,
        //        Description = cityEntity.Description,
        //        Name = cityEntity.Name
        //    };
        //}
    }
}