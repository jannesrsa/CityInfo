namespace CityInfo.API.Models
{
    public class PointOfInterestDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        //public static PointOfInterestDto CreateFromPointOfInterest(Entities.PointOfInterest poi)
        //{
        //    var returnPoi = new PointOfInterestDto
        //    {
        //        Id = poi.Id,
        //        Description = poi.Description,
        //        Name = poi.Name
        //    };

        //    return returnPoi;
        //}
    }
}