﻿using System.ComponentModel.DataAnnotations;

namespace magicvilla_villaapi.models.Dto
{
    public class VillaupdateDTO
    {
        [Required]
        public int ID { get; set; }
        [Required]
        [MaxLength(30)]
        public string Name { get; set; }
        public string Details { get; set; }
        [Required]
        public double Rate { get; set; }
        [Required]
        public int Occupancy { get; set; }
        [Required]
        public int Sqrt { get; set; }
        [Required]
        public string ImageUrl { get; set; }
        public string Amenity { get; set; }
    }
}