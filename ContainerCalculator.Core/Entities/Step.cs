using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ContainerCalculator.Core.Entities
{
    [DataContract]
    public class Step
    {
        [DataMember]
        public List<StepDetail> Details { get; set; }

        [DataMember]
        public int PackedCount { get; set; }

        [DataMember]
        public decimal PercentContainerVolumePacked { get; set; }

        [DataMember]
        public decimal PackedVolume { get; set; }

        [DataMember]
        public decimal PercentItemVolumePacked { get; set; }
        [DataMember]
        public decimal PercentItemPacked { get; set; }

    }
}
