using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoRepository;
using Obsequy.Utility;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Obsequy.Model
{
	public class ResponseAgreement
	{
		#region Data Properties

		[BsonRepresentation(BsonType.Int32)]
		public AgreementTypes WakeDate { get; set; }

		[BsonRepresentation(BsonType.Int32)]
		public AgreementTypes CeremonyDate { get; set; }

		[BsonRepresentation(BsonType.Int32)]
		public AgreementTypes InternmentType { get; set; }

		[BsonRepresentation(BsonType.Int32)]
		public AgreementTypes FuneralType { get; set; }

		[BsonRepresentation(BsonType.Int32)]
		public AgreementTypes WakeType { get; set; }

		[BsonRepresentation(BsonType.Int32)]
		public AgreementTypes ReligionType { get; set; }

		[BsonRepresentation(BsonType.Int32)]
		public AgreementTypes ExpectedAttendanceType { get; set; }

		[BsonRepresentation(BsonType.Int32)]
		public AgreementTypes ServicePreferences { get; set; }

		[BsonRepresentation(BsonType.Int32)]
		public AgreementTypes CasketMaterialType { get; set; }

		[BsonRepresentation(BsonType.Int32)]
		public AgreementTypes CasketSizeType { get; set; }

		[BsonRepresentation(BsonType.Int32)]
		public AgreementTypes CasketColorType { get; set; }

		[BsonRepresentation(BsonType.Int32)]
		public AgreementTypes CasketManufacturerType { get; set; }

		[BsonRepresentation(BsonType.Int32)]
		public AgreementTypes TransportationOfFamilyType { get; set; }

		[BsonRepresentation(BsonType.Int32)]
		public AgreementTypes BurialContainerType { get; set; }

		[BsonRepresentation(BsonType.Int32)]
		public AgreementTypes FlowerSprayType { get; set; }

		[BsonRepresentation(BsonType.Int32)]
		public AgreementTypes PrimaryFlowerType { get; set; }

		[BsonRepresentation(BsonType.Int32)]
		public AgreementTypes SecondaryFlowerType { get; set; }

		[BsonRepresentation(BsonType.Int32)]
		public AgreementTypes AccentFlowerType { get; set; }

		public ChangeReceipt Receipt { get; set; }

		#endregion

		#region Properties

		#endregion

		#region Constructors

		public ResponseAgreement()
			: base()
		{
		}

		#endregion

		#region Methods

		#region As Generic Type
		public AgreementTypes AsAgreementType(string propertyName)
		{
			var agreementType = AgreementTypes.NA;

			try
			{
				agreementType = (AgreementTypes)this.GetType().GetProperty(propertyName).GetValue(this, null);
			}
			catch (Exception)
			{
				throw new Exception(string.Format("Unhandled property name: {0} in ResponseAgreement.AsAgreementType()", propertyName));
			}

			return agreementType;
		}
		#endregion

		#region Clear
		public void Clear()
		{
			this.WakeDate = AgreementTypes.NA;
			this.CeremonyDate = AgreementTypes.NA;

			this.InternmentType = AgreementTypes.NA;
			this.FuneralType = AgreementTypes.NA;
			this.WakeType = AgreementTypes.NA;
			this.ReligionType = AgreementTypes.NA;
			this.ExpectedAttendanceType = AgreementTypes.NA;
			this.ServicePreferences = AgreementTypes.NA;

			this.CasketMaterialType = AgreementTypes.NA;
			this.CasketSizeType = AgreementTypes.NA;
			this.CasketColorType = AgreementTypes.NA;
			this.CasketManufacturerType = AgreementTypes.NA;
			this.BurialContainerType = AgreementTypes.NA;

			this.TransportationOfFamilyType = AgreementTypes.NA;

			this.FlowerSprayType = AgreementTypes.NA;
			this.PrimaryFlowerType = AgreementTypes.NA;
			this.SecondaryFlowerType = AgreementTypes.NA;
			this.AccentFlowerType = AgreementTypes.NA;
		}
		#endregion

		#region Has Changed
		public bool HasChanged(ResponseAgreement agreement)
		{
			if (this.WakeDate != agreement.WakeDate ||
				this.CeremonyDate != agreement.CeremonyDate ||
				this.InternmentType != agreement.InternmentType ||
				this.FuneralType != agreement.FuneralType ||
				this.WakeType != agreement.WakeType ||
				this.ReligionType != agreement.ReligionType ||
				this.ExpectedAttendanceType != agreement.ExpectedAttendanceType ||
				this.ServicePreferences != agreement.ServicePreferences ||
				this.CasketMaterialType != agreement.CasketMaterialType ||
				this.CasketSizeType != agreement.CasketSizeType ||
				this.CasketColorType != agreement.CasketColorType ||
				this.CasketManufacturerType != agreement.CasketManufacturerType ||
				this.BurialContainerType != agreement.BurialContainerType ||
				this.TransportationOfFamilyType != agreement.TransportationOfFamilyType ||
				this.FlowerSprayType != agreement.FlowerSprayType ||
				this.PrimaryFlowerType != agreement.PrimaryFlowerType ||
				this.SecondaryFlowerType != agreement.SecondaryFlowerType ||
				this.AccentFlowerType != agreement.AccentFlowerType)
				return true;

			return false;
		}
		#endregion

		#region Update
		public void Update(ResponseAgreement agreement)
		{
			this.WakeDate = agreement.WakeDate;
			this.CeremonyDate = agreement.CeremonyDate;
			this.InternmentType = agreement.InternmentType;
			this.FuneralType = agreement.FuneralType;
			this.WakeType = agreement.WakeType;
			this.ReligionType = agreement.ReligionType;
			this.ExpectedAttendanceType = agreement.ExpectedAttendanceType;
			this.ServicePreferences = agreement.ServicePreferences;
			this.CasketMaterialType = agreement.CasketMaterialType;
			this.CasketSizeType = agreement.CasketSizeType;
			this.CasketColorType = agreement.CasketColorType;
			this.CasketManufacturerType = agreement.CasketManufacturerType;
			this.BurialContainerType = agreement.BurialContainerType;
			this.TransportationOfFamilyType = agreement.TransportationOfFamilyType;
			this.FlowerSprayType = agreement.FlowerSprayType;
			this.PrimaryFlowerType = agreement.PrimaryFlowerType;
			this.SecondaryFlowerType = agreement.SecondaryFlowerType;
			this.AccentFlowerType = agreement.AccentFlowerType;
		}
		#endregion
		
		#endregion
	}
}