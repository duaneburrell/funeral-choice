using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoRepository;
using Obsequy.Utility;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Obsequy.Model
{
	public class Tuple<T>
	{
		#region Data Properties

		[BsonRepresentation(BsonType.Int32)]
		public T Value { get; set; }

		public object Specified { get; set; }

		#endregion

		#region Constructors

		public Tuple()
		{
		}

		public Tuple(T value, object specified)
		{
			this.Value = value;
			this.Specified = specified;
		}

		#endregion

		#region Methods

		#region Clear
		public void Clear()
		{
			Value = default(T);
			Specified = null;
		}
		#endregion

		#region Clone
		public Tuple<T> Clone()
		{
			return new Tuple<T>()
			{
				Value = this.Value,
				Specified = this.Specified
			};
		}
		#endregion

		#region Has Changed
		public bool HasChanged(Tuple<T> tuple)
		{
			if (tuple == null)
				return false;

			if (!Comparer<T>.Equals(Value, tuple.Value) ||
				this.Specified != tuple.Specified)
			{
				return true;
			}

			return false;
		}
		#endregion

		#region To String
		public override string ToString()
		{
			return string.Format("{0} {1}", Value, (Specified != null) ? ": " + Specified : string.Empty);
		}
		#endregion

		#region Update
		public void Update(Tuple<T> tuple)
		{
			// update all values
			this.Value = tuple.Value;
			this.Specified = tuple.Specified;
		}
		#endregion

		#endregion
	}
}