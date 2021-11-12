using TinyCsvParser.Mapping;
using Backend.Models.Database;

namespace Backend.Models.CsvMappings
{
    class UserCsvMapping : CsvMapping<User>
    {
        public UserCsvMapping()
            : base()
        {
            MapProperty(0, x => x.UserId);
            MapProperty(1, x => x.UserName);
        }
    }
}
