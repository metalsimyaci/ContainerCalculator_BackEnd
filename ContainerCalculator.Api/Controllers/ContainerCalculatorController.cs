using System.Threading;
using System.Web.Http;
using ContainerCalculator.Api.Models;
using ContainerCalculator.Core.Abstracts;
using ContainerCalculator.Core.Entities;
using ContainerCalculator.Service;
using Microsoft.Web.Http;

namespace ContainerCalculator.Api.Controllers
{
    [ApiVersion( "1.0" )]
    [RoutePrefix( "v{version:apiVersion}/containercalculator" )]
    public class ContainerCalculatorController : ApiController
    {
        //[Route("{request}")]
        //[HttpPost]
        //public List<ContainerPackingResult> Calculate([FromBody] ContainerPackingRequest request)
        //{
        //    return PackingService.Pack(request.Containers, request.ItemsToPack, new List<int> {(int) AlgorithmType.EB_AFIT});
        //}

        [Route("{request}")]
        [HttpPost]
        public ContainerPackingResultSummary Calculate([FromBody] ContainerCalculateRequest request,CancellationToken cancellationToken=default(CancellationToken))
        {
            return PackingService.Packing(request.Container, request.ItemsToPack,cancellationToken);
        }

        [Route("{algorithmTypeId}")]
        [HttpGet]
        public IPackingAlgorithm Get(int algorithmTypeId)
        {
            return PackingService.GetPackingAlgorithmFromTypeID(algorithmTypeId);
        }
    }
}
