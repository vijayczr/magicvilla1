using System.ComponentModel.DataAnnotations;

namespace magicvilla_villaapi.models.Dto
{
    public class VillaDTO
    {
        public int ID { get; set; }
        [Required]
        [MaxLength(30)]
        public string Name { get; set; }
        public string Details { get; set; }
        [Required]
        public double Rate { get; set; }
        public int Occupancy { get; set; }
        public int Sqrt { get; set; }
        public string ImageUrl { get; set; }
        public string Amenity { get; set; }
    }
}
