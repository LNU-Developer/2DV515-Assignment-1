using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Backend.Models.Database
{
    public partial class User
    {
        public User()
        {
            Ratings = new HashSet<Rating>();
        }
        public int UserId { get; set; }
        [Required]
        public string UserName { get; set; }
        public ICollection<Rating> Ratings { get; set; }
    }
}
