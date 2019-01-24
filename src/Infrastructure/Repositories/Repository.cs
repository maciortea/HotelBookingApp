namespace Infrastructure.Repositories
{
    public abstract class Repository
    {
        protected readonly ApplicationDbContext _db;

        public Repository(ApplicationDbContext db)
        {
            _db = db;
        }
    }
}
