using TinyCsvParser.Mapping;
using Backend.Models.Database;
namespace Backend.Models.CsvMappings
{
    class RatingCsvMapping : CsvMapping<Rating>
    {
        public RatingCsvMapping()
            : base()
        {
            MapProperty(0, x => x.UserId);
            MapProperty(1, x => x.MovieId);
            MapProperty(2, x => x.Score);
        }
    }
}
