using System.Runtime.Serialization;
// ReSharper disable InconsistentNaming

namespace ContainerCalculator.Core.Entities
{
    [DataContract]
    public class Item
    {
        #region Private Variables

        private decimal volume;

        #endregion Private Variables

        #region Public Properties

		[DataMember]
		public int ID { get; set; }

		[DataMember]
		public bool IsPacked { get; set; }

		[DataMember]
		public decimal Dim1 { get; set; }

		[DataMember]
		public decimal Dim2 { get; set; }

		[DataMember]
		public decimal Dim3 { get; set; }

		[DataMember]
		public decimal CoordX { get; set; }

		[DataMember]
		public decimal CoordY { get; set; }

		[DataMember]
		public decimal CoordZ { get; set; }

		public int Quantity { get; set; }

		[DataMember]
		public decimal PackDimX { get; set; }
		
		[DataMember]
		public decimal PackDimY { get; set; }
		
		[DataMember]
		public decimal PackDimZ { get; set; }
		
		[DataMember]
		public decimal Volume => volume;

        #endregion Public Properties

        #region Constructors

        public Item(int id, decimal dim1, decimal dim2, decimal dim3, int quantity)
        {
            ID = id;
            Dim1 = dim1;
            Dim2 = dim2;
            Dim3 = dim3;
            volume = dim1 * dim2 * dim3;
            Quantity = quantity;
        }

        #endregion Constructors

    }
}
