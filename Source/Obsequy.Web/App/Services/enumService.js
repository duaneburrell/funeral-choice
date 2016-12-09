
angular.module('enum-service', [])
 .service('$enum', ['$window', function ($window) {

 	if (!!$window.enumService) {
 		return $window.enumService;
 	}

 	var USER_SPECIFIED_VALUE = 1000;

 	$window.enumService = {
 		USER_SPECIFIED_VALUE: USER_SPECIFIED_VALUE,

 		accountPrestige: {
 			None: { value: 0, text: 'None' },
 			Creator: { value: 1, text: 'Creator' },
 			Contributor: { value: 2, text: 'Contributor' }
 		},

 		accountPrivilidge: {
 			None: { value: 0, text: 'None' },
 			Elevated: { value: 1, text: 'Elevated' },
 			Standard: { value: 2, text: 'Standard' }
 		},

 		accountStatus: {
 			None: { value: 0, text: 'None' },
 			Pending: { value: 1, text: 'Pending' },
 			Active: { value: 2, text: 'Active' },
 			Disabled: { value: 3, text: 'Disabled' },
 			Suspended: { value: 4, text: 'Suspended' },
 			Terminated: { value: 5, text: 'Terminated' },
 			Rejected: { value: 6, text: 'Rejected' }
 		},

 		accountType: {
 			None:			{ value: 0, text: 'None' },
 			Administrator:	{ value: 1, text: 'Administrator' },
 			Consumer:		{ value: 2, text: 'Consumer' },
 			Provider:		{ value: 3, text: 'Provider' }
 		},
 		
 		agreementTypes: {
 			NA: { value: 0, text: 'N/A' },
 			Agreed: { value: 1, text: 'Agreed' },
 			NotAgreed: { value: 2, text: 'Not Agreed' },
 			Alternate: { value: 3, text: 'Alternate' }
 		},

 		burialContainerTypes: [
   			{ value: 0, text: 'N/A' },
   			{ value: 1, text: 'None' },
   			{ value: 2, text: 'Liner' },
   			{ value: 3, text: 'Cement' },
   			{ value: 4, text: 'Stainless Steel' },
   			{ value: 5, text: 'Not Sure' },
			{ value: USER_SPECIFIED_VALUE, text: 'User Specified' }
 		],

 		businessEstablishedTypes: [
   			{ value: 0, text: 'N/A' },
   			{ value: 1, text: 'Less than 1 Year' },
   			{ value: 2, text: '1 to 5 Years' },
   			{ value: 3, text: '5 to 10 Years' },
   			{ value: 4, text: 'More than 10 Years' },
 		],

 		casketColorTypes: [
			{ value: 0, text: 'Choose ..' },
			{ value: 1, text: 'None' },
			{ value: 2, text: 'Natural' },
			{ value: 3, text: 'Silver' },
			{ value: 4, text: 'Brown' },
			{ value: 5, text: 'Black' },
			{ value: USER_SPECIFIED_VALUE, text: 'User Specified' }
 		],

 		casketManufacturerTypes: [
   			{ value: 0, text: 'Choose ..' },
   			{ value: 1, text: 'None' },
   			{ value: 2, text: 'Astrol' },
   			{ value: 3, text: 'Aurora' },
   			{ value: 4, text: 'Batesville' },
   			{ value: 5, text: 'Matthews' },
   			{ value: 6, text: 'Southern Heritage' },
			{ value: USER_SPECIFIED_VALUE, text: 'User Specified' }
 		],

 		casketMaterialTypes: [
   			{ value: 0, text: 'Choose ..' },
   			{ value: 1, text: 'None' },
   			{ value: 2, text: '#16 Gage Steel' },
   			{ value: 3, text: '#18 Gage Steel' },
   			{ value: 4, text: '#20 Gage Steel' },
   			{ value: 5, text: '#22 Gage Steel' },
   			{ value: 6, text: '#16 Gage Stainless Steel' },
   			{ value: 7, text: '#18 Gage Stainless Steel' },
   			{ value: 8, text: '#20 Gage Stainless Steel' },
   			{ value: 9, text: '#22 Gage Stainless Steel' },
   			{ value: 10, text: '32oz Copper' },
   			{ value: 11, text: '34oz Copper' },
   			{ value: 12, text: '32oz Bronze' },
   			{ value: 13, text: '34oz Bronze' },
   			{ value: 14, text: 'Aspen' },
   			{ value: 15, text: 'Mahogany' },
   			{ value: 16, text: 'Maple' },
   			{ value: 17, text: 'Oak' },
   			{ value: 18, text: 'Pine' },
   			{ value: 19, text: 'Poplar' },
   			{ value: 20, text: 'Rosewood' },
   			{ value: 21, text: 'Walnut' },
			{ value: USER_SPECIFIED_VALUE, text: 'User Specified' }
 		],

 		casketSizeTypes: [
   			{ value: 0, text: 'Choose ..' },
   			{ value: 1, text: 'None' },
   			{ value: 2, text: 'Small' },
   			{ value: 3, text: 'Standard' },
   			{ value: 4, text: 'Oversized' },
			{ value: USER_SPECIFIED_VALUE, text: 'User Specified' }
 		],

 		expectedAttendanceTypes: [
   			{ value: 0, text: 'Choose ..' },
   			{ value: 2, text: 'Less Than 25 People' },
   			{ value: 3, text: '25 to 50 People' },
   			{ value: 4, text: '50 to 75 People' },
   			{ value: 5, text: '75 to 100 People' },
   			{ value: 6, text: '100 to 150 People' },
   			{ value: 7, text: '150 to 200 People' },
   			{ value: 8, text: '200 to 250 People' },
   			{ value: 9, text: 'More Than 250 People' }
 		],

 		expirationMonths: [
   			{ value: '01', text: '01' },
   			{ value: '02', text: '02' },
   			{ value: '03', text: '03' },
   			{ value: '04', text: '04' },
   			{ value: '05', text: '05' },
   			{ value: '06', text: '06' },
   			{ value: '07', text: '07' },
   			{ value: '08', text: '08' },
   			{ value: '09', text: '09' },
   			{ value: '10', text: '10' },
   			{ value: '11', text: '11' },
   			{ value: '12', text: '12' }
 		],

 		expirationYears: [
   			{ value: '2013', text: '2013' },
   			{ value: '2014', text: '2014' },
   			{ value: '2015', text: '2015' },
   			{ value: '2016', text: '2016' },
   			{ value: '2017', text: '2017' },
   			{ value: '2018', text: '2018' },
   			{ value: '2019', text: '2019' },
   			{ value: '2020', text: '2020' }
 		],

 		facilityAgeTypes: [
   			{ value: 0, text: 'N/A' },
   			{ value: 1, text: 'Less than 5 Years' },
   			{ value: 2, text: '5 to 10 Years' },
   			{ value: 3, text: 'More than 10 Years' },
 		],

 		facilityStyleTypes: [
   			{ value: 0, text: 'N/A' },
   			{ value: 1, text: 'Simple' },
   			{ value: 2, text: 'Traditional' },
   			{ value: 3, text: 'Modern' },
   			{ value: 4, text: 'Lavish' },
 		],

 		flowerSprayTypes: [
   			{ value: 0, text: 'N/A' },
   			{ value: 1, text: 'None' },
   			{ value: 2, text: 'Simple' },
   			{ value: 3, text: 'Standard' },
   			{ value: 4, text: 'Elegant' },
			{ value: USER_SPECIFIED_VALUE, text: 'User Specified' }
 		],

 		flowerTypes: [
   			{ value: 0, text: 'N/A' },
   			{ value: 1, text: 'None' },
   			{ value: 2, text: 'Tulips' },
   			{ value: 3, text: 'Rose' },
   			{ value: 4, text: 'Carnations' },
			{ value: USER_SPECIFIED_VALUE, text: 'User Specified' }
 		],

 		funeralDirectorExperienceTypes: [
   			{ value: 0, text: 'N/A' },
   			{ value: 1, text: 'Less than 1 Year' },
   			{ value: 2, text: '1 to 5 Years' },
   			{ value: 3, text: '5 to 10 Years' },
   			{ value: 4, text: 'More than 10 Years' },
 		],

 		funeralTypes: [
   			{ value: 0, text: 'N/A' },
   			{ value: 1, text: 'None' },
   			{ value: 2, text: 'Gravesite' },
   			{ value: 3, text: 'Chapel' },
   			{ value: 4, text: 'Church' },
   			{ value: 5, text: 'Military' },
   			{ value: 6, text: 'Mosque' },
   			{ value: 7, text: 'Synagogue' },
			{ value: USER_SPECIFIED_VALUE, text: 'User Specified' }
 		],

 		internmentTypes: [
   			{ value: 0, text: 'N/A' },
   			{ value: 1, text: 'None' },
   			{ value: 2, text: 'Burial' },
   			{ value: 3, text: 'Cremation' },
   			{ value: 4, text: 'Gift To Science' },
   			{ value: 5, text: 'Remains To Family' },
   			{ value: 6, text: 'Scatter' },
			{ value: USER_SPECIFIED_VALUE, text: 'User Specified' }
 		],

 		maximumDistances: [
   			{ value: 5, text: '5 Miles' },
   			{ value: 10, text: '10 Miles' },
   			{ value: 15, text: '15 Miles' },
   			{ value: 20, text: '20 Miles' },
   			{ value: 25, text: '25 Miles' },
   			{ value: 50, text: '50 Miles' },
   			{ value: 75, text: '75 Miles' },
   			{ value: 100, text: '100 Miles' }
 		],

 		religionTypes: [
            { value: 0, text: 'N/A' },
   			{ value: 1, text: 'None' },
            { value: 2, text: 'Christian (Baptist)' },
            { value: 3, text: 'Christian (Catholic)' },
            { value: 4, text: 'Christian (Protestant)' },
            { value: 5, text: 'Jewish' },
            { value: 6, text: 'Muslim' },
            { value: 7, text: 'Hindu' },
			{ value: USER_SPECIFIED_VALUE, text: 'User Specified' }
 		],

 		remainsChoiceTypes: {
 			None: { value: 0, text: 'None' },
 			CasketRequired: { value: 1, text: 'Casket Required' },
 			UrnRequired: { value: 2, text: 'Urn Required' },
 			Unnecessary: { value: 3, text: 'Unnecessary' }
 		},

 		requestReceiptStates: {
 			None:		{ value: 0, text: 'None' },
 			Draft:		{ value: 1, text: 'Draft' },
 			Pending:	{ value: 2, text: 'Pending' },
 			Expired:	{ value: 3, text: 'Expired' },
 			Completed:	{ value: 4, text: 'Completed' } 
 		},

 		responseReceiptStates: {
 			None:		{ value: 0, text: 'None' },
 			Available:	{ value: 1, text: 'Available' },
 			InLieu:		{ value: 2, text: 'In Lieu' },
 			Reconciled: { value: 3, text: 'Reconciled' },
 			Pending:	{ value: 4, text: 'Pending' },
 			Recalled:	{ value: 5, text: 'Recalled' },
 			ReConfirm:	{ value: 6, text: 'ReConfirm' },
 			Accepted:	{ value: 7, text: 'Accepted' },
 			Rejected:	{ value: 8, text: 'Rejected' },
 			Dismissed:	{ value: 9, text: 'Dismissed' },
 			Expired:	{ value: 10, text: 'Expired' }
 		},

 		scheduleChoiceTypes: {
 			None: { value: 0, text: 'None' },
 			Scheduled: { value: 1, text: 'Scheduled' },
 			PrePlan: { value: 2, text: 'PrePlan' }
 		},

 		transportationChoiceTypes: {
 			None: { value: 0, text: 'None' },
 			Required: { value: 1, text: 'Required' },
 			Unnecessary: { value: 2, text: 'Unnecessary' }
 		},

        transportationFleetAgeTypes: [
   			{ value: 0, text: 'N/A' },
   			{ value: 1, text: 'Less than 1 Year' },
   			{ value: 2, text: '1 to 3 Years' },
   			{ value: 3, text: '4 to 6 Years' },
   			{ value: 4, text: 'More than 6 Years' },
        ],

 		transportationOfFamilyTypes: [
			{ value: 0, text: 'Choose ..' },
   			{ value: 1, text: 'None' },
   			{ value: 2, text: '1 to 4 People' },
   			{ value: 3, text: '5 to 8 People' },
   			{ value: 4, text: '9 to 12 People' },
   			{ value: 5, text: '13 to 16 People' },
   			{ value: 6, text: '17 to 20 People' },
   			{ value: 7, text: 'More Than 20 People' }
 		],

 		states: [
   			{ value: 1, text: 'AL' },
   			{ value: 2, text: 'AK' },
   			{ value: 3, text: 'AZ' },
   			{ value: 4, text: 'AR' },
   			{ value: 5, text: 'CA' },
   			{ value: 6, text: 'CO' },
   			{ value: 7, text: 'CT' },
   			{ value: 8, text: 'DE' },
   			{ value: 9, text: 'DC' },
   			{ value: 10, text: 'FL' },
   			{ value: 11, text: 'GA' },
   			{ value: 12, text: 'HI' },
   			{ value: 13, text: 'ID' },
   			{ value: 14, text: 'IL' },
   			{ value: 15, text: 'IN' },
   			{ value: 16, text: 'IA' },
   			{ value: 17, text: 'KS' },
   			{ value: 18, text: 'KY' },
   			{ value: 19, text: 'LA' },
   			{ value: 20, text: 'ME' },
   			{ value: 21, text: 'MD' },
   			{ value: 22, text: 'MA' },
   			{ value: 23, text: 'MI' },
   			{ value: 24, text: 'MN' },
   			{ value: 25, text: 'MS' },
   			{ value: 26, text: 'MO' },
   			{ value: 27, text: 'MT' },
   			{ value: 28, text: 'NE' },
   			{ value: 29, text: 'NV' },
   			{ value: 30, text: 'NH' },
   			{ value: 31, text: 'NJ' },
   			{ value: 32, text: 'NM' },
   			{ value: 33, text: 'NY' },
   			{ value: 34, text: 'NC' },
   			{ value: 35, text: 'ND' },
   			{ value: 36, text: 'OH' },
   			{ value: 37, text: 'OK' },
   			{ value: 38, text: 'OR' },
   			{ value: 39, text: 'PA' },
   			{ value: 40, text: 'RI' },
   			{ value: 41, text: 'SC' },
   			{ value: 42, text: 'SD' },
   			{ value: 43, text: 'TN' },
   			{ value: 44, text: 'Tx' },
   			{ value: 45, text: 'UT' },
   			{ value: 46, text: 'VT' },
   			{ value: 47, text: 'VA' },
   			{ value: 48, text: 'WA' },
   			{ value: 49, text: 'WV' },
   			{ value: 50, text: 'WI' },
   			{ value: 51, text: 'WY' }
 		],

 		statesLimited: [
   			{ value: 1, text: 'AL' },
   			{ value: 2, text: 'AK' },
   			{ value: 3, text: 'AZ' },
   			{ value: 4, text: 'AR' },
   			{ value: 5, text: 'CA' },
   			{ value: 6, text: 'CO' },
   			{ value: 7, text: 'CT' },
   			{ value: 8, text: 'DE' },
   			{ value: 9, text: 'DC' },
   			{ value: 10, text: 'FL' },
   			{ value: 11, text: 'GA' },
   			{ value: 12, text: 'HI' },
   			{ value: 13, text: 'ID' },
   			{ value: 14, text: 'IL' },
   			{ value: 15, text: 'IN' },
   			{ value: 16, text: 'IA' },
   			{ value: 17, text: 'KS' },
   			{ value: 18, text: 'KY' },
   			{ value: 19, text: 'LA' },
   			{ value: 20, text: 'ME' },
   			{ value: 21, text: 'MD' },
   			{ value: 22, text: 'MA' },
   			{ value: 23, text: 'MI' },
   			{ value: 24, text: 'MN' },
   			{ value: 25, text: 'MS' },
   			{ value: 26, text: 'MO' },
   			{ value: 27, text: 'MT' },
   			{ value: 28, text: 'NE' },
   			{ value: 29, text: 'NV' },
   			{ value: 30, text: 'NH' },
   			{ value: 31, text: 'NJ' },
   			{ value: 32, text: 'NM' },
   			{ value: 33, text: 'NY' },
   			{ value: 34, text: 'NC' },
   			{ value: 35, text: 'ND' },
   			{ value: 36, text: 'OH' },
   			{ value: 37, text: 'OK' },
   			{ value: 38, text: 'OR' },
   			{ value: 39, text: 'PA' },
   			{ value: 40, text: 'RI' },
   			{ value: 41, text: 'SC' },
   			{ value: 42, text: 'SD' },
   			{ value: 43, text: 'TN' },
   			{ value: 44, text: 'Tx' },
   			{ value: 45, text: 'UT' },
   			{ value: 46, text: 'VT' },
   			{ value: 47, text: 'VA' },
   			{ value: 48, text: 'WA' },
   			{ value: 49, text: 'WV' },
   			{ value: 50, text: 'WI' },
   			{ value: 51, text: 'WY' }
 		],

 		wakeTypes: [
   			{ value: 0, text: 'N/A' },
   			{ value: 1, text: 'None' },
   			{ value: 2, text: 'Family Only' },
   			{ value: 3, text: 'Public' },
			{ value: USER_SPECIFIED_VALUE, text: 'User Specified' }
 		],

 		isSelected: function (item) {
			if (item && item.value)
				return (item.value !== 0 && item.value !== 1);
			return false;
 		}
 	};

 	return $window.enumService;
 }]);

