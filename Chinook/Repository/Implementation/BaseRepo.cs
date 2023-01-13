namespace Chinook.Repository
{
    public class BaseRepo
    {
        protected ChinookContext _dbContext = null;

        public BaseRepo(ChinookContext dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
