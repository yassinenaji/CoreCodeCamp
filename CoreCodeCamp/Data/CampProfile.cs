using AutoMapper;
using CoreCodeCamp.Models;

namespace CoreCodeCamp.Data
{
    public class CampProfile:Profile
    {

        public CampProfile()
        {
          var camp=  this.CreateMap<Camp, CampModel>().ReverseMap()/*.ForMember(c=>c.Venue,o=>o.MapFrom(m=>m.Location.VenueName))*/;
         var talk=   this.CreateMap<Talk, TalkModel>().ReverseMap();
         var speaker=   this.CreateMap<Speaker , SpeakerModel>().ReverseMap();
       
            
        }
    }
}
