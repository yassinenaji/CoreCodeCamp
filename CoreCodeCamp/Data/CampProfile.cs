using AutoMapper;
using CoreCodeCamp.Models;

namespace CoreCodeCamp.Data
{
    public class CampProfile:Profile
    {

        public CampProfile()
        {
          var camp=  this.CreateMap<Camp, CampModel>().ReverseMap();
         var talk=   this.CreateMap<Talk, TalkModel>().ReverseMap();
       
            //    .ForMember(c=>c.Venue,o=>o.MapFrom(m=>m.Location.VenueName));
        }
    }
}
