using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Backend.Models.Database
{
    public partial class Movie
    {
        public Movie()
        {
            Ratings = new HashSet<Rating>();
        }
        public int MovieId { get; set; }
        [Required]
        public string MovieTitle { get; set; }
        public int MovieYear { get; set; }

        public ICollection<Rating> Ratings { get; set; }

    }
}
