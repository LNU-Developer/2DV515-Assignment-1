using TinyCsvParser.Mapping;
using Backend.Models.Database;
namespace Backend.Models.CsvMappings
{
    class MovieCsvMapping : CsvMapping<Movie>
    {
        public MovieCsvMapping()
            : base()
        {
            MapProperty(0, x => x.MovieId);
            MapProperty(1, x => x.MovieTitle);
            MapProperty(2, x => x.MovieYear);
        }
    }
}
