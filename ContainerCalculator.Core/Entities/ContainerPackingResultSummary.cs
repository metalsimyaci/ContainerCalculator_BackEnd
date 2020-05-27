using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ContainerCalculator.Core.Entities
{
    [DataContract]
    public class ContainerPackingResultSummary
    {
        [DataMember]
        public int ContainerId { get; set; }

        [DataMember]
        public int TotalContainerCount { get; set; }
        
        [DataMember]
        public bool IsCompletePack { get; set; }
        
        [DataMember]
        public long PackTimeInMilliseconds { get; set; }

        [DataMember]
        public decimal PercentContainerVolumePacked { get; set; }

        [DataMember]
        public decimal PercentItemVolumePacked { get; set; }

        [DataMember]
        public int TotalPackages { get; set; }

        [DataMember]
        public int PackedItemsCount { get; set; }

        [DataMember]
        public decimal PackedItemsVolume { get; set; }

        [DataMember]
        public int UnPackedItemsCount { get; set; }

        [DataMember]
        public List<Step> Steps { get; set; }

        [DataMember]
        public bool IsError { get; set; }

        [DataMember]
        public string Message { get; set; }

        [DataMember]
        public List<Item> InvalidPackedItems { get; set; }
    }
}
