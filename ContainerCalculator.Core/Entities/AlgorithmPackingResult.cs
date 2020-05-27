using System.Collections.Generic;
using System.Runtime.Serialization;
// ReSharper disable InconsistentNaming

namespace ContainerCalculator.Core.Entities
{
    [DataContract]
    public class AlgorithmPackingResult
    {
        #region Public Properties

        [DataMember]
        public int AlgorithmID { get; set; }

        [DataMember]
        public string AlgorithmName { get; set; }

        [DataMember]
        public bool IsCompletePack { get; set; }

        [DataMember]
        public List<Item> PackedItems { get; set; }

        [DataMember]
        public long PackTimeInMilliseconds { get; set; }

        [DataMember]
        public decimal PercentContainerVolumePacked { get; set; }

        [DataMember]
        public decimal PercentItemVolumePacked { get; set; }

        [DataMember]
        public List<Item> UnpackedItems { get; set; }

        #endregion Public Properties

        #region Constructors
        
        public AlgorithmPackingResult()
        {
            PackedItems = new List<Item>();
            UnpackedItems = new List<Item>();
        }

        #endregion Constructors
    }
}
