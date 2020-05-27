using System.Collections.Generic;
using System.Runtime.Serialization;
// ReSharper disable InconsistentNaming

namespace ContainerCalculator.Core.Entities
{
    [DataContract]
    public class ContainerPackingResult
    {
        #region Constructors

        public ContainerPackingResult()
        {
            AlgorithmPackingResults = new List<AlgorithmPackingResult>();
        }

        #endregion Constructors

        #region Public Properties

        [DataMember]
        public int ContainerID { get; set; }

        [DataMember]
        public List<AlgorithmPackingResult> AlgorithmPackingResults { get; set; }

        #endregion Public Properties
    }
}
