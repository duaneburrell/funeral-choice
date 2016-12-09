using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Obsequy.Utility;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Obsequy.Model
{
	public class RequestReceipt : ChangeReceipt
	{
		#region Data Properties

		[BsonRepresentation(BsonType.Int32)]
		public RequestReceiptStates State { get; set; }

		#endregion

		#region Properties

		#region Can Generate Response
		[BsonIgnore]
		public bool CanGenerateResponse
		{
			get { return (this.IsPending); }
		}
		#endregion

		#region Is Pending
		[BsonIgnore]
		public bool IsPending
		{
			get { return (this.State == RequestReceiptStates.Pending); }
		}
		#endregion

		#region Is Completed
		[BsonIgnore]
		public bool IsCompleted
		{
			get { return (this.State == RequestReceiptStates.Completed); }
		}
		#endregion

		#region Is Draft
		[BsonIgnore]
		public bool IsDraft
		{
			get { return (this.State == RequestReceiptStates.Draft); }
		}
		#endregion

		#region Is Expired
		[BsonIgnore]
		public bool IsExpired
		{
			get { return (this.State == RequestReceiptStates.Expired); }
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

		public RequestReceipt()
			: base()
		{
		}

		public RequestReceipt(AccountSession accountSession)
			: base(accountSession)
		{
			this.State = RequestReceiptStates.Draft;
		}

		#endregion

		#region Methods

		#region To String
		public override string ToString()
		{
			return string.Format("{0}", State);
		}
		#endregion
		
		#endregion
	}
}