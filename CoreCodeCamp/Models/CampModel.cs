using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CoreCodeCamp.Models
{
    public class CampModel
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        [Required]
        public string Moniker { get; set; }
        public DateTime EventDate { get; set; } = DateTime.MinValue;
        [Range(1,101)]
        public int Length { get; set; } = 1;

        /*To map an object in the same class, each field must start with the name of the object
            so that the automapper can map the object with the class */
         public string LocationVenueName { get; set; }

        //we using here a diffrent name to show how to bind the model with the entity in CampProfile
        //public string Venue { get; set; }
        public string LocationAddress1 { get; set; }
        public string LocationAddress2 { get; set; }
        public string LocationAddress3 { get; set; }
        public string LocationCityTown { get; set; }
        public string LocationStateProvince { get; set; }
        public string LocationPostalCode { get; set; }
        public string LocationCountry { get; set; }
        public SpeakerModel Speaker { get; set; }
        public ICollection<TalkModel> Talks { get; set; }


    }
}
