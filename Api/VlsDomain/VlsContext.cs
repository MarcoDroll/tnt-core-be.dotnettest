using Api.VlsDomain.EntityModel;
using Microsoft.EntityFrameworkCore;

namespace Api.VlsDomain
{
    public class VlsContext : DbContext
    {
        public VlsContext(DbContextOptions options) : base(options) {}

        public DbSet<EpcisObjectEvent> EpcisObjectEvents { get; set; }
    }
}
