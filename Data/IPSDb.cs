using Microsoft.EntityFrameworkCore;
using WidmyIPS.Models;

namespace WidmyIPS.Data
{
    public class IPSDb : DbContext
    {
        public IPSDb(DbContextOptions<IPSDb> options) : base(options)
        {

        }
        public DbSet<IPS> IPSs => Set<IPS>();
    }
}