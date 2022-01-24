using Api.VlsDomain;
using Api.VlsDomain.EntityModel;
using Api.VlsDomain.Repository;
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
        private readonly EpcisObjectEventRepository epcisObjectEventRepository;

        public EpcisObjectEventController(
            ILogger<EpcisObjectEventController> logger,
            EpcisObjectEventRepository epcisObjectEventRepository)
        {
            this._logger = logger;
            this.epcisObjectEventRepository = epcisObjectEventRepository;
        }

        [HttpGet(Name = "GetEpcisObjectEvents")]
        [Authorize]
        public IEnumerable<EpcisObjectEvent> Get()
        {
            return epcisObjectEventRepository.GetFiftyEntries();
        }
    }
}