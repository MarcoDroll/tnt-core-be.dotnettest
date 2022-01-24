using Api.VlsDomain.EntityModel;
using Microsoft.EntityFrameworkCore;

namespace Api.VlsDomain
{
    public interface IVlsContext
    {
        DbSet<EpcisObjectEvent>? EpcisObjectEvents { get; set; }
    }
}