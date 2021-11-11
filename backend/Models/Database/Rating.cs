using System.ComponentModel.DataAnnotations;
namespace Backend.Models.Database
{
    public partial class Rating
    {
        public int RatingId { get; set; }

        [Required]
        public int UserId { get; set; }
        public virtual User User { get; set; }

        [Required]
        public int MovieId { get; set; }
        public virtual Movie Movie { get; set; }

        public double Score { get; set; }

    }
}