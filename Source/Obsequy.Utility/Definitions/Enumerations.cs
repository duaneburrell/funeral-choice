using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Obsequy.Utility
{
	public enum AccountPrestige
	{
		None = 0,
		Creator = 1,
		Contributor = 2
	};

	public enum AccountPrivilidge
    {
        None = 0,
        Elevated = 1,
        Standard = 2
    };

	public enum AccountStatus
	{
		None = 0,
		Pending = 1,
		Active = 2,
		Disabled = 3,
		Suspended = 4,
		Terminated = 5,
		Rejected = 6
	};

	public enum AccountType
	{
		None = 0,
		Administrator = 1,
		Consumer = 2,
		Provider = 3
	};

	public enum AgreementTypes
	{
		NA = 0,
		Agreed = 1,
		NotAgreed = 2,
		Alternate = 3
	};

	public enum ApplicationEnvironments
	{
		None = 0,
		Demonstration = 1,
		Development = 2,
		Production = 3
	};

	public enum BusinessEstablishedTypes
	{
		NA = 0,
		LessThanOneYear = 1,
		OneToFiveYears = 2,
		FiveToTenYears = 3,
		MoreThanTenYears = 4
	};

	public enum BurialContainerTypes
	{
		NA = 0,
		None = 1,
		Liner = 2,
		Cement = 3,
		StainlessSteel = 4,
		NotSure = 5,
		UserSpecified = Definitions.USER_SPECIFIED_VALUE
	};

	public enum CasketColorTypes
	{
		NA = 0,
		None = 1,
		Natural = 2,
		Silver = 3,
		Brown = 4,
		Black = 5,
		UserSpecified = Definitions.USER_SPECIFIED_VALUE
	};

	public enum CasketMaterialTypes
	{
		NA = 0,
		None = 1,
		Steel16Gage = 2,
		Steel18Gage = 3,
		Steel20Gage = 4,
		Steel22Gage = 5,
		StainlessSteel16Gage = 6,
		StainlessSteel18Gage = 7,
		StainlessSteel20Gage = 8,
		StainlessSteel22Gage = 9,
        Copper32Oz = 10,
        Copper34Oz = 11,
        Bronze32Oz = 12,
        Bronze34Oz = 13,
        Aspen = 14,
        Mahogany = 15,
        Maple = 16,
        Oak = 17,
        Pine = 18,
        Poplar = 19,
        Rosewood = 20,
        Walnut = 21,
		UserSpecified = Definitions.USER_SPECIFIED_VALUE
	};

	public enum CasketManufacturerTypes
	{
		NA = 0,
		None = 1,
		Astrol = 2,
		Aurora = 3,
		Batesville = 4,
		Matthews = 5,
		SouthernHeritage = 6,
		UserSpecified = Definitions.USER_SPECIFIED_VALUE
	};

	public enum CasketSizeTypes
	{
		NA = 0,
		None = 1,
		Small = 2,
		Standard = 3,
		Oversized = 4,
		UserSpecified = Definitions.USER_SPECIFIED_VALUE
	};

	public enum ExpectedAttendanceTypes
	{
		NA = 0,
		None = 1,
		UpTo25 = 2,
		UpTo50 = 3,
		UpTo75 = 4,
		UpTo100 = 5,
		UpTo150 = 6,
		UpTo200 = 7,
		UpTo250 = 8,
		MoreThan250 = 9
	};

	public enum FacilityAgeTypes
	{
		NA = 0,
		LessThanFiveYears = 1,
		FiveToTenYears = 2,
		MoreThanTenYears = 3
	};

	public enum FacilityStyleTypes
	{
		NA = 0,
		Simple = 1,
		Traditional = 2,
		Modern = 3,
		Lavish = 4
	};

	public enum FlowerSprayTypes
	{
		NA = 0,
		None = 1,
		Simple = 2,
		Standard = 3,
		Elegant = 4,
		UserSpecified = Definitions.USER_SPECIFIED_VALUE
	};

	public enum FlowerTypes
	{
		NA = 0,
		None = 1,
		Tulips = 2,
		Rose = 3,
		Carnations = 4,
		UserSpecified = Definitions.USER_SPECIFIED_VALUE
	};

	public enum FuneralDirectorExperienceTypes
	{
		NA = 0,
		LessThanOneYear = 1,
		OneToFiveYears = 2,
		FiveToTenYears = 3,
		MoreThanTenYears = 4
	};

	public enum FuneralTypes
	{
		NA = 0,
		None = 1,
		Gravesite = 2,
		Chapel = 3,
		Church = 4,
		Military = 5,
		UserSpecified = Definitions.USER_SPECIFIED_VALUE
	};

	public enum GenericTypes
	{
		NA = 0,
		None = 1,
		TypeSpecified = 3,
		UserSpecified = Definitions.USER_SPECIFIED_VALUE
	};

	public enum InternmentTypes
	{
		NA = 0,
		None = 1,
		Burial = 2,
		Cremation = 3,
		GiftToScience = 4,
		RemainsToFamily = 5,
		Scatter = 6,
		UserSpecified = Definitions.USER_SPECIFIED_VALUE
	};

	public enum NotificationEventTypes
	{
		None = 0
	};

	public enum ReligionTypes
	{
		NA = 0,
		None = 1,
		Christian_Baptist = 2,
		Christian_Catholic = 3,
		Christian_Protestant = 4,
		Jewish = 5,
		Muslim = 6,
		Hindu = 7,
		UserSpecified = Definitions.USER_SPECIFIED_VALUE
	};

	public enum RemainsChoiceTypes
	{
		None = 0,
		CasketRequired = 1,
		UrnRequired = 2,
		Unnecessary = 3
	};

	public enum RequestReceiptStates
    {
        None = 0,
		Draft = 1,
		Pending = 2,
		Expired = 3,
		Completed = 4
    };

	public enum ResponseReceiptStates
	{
		None = 0,
		Available = 1,
		InLieu = 2,
		Reconciled = 3,
		Pending = 4,
		Recalled = 5,
		ReConfirm = 6,
		Accepted = 7,
		Rejected = 8,
		Dismissed = 9,
		Expired = 10
	};

	public enum ScheduleChoiceTypes
	{
		None = 0,
		Scheduled = 1,
		PrePlan = 2
	};

	public enum TransportationChoiceTypes
	{
		None = 0,
		Required = 1,
		Unnecessary = 2
	};

	public enum TransportationFleetAgeTypes
	{
		NA = 0,
		LessThanOneYear = 1,
		OneToThreeYears = 2,
		FourToSixYears = 3,
		MoreThanSixYears = 4
	};

	public enum TransportationOfFamilyTypes
	{
		NA = 0,
		None = 1,
		UpTo4 = 2,
		UpTo8 = 3,
		UpTo12 = 4,
		UpTo16 = 5,
		UpTo20 = 6,
		MoreThan20 = 7
	};

	public enum WakeTypes
	{
		NA = 0,
		None = 1,
		FamilyOnly = 2,
		Public = 3,
		UserSpecified = Definitions.USER_SPECIFIED_VALUE
	};

}