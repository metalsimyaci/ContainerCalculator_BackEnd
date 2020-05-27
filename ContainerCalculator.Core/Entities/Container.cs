using System;
using System.Runtime.Serialization;
// ReSharper disable InconsistentNaming

namespace ContainerCalculator.Core.Entities
{
    [Serializable]
    [DataContract]
    public class Container
    {
        #region Private Variables

        private decimal volume;

        #endregion Private Variables

        #region Constructors

        public Container(int id, decimal length, decimal width, decimal height)
        {
            ID = id;
            Length = length;
            Width = width;
            Height = height;
            volume = length * width * height;
        }

        #endregion Constructors

        #region Public Properties

        [DataMember]
        public int ID { get; set; }

        [DataMember]
        public decimal Length { get; set; }

        [DataMember]
        public decimal Width { get; set; }

        [DataMember]       
        public decimal Height { get; set; }

        [DataMember]
        public decimal Volume => volume;

        #endregion Public Properties
    }
}
