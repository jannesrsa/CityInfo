using System.Collections.Generic;
using CityInfo.API.Models;

namespace CityInfo.API
{
    public class CitiesDataStore
    {
        public CitiesDataStore()
        {
            Cities = new List<CityDto>()
            {
                new CityDto
                {
                    Id =1,
                    Name ="NYC" ,
                    Description ="NYC Desc"
                },
                new CityDto
                {
                   Id =2,
                   Name ="Cape Town",
                   Description ="NYC Desc"
               }
            };
        }

        public static CitiesDataStore Current { get; } = new CitiesDataStore();

        public List<CityDto> Cities { get; set; }

    }
}