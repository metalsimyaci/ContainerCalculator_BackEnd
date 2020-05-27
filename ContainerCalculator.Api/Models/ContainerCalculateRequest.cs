using System.Collections.Generic;
using ContainerCalculator.Core.Entities;

namespace ContainerCalculator.Api.Models
{
    public class ContainerCalculateRequest
    {
        public Container Container { get; set; }

        public List<Item> ItemsToPack { get; set; }   
    }
}