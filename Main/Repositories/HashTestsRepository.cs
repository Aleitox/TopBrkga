using Main.Entities;
using Main.Model;
using System.Data.Entity;

namespace Main.Repositories
{
    public class HashTestsRepository : Repository<HashTest>
    {
        public HashTestsRepository(DbContext dataContext) : base(dataContext)
        {
        }

        public void SaveHashTest(HashTest hashTest)
        {
            Insert(hashTest);
            SaveChanges();
        }
    }
}
