using System.Collections.Generic;
using ContainerCalculator.Core.Entities;

namespace ContainerCalculator.Core.Abstracts
{
    public interface IPackingAlgorithm
    {
        AlgorithmPackingResult Run(Container container, List<Item> items);
    }
}