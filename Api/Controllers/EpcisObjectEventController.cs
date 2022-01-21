using Api.VlsDomain;
using Api.VlsDomain.EntityModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EpcisObjectEventController : ControllerBase
    {
        private readonly ILogger<EpcisObjectEventController> _logger;
        private readonly VlsContext _vlsContext;

        public EpcisObjectEventController(
            ILogger<EpcisObjectEventController> logger,
            VlsContext vlsContext)
        {
            _logger = logger;
            _vlsContext = vlsContext;
        }

        [HttpGet(Name = "GetEpcisObjectEvents")]
        [Authorize]
        public IEnumerable<EpcisObjectEvent> Get()
        {
            return _vlsContext.EpcisObjectEvents.Take(50).ToList();
        }
    }
}