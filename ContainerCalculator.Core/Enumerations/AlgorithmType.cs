using System.Runtime.Serialization;
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming

namespace ContainerCalculator.Core.Enumerations
{
    [DataContract]
    public enum AlgorithmType
    {
        [DataMember]
        EB_AFIT = 1
    }
}