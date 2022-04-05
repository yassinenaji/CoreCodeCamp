﻿using CoreCodeCamp.Data;
using System.ComponentModel.DataAnnotations;

namespace CoreCodeCamp.Models
{
    public class TalkModel
    {

        //public Camp Camp { get; set; }
        [Required]
        [StringLength(100)]
        public string Title { get; set; }
        [Required]
        [StringLength(4000,MinimumLength =20)]
        public string Abstract { get; set; }
        [Range(100,300)]
        public int Level { get; set; }
        public Speaker Speaker { get; set; }

    }
}
