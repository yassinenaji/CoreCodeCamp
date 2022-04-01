using AutoMapper;
using CoreCodeCamp.Models;

namespace CoreCodeCamp.Data
{
    public class CampProfile:Profile
    {

        public CampProfile()
        {
            this.CreateMap<Camp, CampModel>().ReverseMap();
            //    .ForMember(c=>c.Venue,o=>o.MapFrom(m=>m.Location.VenueName));
        }
    }
}
