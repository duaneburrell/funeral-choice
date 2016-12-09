
angular.module('responseAgreement-model', [])
	.factory('ResponseAgreement', ['ChangeReceipt', function (ChangeReceipt) {
 		var ResponseAgreement = function (properties) {

 			angular.extend(this, {

 				wakeDate: undefined,
 				ceremonyDate: undefined,

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

 				transportationOfFamilyType: undefined,

 				flowerSprayType: undefined,
 				primaryFlowerType: undefined,
 				secondaryFlowerType: undefined,
 				accentFlowerType: undefined,

 				modified: new ChangeReceipt(),

 				update: function (properties) {
 					properties = properties || {};

					// update properties
 					this.wakeDate = properties.wakeDate;
 					this.ceremonyDate = properties.ceremonyDate;

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

 					this.transportationOfFamilyType = properties.transportationOfFamilyType;

 					this.flowerSprayType = properties.flowerSprayType;
 					this.primaryFlowerType = properties.primaryFlowerType;
 					this.secondaryFlowerType = properties.secondaryFlowerType;
 					this.accentFlowerType = properties.accentFlowerType;

 					this.modified.update(properties.modified);
				},

				toJSON: function () {
					return {
						wakeDate: this.wakeDate,
						ceremonyDate: this.ceremonyDate,
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
						transportationOfFamilyType: this.transportationOfFamilyType,
						flowerSprayType: this.flowerSprayType,
						primaryFlowerType: this.primaryFlowerType,
						secondaryFlowerType: this.secondaryFlowerType,
						accentFlowerType: this.accentFlowerType
					};
				}
			});

			this.update(properties);
		};

	return ResponseAgreement;
	}]);