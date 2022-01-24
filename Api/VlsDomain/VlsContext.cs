using Api.VlsDomain.EntityModel;
using Microsoft.EntityFrameworkCore;

namespace Api.VlsDomain
{
    public class VlsContext : DbContext, IVlsContext
    {
        public VlsContext(DbContextOptions dbContextOptions) : base(dbContextOptions) { }
        public VlsContext() { }

        public DbSet<EpcisObjectEvent>? EpcisObjectEvents { get; set; }
    }
}
