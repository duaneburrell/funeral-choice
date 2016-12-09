using FluentValidation.Results;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Obsequy.Utility;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Obsequy.Model
{
	public class ConsumerPreference
	{
		#region Data Properties

		[BsonRepresentation(BsonType.Double)]
		public double MaxDistance { get; set; }

		public Proximity Proximity { get; set; }

		public Obsequy.Model.Tuple<InternmentTypes> InternmentType { get; set; }

		public Obsequy.Model.Tuple<FuneralTypes> FuneralType { get; set; }

		public Obsequy.Model.Tuple<WakeTypes> WakeType { get; set; }

		public Obsequy.Model.Tuple<ReligionTypes> ReligionType { get; set; }

		public Obsequy.Model.Tuple<ExpectedAttendanceTypes> ExpectedAttendanceType { get; set; }

		[BsonRepresentation(BsonType.String)]
		public string ServicePreferences { get; set; }

		[BsonRepresentation(BsonType.Int32)]
		public RemainsChoiceTypes RemainsChoiceType { get; set; }

		public Obsequy.Model.Tuple<CasketMaterialTypes> CasketMaterialType { get; set; }

		public Obsequy.Model.Tuple<CasketSizeTypes> CasketSizeType { get; set; }

		public Obsequy.Model.Tuple<CasketColorTypes> CasketColorType { get; set; }

		public Obsequy.Model.Tuple<CasketManufacturerTypes> CasketManufacturerType { get; set; }

		public Obsequy.Model.Tuple<BurialContainerTypes> BurialContainerType { get; set; }

		[BsonRepresentation(BsonType.Int32)]
		public TransportationChoiceTypes TransportationChoiceType { get; set; }

		public Obsequy.Model.Tuple<TransportationOfFamilyTypes> TransportationOfFamilyType { get; set; }

		public Obsequy.Model.Tuple<FlowerSprayTypes> FlowerSprayType { get; set; }

		public Obsequy.Model.Tuple<FlowerTypes> PrimaryFlowerType { get; set; }

		public Obsequy.Model.Tuple<FlowerTypes> SecondaryFlowerType { get; set; }

		public Obsequy.Model.Tuple<FlowerTypes> AccentFlowerType { get; set; }

		#endregion

		#region Properties

		#region Any Casket Selections
		[BsonIgnore]
		public bool AnyCasketSelections
		{
			get
			{
				return (
					(this.CasketMaterialType.Value != CasketMaterialTypes.NA && this.CasketMaterialType.Value != CasketMaterialTypes.None) ||
					(this.CasketSizeType.Value != CasketSizeTypes.NA && this.CasketSizeType.Value != CasketSizeTypes.None) ||
					(this.CasketColorType.Value != CasketColorTypes.NA && this.CasketColorType.Value != CasketColorTypes.None) ||
					(this.CasketManufacturerType.Value != CasketManufacturerTypes.NA && this.CasketManufacturerType.Value != CasketManufacturerTypes.None) ||
					(this.BurialContainerType.Value != BurialContainerTypes.NA && this.BurialContainerType.Value != BurialContainerTypes.None));
			}
		}
		#endregion

		#region Any FLower Selections
		[BsonIgnore]
		public bool AnyFlowerSelections
		{
			get
			{
				return (
					(this.PrimaryFlowerType.Value != FlowerTypes.NA && this.PrimaryFlowerType.Value != FlowerTypes.None) ||
					(this.SecondaryFlowerType.Value != FlowerTypes.NA && this.SecondaryFlowerType.Value != FlowerTypes.None) ||
					(this.AccentFlowerType.Value != FlowerTypes.NA && this.AccentFlowerType.Value != FlowerTypes.None) ||
					(this.FlowerSprayType.Value != FlowerSprayTypes.NA && this.FlowerSprayType.Value != FlowerSprayTypes.None));
			}
		}
		#endregion

		#region Any Service Selections
		[BsonIgnore]
		public bool AnyServiceSelections
		{
			get
			{
				return (
					(this.InternmentType.Value != InternmentTypes.NA && this.InternmentType.Value != InternmentTypes.None) ||
					(this.FuneralType.Value != FuneralTypes.NA && this.FuneralType.Value != FuneralTypes.None) ||
					(this.WakeType.Value != WakeTypes.NA && this.WakeType.Value != WakeTypes.None) ||
					(this.ReligionType.Value != ReligionTypes.NA && this.ReligionType.Value != ReligionTypes.None) ||
					(!string.IsNullOrEmpty(this.ServicePreferences)));
			}
		}
		#endregion

		#region Any Transportation Selections
		[BsonIgnore]
		public bool AnyTransportationSelections
		{
			get
			{
				return (
					(this.TransportationOfFamilyType.Value != TransportationOfFamilyTypes.NA && this.TransportationOfFamilyType.Value != TransportationOfFamilyTypes.None));
			}
		}
		#endregion

		#region Chosen Options Count
		[BsonIgnore]
		public int ChosenOptionsCount
		{
			get
			{
				var count = 0;

				count += (this.InternmentType.Value != InternmentTypes.NA ? 1 : 0);
				count += (this.FuneralType.Value != FuneralTypes.NA ? 1 : 0);
				count += (this.WakeType.Value != WakeTypes.NA ? 1 : 0);
				count += (this.ReligionType.Value != ReligionTypes.NA ? 1 : 0);
				count += (this.ExpectedAttendanceType.Value != ExpectedAttendanceTypes.NA ? 1 : 0);

				count += (!string.IsNullOrEmpty(this.ServicePreferences) ? 1 : 0);

				if (this.RemainsChoiceType == RemainsChoiceTypes.Unnecessary)
				{
					count += 1;
				}
				if (this.RemainsChoiceType == RemainsChoiceTypes.CasketRequired)
				{
					count += (this.CasketMaterialType.Value != CasketMaterialTypes.NA ? 1 : 0);
					count += (this.CasketSizeType.Value != CasketSizeTypes.NA ? 1 : 0);
					count += (this.CasketColorType.Value != CasketColorTypes.NA ? 1 : 0);
					count += (this.CasketManufacturerType.Value != CasketManufacturerTypes.NA ? 1 : 0);
					count += (this.BurialContainerType.Value != BurialContainerTypes.NA ? 1 : 0);

                    count += (this.FlowerSprayType.Value != FlowerSprayTypes.NA ? 1 : 0);
                }
				if (this.RemainsChoiceType == RemainsChoiceTypes.UrnRequired)
				{
					count += 1;
				}


				if (this.TransportationChoiceType == TransportationChoiceTypes.Unnecessary)
				{
					count += 1;
				}
				if (this.TransportationChoiceType == TransportationChoiceTypes.Required)
				{
					count += (this.TransportationOfFamilyType.Value != TransportationOfFamilyTypes.NA ? 1 : 0);
				}
				
				count += (this.PrimaryFlowerType.Value != FlowerTypes.NA ? 1 : 0);
				count += (this.SecondaryFlowerType.Value != FlowerTypes.NA ? 1 : 0);
				count += (this.AccentFlowerType.Value != FlowerTypes.NA ? 1 : 0);

				return count;
			}
		}
		#endregion

		#region Is Completed
		[BsonIgnore]
		public bool IsCompleted
		{
			get { return (this.ChosenOptionsCount == this.TotalOptionsCount); }
		}
		#endregion

		#region Percent Complete
		[BsonIgnore]
		public double PercentComplete
		{
			get { return (((double)this.ChosenOptionsCount) / ((double)this.TotalOptionsCount)); }
		}
		#endregion

		#region Total Options Count
		[BsonIgnore]
		public int TotalOptionsCount
		{
			get 
			{
				var count = 0;

				// service
				count = 5 + (!string.IsNullOrEmpty(this.ServicePreferences) ? 1 : 0);

				// casket / urn
				if (this.RemainsChoiceType == RemainsChoiceTypes.Unnecessary)
					count += 1;
				else if (this.RemainsChoiceType == RemainsChoiceTypes.CasketRequired)
					count += 6;
				else if (this.RemainsChoiceType == RemainsChoiceTypes.UrnRequired)
					count += 1;

				// transportation
				if (this.TransportationChoiceType == TransportationChoiceTypes.Unnecessary)
					count += 1;
				else if (this.TransportationChoiceType == TransportationChoiceTypes.Required)
					count += 1;
				
				// flowers
				count += 3;

				return count; 
			}
		}
		#endregion

		#endregion

		#region Constructors

		public ConsumerPreference()
			: base()
		{
			this.Proximity = new Proximity();
			this.InternmentType = new Obsequy.Model.Tuple<InternmentTypes>();
			this.FuneralType = new Obsequy.Model.Tuple<FuneralTypes>();
			this.WakeType = new Obsequy.Model.Tuple<WakeTypes>();
			this.ExpectedAttendanceType = new Obsequy.Model.Tuple<ExpectedAttendanceTypes>();
			this.CasketMaterialType = new Obsequy.Model.Tuple<CasketMaterialTypes>();
			this.CasketSizeType = new Obsequy.Model.Tuple<CasketSizeTypes>();
			this.CasketColorType = new Obsequy.Model.Tuple<CasketColorTypes>();
			this.CasketManufacturerType = new Obsequy.Model.Tuple<CasketManufacturerTypes>();
			this.BurialContainerType = new Obsequy.Model.Tuple<BurialContainerTypes>();
			this.TransportationOfFamilyType = new Obsequy.Model.Tuple<TransportationOfFamilyTypes>();
			this.FlowerSprayType = new Obsequy.Model.Tuple<FlowerSprayTypes>();
			this.PrimaryFlowerType = new Obsequy.Model.Tuple<FlowerTypes>();
			this.SecondaryFlowerType = new Obsequy.Model.Tuple<FlowerTypes>();
			this.AccentFlowerType = new Obsequy.Model.Tuple<FlowerTypes>();
			this.ReligionType = new Obsequy.Model.Tuple<ReligionTypes>();
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
				case "InternmentType": generic.Value = (GenericTypes)Convert.ToInt32(InternmentType.Value); generic.Specified = InternmentType.Specified; break;
				case "FuneralType": generic.Value = (GenericTypes)Convert.ToInt32(FuneralType.Value); generic.Specified = FuneralType.Specified; break;
				case "WakeType": generic.Value = (GenericTypes)Convert.ToInt32(WakeType.Value); generic.Specified = WakeType.Specified; break;
				case "ReligionType": generic.Value = (GenericTypes)Convert.ToInt32(ReligionType.Value); generic.Specified = ReligionType.Specified; break;
				case "ExpectedAttendanceType": generic.Value = (GenericTypes)Convert.ToInt32(ExpectedAttendanceType.Value); generic.Specified = ExpectedAttendanceType.Specified; break;

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
					throw new Exception(string.Format("Unhandled property name: {0} in ConsumerPreference.AsGenericType()", propertyName));
			}

			return generic;
		}
		#endregion

		#region Clone
		public ConsumerPreference Clone()
		{
			return new ConsumerPreference()
			{
				MaxDistance = this.MaxDistance,
				Proximity = this.Proximity.Clone(),
				InternmentType = this.InternmentType.Clone(),
				FuneralType = this.FuneralType.Clone(),
				WakeType = this.WakeType.Clone(),
				ReligionType = this.ReligionType.Clone(),
				ExpectedAttendanceType = this.ExpectedAttendanceType.Clone(),
				ServicePreferences = this.ServicePreferences,
				RemainsChoiceType = this.RemainsChoiceType,
				CasketMaterialType = this.CasketMaterialType.Clone(),
				CasketSizeType = this.CasketSizeType.Clone(),
				CasketColorType = this.CasketColorType.Clone(),
				CasketManufacturerType = this.CasketManufacturerType.Clone(),
				BurialContainerType = this.BurialContainerType.Clone(),
				TransportationChoiceType = this.TransportationChoiceType,
				TransportationOfFamilyType = this.TransportationOfFamilyType.Clone(),
				FlowerSprayType = this.FlowerSprayType.Clone(),
				PrimaryFlowerType = this.PrimaryFlowerType.Clone(),
				SecondaryFlowerType = this.SecondaryFlowerType.Clone(),
				AccentFlowerType = this.AccentFlowerType.Clone()
			};
		}
		#endregion

		#region Has Changed
		public bool HasChanged(ConsumerPreference preference)
		{
			if (preference == null)
				return false;

			if (this.MaxDistance != preference.MaxDistance)
				return true;

			if (this.Proximity.HasChanged(preference.Proximity))
				return true;

			if (this.InternmentType.HasChanged(preference.InternmentType) ||
				this.FuneralType.HasChanged(preference.FuneralType) ||
				this.WakeType.HasChanged(preference.WakeType) ||
				this.ReligionType.HasChanged(preference.ReligionType) ||
				this.ExpectedAttendanceType.HasChanged(preference.ExpectedAttendanceType) ||
				this.ServicePreferences != preference.ServicePreferences ||
				this.RemainsChoiceType != preference.RemainsChoiceType ||
				this.CasketMaterialType.HasChanged(preference.CasketMaterialType) ||
				this.CasketSizeType.HasChanged(preference.CasketSizeType) ||
				this.CasketColorType.HasChanged(preference.CasketColorType) ||
				this.CasketManufacturerType.HasChanged(preference.CasketManufacturerType) ||
				this.BurialContainerType.HasChanged(preference.BurialContainerType) ||
				this.TransportationChoiceType != preference.TransportationChoiceType ||
				this.TransportationOfFamilyType.HasChanged(preference.TransportationOfFamilyType) ||
				this.FlowerSprayType.HasChanged(preference.FlowerSprayType) ||
				this.PrimaryFlowerType .HasChanged(preference.PrimaryFlowerType) ||
				this.SecondaryFlowerType.HasChanged(preference.SecondaryFlowerType) ||
				this.AccentFlowerType.HasChanged(preference.AccentFlowerType))
			{
				return true;
			}

			return false;
		}
		#endregion

		#region Update
		public void Update(ConsumerPreference preference)
		{
			// update all values
			this.MaxDistance = MathHelper.RoundMaxDistance(preference.MaxDistance);
			this.InternmentType.Update(preference.InternmentType);
			this.FuneralType.Update(preference.FuneralType);
			this.WakeType.Update(preference.WakeType);
			this.ExpectedAttendanceType.Update(preference.ExpectedAttendanceType);
			this.ServicePreferences = preference.ServicePreferences;
			this.RemainsChoiceType = preference.RemainsChoiceType;
			this.CasketMaterialType.Update(preference.CasketMaterialType);
			this.CasketSizeType.Update(preference.CasketSizeType);
			this.CasketColorType.Update(preference.CasketColorType);
			this.CasketManufacturerType.Update(preference.CasketManufacturerType);
			this.BurialContainerType.Update(preference.BurialContainerType);
			this.TransportationChoiceType = preference.TransportationChoiceType;
			this.TransportationOfFamilyType = preference.TransportationOfFamilyType;
			this.FlowerSprayType.Update(preference.FlowerSprayType);
			this.PrimaryFlowerType.Update(preference.PrimaryFlowerType);
			this.SecondaryFlowerType.Update(preference.SecondaryFlowerType);
			this.AccentFlowerType.Update(preference.AccentFlowerType);
			this.ReligionType.Update(preference.ReligionType);

			this.Proximity.Update(preference.Proximity);
		}
		#endregion

		#endregion

		#region Validation
		public ValidationResult Validate(AccountSession accountSession, ValidationMode validationMode)
		{
			return ((new ConsumerPreferenceValidator(accountSession, validationMode, this)).Validate(this) as ValidationResult);
		}
		#endregion
	}
}
