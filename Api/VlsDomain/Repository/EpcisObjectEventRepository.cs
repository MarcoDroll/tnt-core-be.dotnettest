using Api.VlsDomain.EntityModel;

namespace Api.VlsDomain.Repository
{
    public class EpcisObjectEventRepository
    {
        private readonly IVlsContext _vlsContext;

        public EpcisObjectEventRepository(IVlsContext vlsContext)
        {
            _vlsContext = vlsContext;
        }

        public List<EpcisObjectEvent> GetFiftyEntries()
        {
            return this._vlsContext.EpcisObjectEvents?.Take(50).ToList() ?? new List<EpcisObjectEvent>();
        }
    }
}
