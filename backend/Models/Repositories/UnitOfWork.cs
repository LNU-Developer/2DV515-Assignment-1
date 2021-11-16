using Backend.Models.Database;
using System.Threading.Tasks;

namespace Backend.Models.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly Context _context;

        public UnitOfWork(Context context)
        {
            _context = context;
            Movies = new MovieRepository(_context);
            Ratings = new RatingRepository(_context);
            Users = new UserRepository(_context);
        }

        public IMovieRepository Movies { get; private set; }
        public IRatingRepository Ratings { get; private set; }
        public IUserRepository Users { get; private set; }

        public Task<int> CompleteAsync()
        {
            return _context.SaveChangesAsync();
        }
        public int Complete()
        {
            return _context.SaveChanges();
        }
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}