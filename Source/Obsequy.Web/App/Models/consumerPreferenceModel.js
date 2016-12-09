

angular.module('consumerPreference-model', [])
	 .factory('ConsumerPreference', ['$enum', 'Address', function ($enum, Address) {
 		var ConsumerPreference = function (properties) {

 			angular.extend(this, {

 				maxDistance: undefined,
 				proximity: new Address(),

 				remainsChoiceType: undefined,

 				internmentType: undefined,
 				funeralType: undefined,
 				wakeType: undefined,
 				religionType: undefined,
 				expectedAttendanceType: undefined,
 				servicePreferences: undefined,

 				casketMaterialType: undefined,
 				casketSizeType: undefined,
 				casketColorType: undefined,
 				casketManufacturerType: undefined,
 				burialContainerType: undefined,

 				transportationChoiceType: undefined,
 				transportationOfFamilyType: undefined,

 				flowerSprayType: undefined,
 				primaryFlowerType: undefined,
 				secondaryFlowerType: undefined,
 				accentFlowerType: undefined,

 				isCompleted: undefined,
 				percentComplete: undefined,
 				specifiedOptionsCount: undefined,
 				totalOptionsCount: undefined,

 				anyCasketSelections: undefined,
 				anyFlowerSelections: undefined,
 				anyServiceSelections: undefined,
 				anyTransportationSelections: undefined,

 				update: function (properties) {
 					properties = properties || {};

 					// update properties
 					this.maxDistance = properties.maxDistance;
 					this.proximity.update(properties.proximity);

 					this.remainsChoiceType = properties.remainsChoiceType;

 					this.internmentType = properties.internmentType;
 					this.funeralType = properties.funeralType;
 					this.wakeType = properties.wakeType;
 					this.religionType = properties.religionType;
 					this.expectedAttendanceType = properties.expectedAttendanceType;
 					this.servicePreferences = properties.servicePreferences;

 					this.casketMaterialType = properties.casketMaterialType;
 					this.casketSizeType = properties.casketSizeType;
 					this.casketColorType = properties.casketColorType;
 					this.casketManufacturerType = properties.casketManufacturerType;
 					this.burialContainerType = properties.burialContainerType;

 					this.transportationChoiceType = properties.transportationChoiceType;
 					this.transportationOfFamilyType = properties.transportationOfFamilyType;

 					this.flowerSprayType = properties.flowerSprayType;
 					this.primaryFlowerType = properties.primaryFlowerType;
 					this.secondaryFlowerType = properties.secondaryFlowerType;
 					this.accentFlowerType = properties.accentFlowerType;

 					this.isCompleted = properties.isCompleted;
 					this.percentComplete = properties.percentComplete;
 					this.specifiedOptionsCount = properties.specifiedOptionsCount;
 					this.totalOptionsCount = properties.totalOptionsCount;

 					this.anyCasketSelections = properties.anyCasketSelections;
 					this.anyFlowerSelections = properties.anyFlowerSelections;
 					this.anyServiceSelections = properties.anyServiceSelections;
 					this.anyTransportationSelections = properties.anyTransportationSelections;
				},

				toJSON: function () {
					return {
						maxDistance: this.maxDistance,
						proximity: this.proximity.toJSON(),
						remainsChoiceType: this.remainsChoiceType,
						internmentType: this.internmentType,
						funeralType: this.funeralType,
						wakeType: this.wakeType,
						religionType: this.religionType,
						expectedAttendanceType: this.expectedAttendanceType,
						servicePreferences: this.servicePreferences,
						casketMaterialType: this.casketMaterialType,
						casketSizeType: this.casketSizeType,
						casketColorType: this.casketColorType,
						casketManufacturerType: this.casketManufacturerType,
						burialContainerType: this.burialContainerType,
						transportationChoiceType: this.transportationChoiceType,
						transportationOfFamilyType: this.transportationOfFamilyType,
						flowerSprayType: this.flowerSprayType,
						primaryFlowerType: this.primaryFlowerType,
						secondaryFlowerType: this.secondaryFlowerType,
						accentFlowerType: this.accentFlowerType,
						anyCasketSelections: this.anyCasketSelections,
						anyFlowerSelections: this.anyFlowerSelections,
						anyServiceSelections: this.anyServiceSelections,
						anyTransportationSelections: this.anyTransportationSelections
					};
				}
			});

			this.update(properties);
		};

 		return ConsumerPreference;
	 }]);