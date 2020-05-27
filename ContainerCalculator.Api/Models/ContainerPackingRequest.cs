using System.Collections.Generic;
using ContainerCalculator.Core.Entities;

namespace ContainerCalculator.Api.Models
{
    public class ContainerPackingRequest
    {
        public List<Container> Containers { get; set; }

        public List<Item> ItemsToPack { get; set; }        
    }
}