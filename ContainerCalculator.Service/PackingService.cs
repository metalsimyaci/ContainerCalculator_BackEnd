using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ContainerCalculator.Core.Abstracts;
using ContainerCalculator.Core.Algorithms;
using ContainerCalculator.Core.Entities;
using ContainerCalculator.Core.Enumerations;

namespace ContainerCalculator.Service
{
    /// <summary>
    /// The container packing service.
    /// </summary>
    public static class PackingService
    {
        /// <summary>
		/// Attempts to pack the specified containers with the specified items using the specified algorithms.
		/// </summary>
		/// <param name="containers">The list of containers to pack.</param>
		/// <param name="itemsToPack">The items to pack.</param>
		/// <param name="algorithmTypeIDs">The list of algorithm type IDs to use for packing.</param>
		/// <returns>A container packing result with lists of the packed and unpacked items.</returns>
		public static List<ContainerPackingResult> Pack(List<Container> containers, List<Item> itemsToPack, List<int> algorithmTypeIDs)
        {
            Object sync = new Object { };
            List<ContainerPackingResult> result = new List<ContainerPackingResult>();

            Parallel.ForEach(containers, container =>
            {
                ContainerPackingResult containerPackingResult = new ContainerPackingResult();
                containerPackingResult.ContainerID = container.ID;

                Parallel.ForEach(algorithmTypeIDs, algorithmTypeID =>
                {
                    IPackingAlgorithm algorithm = PackingService.GetPackingAlgorithmFromTypeID(algorithmTypeID);

                    // Until I rewrite the algorithm with no side effects, we need to clone the item list
                    // so the parallel updates don't interfere with each other.
                    List<Item> items = new List<Item>();

                    itemsToPack.ForEach(item =>
                    {
                        items.Add(new Item(item.ID, item.Dim1, item.Dim2, item.Dim3, item.Quantity));
                    });

                    Stopwatch stopwatch = new Stopwatch();
                    stopwatch.Start();
                    AlgorithmPackingResult algorithmResult = algorithm.Run(container, items);
                    stopwatch.Stop();

                    algorithmResult.PackTimeInMilliseconds = stopwatch.ElapsedMilliseconds;

                    decimal containerVolume = container.Length * container.Width * container.Height;
                    decimal itemVolumePacked = algorithmResult.PackedItems.Sum(i => i.Volume);
                    decimal itemVolumeUnpacked = algorithmResult.UnpackedItems.Sum(i => i.Volume);

                    algorithmResult.PercentContainerVolumePacked = Math.Round(itemVolumePacked / containerVolume * 100, 2);
                    algorithmResult.PercentItemVolumePacked = Math.Round(itemVolumePacked / (itemVolumePacked + itemVolumeUnpacked) * 100, 2);

                    lock (sync)
                    {
                        containerPackingResult.AlgorithmPackingResults.Add(algorithmResult);
                    }
                });

                containerPackingResult.AlgorithmPackingResults = containerPackingResult.AlgorithmPackingResults.OrderBy(r => r.AlgorithmName).ToList();

                lock (sync)
                {
                    result.Add(containerPackingResult);
                }
            });

            return result;
        }
        /// <summary>
        /// Gets the packing algorithm from the specified algorithm type ID.
        /// </summary>
        /// <param name="algorithmTypeID">The algorithm type ID.</param>
        /// <returns>An instance of a packing algorithm implementing AlgorithmBase.</returns>
        /// <exception cref="System.Exception">Invalid algorithm type.</exception>
        public static IPackingAlgorithm GetPackingAlgorithmFromTypeID(int algorithmTypeID)
        {
            switch (algorithmTypeID)
            {
                case (int)AlgorithmType.EB_AFIT:
                    return new EB_AFIT();

                default:
                    throw new Exception("Invalid algorithm type.");
            }
        }
        public static ContainerPackingResultSummary Packing(Container container, List<Item> itemsToPack, CancellationToken cancellationToken)
        {
            try
            {
                IPackingAlgorithm algorithm = PackingService.GetPackingAlgorithmFromTypeID((int)AlgorithmType.EB_AFIT);
                if (!itemsToPack.Any())
                    return new ContainerPackingResultSummary
                    {
                        ContainerId = container.ID,
                        IsError = true,
                        Message = $"Packages not found",
                        IsCompletePack = false
                    };
                List<Item> items = new List<Item>();
                List<Item> invalidPackedItems = new List<Item>();
                itemsToPack.ForEach(item =>
                {
                    if ((container.Height < item.Dim1 || container.Height < item.Dim2 || container.Height < item.Dim3) ||
                       (container.Length < item.Dim1 || container.Length < item.Dim2 || container.Length < item.Dim3) ||
                       (container.Width < item.Dim1 || container.Width < item.Dim2 || container.Width < item.Dim3))
                        invalidPackedItems.Add(new Item(item.ID, item.Dim1, item.Dim2, item.Dim3, item.Quantity));
                    else
                        items.Add(new Item(item.ID, item.Dim1, item.Dim2, item.Dim3, item.Quantity));
                });
                if (!items.Any())
                    return new ContainerPackingResultSummary
                    {
                        ContainerId = container.ID,
                        IsError = true,
                        IsCompletePack = false,
                        Message = $"Packages dimensions are invalid",
                        InvalidPackedItems = invalidPackedItems
                    };
                int totalPackeds = items.Sum(x => x.Quantity);
                var groupedItems = items.GroupBy(x => x.ID).Select(g => new
                {
                    Key = g.Key,
                    Count = g.Sum(y => y.Quantity)
                }).ToLookup(i => i.Key, i => i.Count);
                ContainerPackingResultSummary res = new ContainerPackingResultSummary
                {
                    ContainerId = container.ID,
                    TotalPackages = totalPackeds,
                    TotalContainerCount = 0,
                    UnPackedItemsCount = 0,
                    Steps = new List<Step>()
                };
                decimal cVolume = container.Length * container.Width * container.Height;
                int lastUnPackedItemsCount = 0;

                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                try
                {
                    while (true)
                    {
                        if (cancellationToken.IsCancellationRequested)
                            throw new OperationCanceledException("Request time out");

                        AlgorithmPackingResult algorithmResult = algorithm.Run(container, items);
                        res.TotalContainerCount++;

                        decimal iVolumePacked = algorithmResult.PackedItems.Sum(i => i.Volume);
                        decimal iVolumeUnpacked = algorithmResult.UnpackedItems.Sum(i => i.Volume);
                        int packedCount = algorithmResult.PackedItems.Sum(x => x.Quantity);
                        res.PackedItemsCount += packedCount;
                        res.PackedItemsVolume += iVolumePacked;
                        res.Steps.Add(new Step
                        {
                            PackedCount = packedCount,
                            PackedVolume = iVolumePacked,
                            PercentContainerVolumePacked = Math.Round(iVolumePacked / cVolume * 100, 2),
                            PercentItemVolumePacked =
                                Math.Round(iVolumePacked / (iVolumePacked + iVolumeUnpacked) * 100, 2),
                            PercentItemPacked = Math.Round(packedCount / (decimal)totalPackeds * 100, 2),
                            Details = algorithmResult.PackedItems.GroupBy(x => x.ID)
                                .Select(x => new StepDetail
                                {
                                    PackedCount = x.Sum(y => y.Quantity),
                                    ItemId = x.Key,
                                    PercentPackedItem =
                                        Math.Round((decimal)x.Sum(y => y.Quantity) / groupedItems[x.Key].First() * 100,
                                            2)
                                }).ToList()
                        });
                        res.IsCompletePack = algorithmResult.IsCompletePack;
                        if (res.IsCompletePack)
                            break;
                        if (!algorithmResult.UnpackedItems.Any() ||
                            algorithmResult.UnpackedItems.Count == lastUnPackedItemsCount)
                        {
                            res.IsError = true;
                            res.Message = "Process completed with error";
                            res.UnPackedItemsCount = lastUnPackedItemsCount;

                            break;
                        }

                        lastUnPackedItemsCount = algorithmResult.UnpackedItems.Count;
                        items = algorithmResult.UnpackedItems;
                        if (res.TotalContainerCount > 50)
                            throw new Exception("No calculation for more than 50 containers");

                    }
                }
                catch (OperationCanceledException e)
                {
                    res.IsError = true;
                    res.Message = "Request time out";
                    res.UnPackedItemsCount = lastUnPackedItemsCount;
                    return res;
                }
                catch (Exception e)
                {
                    res.IsError = true;
                    res.Message = e.Message;
                    res.UnPackedItemsCount = lastUnPackedItemsCount;
                    return res;
                }

                stopwatch.Stop();
                res.PackTimeInMilliseconds = stopwatch.ElapsedMilliseconds;
                res.PercentContainerVolumePacked =
                    Math.Round(res.PackedItemsVolume / (cVolume * res.TotalContainerCount) * 100, 2);
                res.PercentItemVolumePacked = Math.Round(res.PackedItemsCount / (decimal)totalPackeds * 100, 2);

                return res;


            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
