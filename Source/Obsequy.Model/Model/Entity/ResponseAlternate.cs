using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoRepository;
using Obsequy.Utility;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Obsequy.Model
{
	public class ResponseAlternate
	{
		#region Data Properties

		public Obsequy.Model.Tuple<GenericTypes> WakeDate { get; set; }

		public Obsequy.Model.Tuple<GenericTypes> CeremonyDate { get; set; }

		public Obsequy.Model.Tuple<InternmentTypes> InternmentType { get; set; }

		public Obsequy.Model.Tuple<FuneralTypes> FuneralType { get; set; }

		public Obsequy.Model.Tuple<WakeTypes> WakeType { get; set; }

		public Obsequy.Model.Tuple<ReligionTypes> ReligionType { get; set; }

		public Obsequy.Model.Tuple<ExpectedAttendanceTypes> ExpectedAttendanceType { get; set; }

		public Obsequy.Model.Tuple<GenericTypes> ServicePreferences { get; set; }

		public Obsequy.Model.Tuple<CasketMaterialTypes> CasketMaterialType { get; set; }

		public Obsequy.Model.Tuple<CasketSizeTypes> CasketSizeType { get; set; }

		public Obsequy.Model.Tuple<CasketColorTypes> CasketColorType { get; set; }

		public Obsequy.Model.Tuple<CasketManufacturerTypes> CasketManufacturerType { get; set; }

		public Obsequy.Model.Tuple<TransportationOfFamilyTypes> TransportationOfFamilyType { get; set; }

		public Obsequy.Model.Tuple<BurialContainerTypes> BurialContainerType { get; set; }

		public Obsequy.Model.Tuple<FlowerSprayTypes> FlowerSprayType { get; set; }

		public Obsequy.Model.Tuple<FlowerTypes> PrimaryFlowerType { get; set; }

		public Obsequy.Model.Tuple<FlowerTypes> SecondaryFlowerType { get; set; }

		public Obsequy.Model.Tuple<FlowerTypes> AccentFlowerType { get; set; }

		public ChangeReceipt Receipt { get; set; }

		#endregion

		#region Properties

		#endregion

		#region Constructors

		public ResponseAlternate()
			: base()
		{
			this.WakeDate = new Obsequy.Model.Tuple<GenericTypes>();
			this.CeremonyDate = new Obsequy.Model.Tuple<GenericTypes>();
			this.InternmentType = new Obsequy.Model.Tuple<InternmentTypes>();
			this.FuneralType = new Obsequy.Model.Tuple<FuneralTypes>();
			this.WakeType = new Obsequy.Model.Tuple<WakeTypes>();
			this.ReligionType = new Obsequy.Model.Tuple<ReligionTypes>();
			this.ExpectedAttendanceType = new Obsequy.Model.Tuple<ExpectedAttendanceTypes>();
			this.ServicePreferences = new Obsequy.Model.Tuple<GenericTypes>();
			this.CasketMaterialType = new Obsequy.Model.Tuple<CasketMaterialTypes>();
			this.CasketSizeType = new Obsequy.Model.Tuple<CasketSizeTypes>();
			this.CasketColorType = new Obsequy.Model.Tuple<CasketColorTypes>();
			this.CasketManufacturerType = new Obsequy.Model.Tuple<CasketManufacturerTypes>();
			this.BurialContainerType = new Obsequy.Model.Tuple<BurialContainerTypes>();
			this.TransportationOfFamilyType = new Tuple<TransportationOfFamilyTypes>();
			this.FlowerSprayType = new Obsequy.Model.Tuple<FlowerSprayTypes>();
			this.PrimaryFlowerType = new Obsequy.Model.Tuple<FlowerTypes>();
			this.SecondaryFlowerType = new Obsequy.Model.Tuple<FlowerTypes>();
			this.AccentFlowerType = new Obsequy.Model.Tuple<FlowerTypes>();
			this.Receipt = new ChangeReceipt();
		}

		#endregion

		#region Methods

		#region As Generic Type
		public Obsequy.Model.Tuple<GenericTypes> AsGenericType(string propertyName)
		{
			var generic = new Obsequy.Model.Tuple<GenericTypes>()
			{
				Value = GenericTypes.TypeSpecified,
				Specified = string.Empty
			};

			switch (propertyName)
			{
				case "WakeDate": generic.Value = (GenericTypes)Convert.ToInt32(WakeDate.Value); generic.Specified = WakeDate.Specified; break;
				case "CeremonyDate": generic.Value = (GenericTypes)Convert.ToInt32(CeremonyDate.Value); generic.Specified = CeremonyDate.Specified; break;

				case "InternmentType": generic.Value = (GenericTypes)Convert.ToInt32(InternmentType.Value); generic.Specified = InternmentType.Specified; break;
				case "FuneralType": generic.Value = (GenericTypes)Convert.ToInt32(FuneralType.Value); generic.Specified = FuneralType.Specified; break;
				case "WakeType": generic.Value = (GenericTypes)Convert.ToInt32(WakeType.Value); generic.Specified = WakeType.Specified; break;
				case "ReligionType": generic.Value = (GenericTypes)Convert.ToInt32(ReligionType.Value); generic.Specified = ReligionType.Specified; break;
				case "ExpectedAttendanceType": generic.Value = (GenericTypes)Convert.ToInt32(ExpectedAttendanceType.Value); generic.Specified = ExpectedAttendanceType.Specified; break;
				case "ServicePreferences": generic.Value = (GenericTypes)Convert.ToInt32(ServicePreferences.Value); generic.Specified = ServicePreferences.Specified; break;

				case "CasketMaterialType": generic.Value = (GenericTypes)Convert.ToInt32(CasketMaterialType.Value); generic.Specified = CasketMaterialType.Specified; break;
				case "CasketSizeType": generic.Value = (GenericTypes)Convert.ToInt32(CasketSizeType.Value); generic.Specified = CasketSizeType.Specified; break;
				case "CasketColorType": generic.Value = (GenericTypes)Convert.ToInt32(CasketColorType.Value); generic.Specified = CasketColorType.Specified; break;
				case "CasketManufacturerType": generic.Value = (GenericTypes)Convert.ToInt32(CasketManufacturerType.Value); generic.Specified = CasketManufacturerType.Specified; break;
				case "BurialContainerType": generic.Value = (GenericTypes)Convert.ToInt32(BurialContainerType.Value); generic.Specified = BurialContainerType.Specified; break;

				case "TransportationOfFamilyType": generic.Value = (GenericTypes)Convert.ToInt32(TransportationOfFamilyType.Value); generic.Specified = TransportationOfFamilyType.Specified; break;

				case "FlowerSprayType": generic.Value = (GenericTypes)Convert.ToInt32(FlowerSprayType.Value); generic.Specified = FlowerSprayType.Specified; break;
				case "PrimaryFlowerType": generic.Value = (GenericTypes)Convert.ToInt32(PrimaryFlowerType.Value); generic.Specified = PrimaryFlowerType.Specified; break;
				case "SecondaryFlowerType": generic.Value = (GenericTypes)Convert.ToInt32(SecondaryFlowerType.Value); generic.Specified = SecondaryFlowerType.Specified; break;
				case "AccentFlowerType": generic.Value = (GenericTypes)Convert.ToInt32(AccentFlowerType.Value); generic.Specified = AccentFlowerType.Specified; break;

				default:
					throw new Exception(string.Format("Unhandled property name: {0} in ResponseAlternate.AsGenericType()", propertyName));
			}

			return generic;
		}
		#endregion

		#region Clear
		public void Clear()
		{
			this.WakeDate.Clear();
			this.CeremonyDate.Clear();

			this.InternmentType.Clear();
			this.FuneralType.Clear();
			this.WakeType.Clear();
			this.ReligionType.Clear();
			this.ExpectedAttendanceType.Clear();
			this.ServicePreferences.Clear();

			this.CasketMaterialType.Clear();
			this.CasketSizeType.Clear();
			this.CasketColorType.Clear();
			this.CasketManufacturerType.Clear();
			this.BurialContainerType.Clear();

			this.TransportationOfFamilyType.Clear();

			this.FlowerSprayType.Clear();
			this.PrimaryFlowerType.Clear();
			this.SecondaryFlowerType.Clear();
			this.AccentFlowerType.Clear();
		}
		#endregion

		#region Clone
		public ResponseAlternate Clone()
		{
			return new ResponseAlternate()
			{
				Receipt = this.Receipt.Clone(),
				WakeDate = this.WakeDate.Clone(),
				CeremonyDate = this.CeremonyDate.Clone(),
				InternmentType = this.InternmentType.Clone(),
				FuneralType = this.FuneralType.Clone(),
				WakeType = this.WakeType.Clone(),
				ReligionType = this.ReligionType.Clone(),
				ExpectedAttendanceType = this.ExpectedAttendanceType.Clone(),
				ServicePreferences = this.ServicePreferences.Clone(),
				CasketMaterialType = this.CasketMaterialType.Clone(),
				CasketSizeType = this.CasketSizeType.Clone(),
				CasketColorType = this.CasketColorType.Clone(),
				CasketManufacturerType = this.CasketManufacturerType.Clone(),
				TransportationOfFamilyType = this.TransportationOfFamilyType.Clone(),
				BurialContainerType = this.BurialContainerType.Clone(),
				FlowerSprayType = this.FlowerSprayType.Clone(),
				PrimaryFlowerType = this.PrimaryFlowerType.Clone(),
				SecondaryFlowerType = this.SecondaryFlowerType.Clone(),
				AccentFlowerType = this.AccentFlowerType.Clone()
			};
		}
		#endregion

		#region Has Changed
		public bool HasChanged(ResponseAlternate alternate)
		{
			if (alternate == null)
				return false;

			if (this.WakeDate.HasChanged(alternate.WakeDate) ||
				this.FuneralType.HasChanged(alternate.FuneralType) ||
				this.CeremonyDate.HasChanged(alternate.CeremonyDate) ||
				this.InternmentType.HasChanged(alternate.InternmentType) ||
				this.WakeType.HasChanged(alternate.WakeType) ||
				this.ReligionType.HasChanged(alternate.ReligionType) ||
				this.ExpectedAttendanceType.HasChanged(alternate.ExpectedAttendanceType) ||
				this.ServicePreferences.HasChanged(alternate.ServicePreferences) ||
				this.CasketMaterialType.HasChanged(alternate.CasketMaterialType) ||
				this.CasketSizeType.HasChanged(alternate.CasketSizeType) ||
				this.CasketColorType.HasChanged(alternate.CasketColorType) ||
				this.CasketManufacturerType.HasChanged(alternate.CasketManufacturerType) ||
				this.BurialContainerType.HasChanged(alternate.BurialContainerType) ||
				this.TransportationOfFamilyType.HasChanged(alternate.TransportationOfFamilyType) ||
				this.FlowerSprayType.HasChanged(alternate.FlowerSprayType) ||
				this.PrimaryFlowerType.HasChanged(alternate.PrimaryFlowerType) ||
				this.SecondaryFlowerType.HasChanged(alternate.SecondaryFlowerType) ||
				this.AccentFlowerType.HasChanged(alternate.AccentFlowerType))
			{
				return true;
			}

			return false;
		}
		#endregion

		#region Update
		public void Update(ResponseAlternate alternate)
		{
			// update all values
			this.WakeDate.Update(alternate.WakeDate);
			this.CeremonyDate.Update(alternate.CeremonyDate);
			this.InternmentType.Update(alternate.InternmentType);
			this.FuneralType.Update(alternate.FuneralType);
			this.WakeType.Update(alternate.WakeType);
			this.ReligionType.Update(alternate.ReligionType);
			this.ExpectedAttendanceType.Update(alternate.ExpectedAttendanceType);
			this.ServicePreferences.Update(alternate.ServicePreferences);
			this.CasketMaterialType.Update(alternate.CasketMaterialType);
			this.CasketSizeType.Update(alternate.CasketSizeType);
			this.CasketColorType.Update(alternate.CasketColorType);
			this.CasketManufacturerType.Update(alternate.CasketManufacturerType);
			this.BurialContainerType.Update(alternate.BurialContainerType);
			this.TransportationOfFamilyType.Update(alternate.TransportationOfFamilyType);
			this.FlowerSprayType.Update(alternate.FlowerSprayType);
			this.PrimaryFlowerType.Update(alternate.PrimaryFlowerType);
			this.SecondaryFlowerType.Update(alternate.SecondaryFlowerType);
			this.AccentFlowerType.Update(alternate.AccentFlowerType);
		}
		#endregion

		#endregion
	}
}