using System.Runtime.Serialization;

namespace ContainerCalculator.Core.Entities
{
    [DataContract]
    public class StepDetail
    {
        [DataMember]
        public int ItemId { get; set; }

        [DataMember]
        public int PackedCount { get; set; }

        [DataMember]
        public decimal PercentPackedItem { get; set; }
    }
}
