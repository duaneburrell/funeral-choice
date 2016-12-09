using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Obsequy.Utility;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Obsequy.Model
{
	public class ResponseReceipt : ChangeReceipt
	{
		#region Data Properties

		[BsonRepresentation(BsonType.Int32)]
		public ResponseReceiptStates State { get; set; }

		[BsonRepresentation(BsonType.Double)]
		public double? Value { get; set; }

		#endregion

		#region Properties

		#region Is Accepted
		[BsonIgnore]
		public bool IsAccepted
		{
			get { return (this.State == ResponseReceiptStates.Accepted); }
		}
		#endregion

		#region Is Available
		[BsonIgnore]
		public bool IsAvailable
		{
			get { return (this.State == ResponseReceiptStates.Available); }
		}
		#endregion

		#region Is Dismissed
		[BsonIgnore]
		public bool IsDismissed
		{
			get { return (this.State == ResponseReceiptStates.Dismissed); }
		}
		#endregion

		#region Is Expired
		[BsonIgnore]
		public bool IsExpired
		{
			get { return (this.State == ResponseReceiptStates.Expired); }
		}
		#endregion

		#region Is Pending
		[BsonIgnore]
		public bool IsPending
		{
			get { return (this.State == ResponseReceiptStates.Pending); }
		}
		#endregion

		#region Is ReConfirm
		[BsonIgnore]
		public bool IsReConfirm
		{
			get { return (this.State == ResponseReceiptStates.ReConfirm); }
		}
		#endregion

		#region Is Rejected
		[BsonIgnore]
		public bool IsRejected
		{
			get { return (this.State == ResponseReceiptStates.Rejected); }
		}
		#endregion

		#region Mnemonic
		[BsonIgnore]
		public string Mnemonic
		{
			get { return this.State.ToString(); }
		}
		#endregion

		#endregion

		#region Constructors

		public ResponseReceipt()
			: base()
		{
		}

		public ResponseReceipt(AccountSession accountSession)
			: base(accountSession)
		{
			this.State = ResponseReceiptStates.Available;
			this.Value = null;
		}

		#endregion

		#region Methods
		
		#region To String
		public override string ToString()
		{
			return string.Format("{0}: {1}", State, Value);
		}
		#endregion

		#endregion
	}
}