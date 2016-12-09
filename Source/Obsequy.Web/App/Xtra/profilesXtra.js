
angular.module('profiles-xtra', [])
	.service('$profiles', ['$window', '$enum', function ($window, $enum) {

 		if (!!$window.profilesXtra) {
 			return $window.profilesXtra;
 		}

 		var password = 'doggo1',
			consumer = {
				registration: [
					{ key: 'Ralph', member: { email: 'consumer1@synchro-soft.com', firstName: 'Ralph', lastName: 'Hale' }, password: password, confirmPassword: password },
					{ key: 'Marty', member: { email: 'consumer2@synchro-soft.com', firstName: 'Marty', lastName: 'Keeter' }, password: password, confirmPassword: password },
					{ key: 'Ed', member: { email: 'consumer3@synchro-soft.com', firstName: 'Ed', lastName: 'Dupas' }, password: password, confirmPassword: password },
					{ key: 'Nate', member: { email: 'consumer4@synchro-soft.com', firstName: 'Nate', lastName: 'East' }, password: password, confirmPassword: password }
				],

				account: [
				],

				principal: [
					{ key: 'Jake Roberts', firstName: 'Jake', lastName: 'Roberts' },
					{ key: 'Terry Bollea', firstName: 'Terry', lastName: 'Bollea' },
					{ key: 'John Nash', firstName: 'John', lastName: 'Nash' },
					{ key: 'Randy Orton', firstName: 'Randy', lastName: 'Orton' }
				],

				preference: [
					{ key: 'Brooklyn', proximity: { city: 'Brooklyn', state: 'NY' }, maxDistance: 20, internmentType: { value: 3 }, funeralType: { value: 4 }, wakeType: { value: 1 }, religionType: { value: 1 }, expectedAttendanceType: { value: 2 }, servicePreferences: '', remainsChoiceType: 1, casketMaterialType: { value: 2 }, casketSizeType: { value: 4 }, casketColorType: { value: 2 }, casketManufacturerType: { value: 1 }, burialContainerType: { value: 2 }, transportationChoiceType: 1, transportationOfFamilyType: { value: 2 }, flowerSprayType: { value: 1 }, primaryFlowerType: { value: 2 }, secondaryFlowerType: { value: $enum.USER_SPECIFIED_VALUE, specified: 'Orchids' }, accentFlowerType: { value: 3 } },
					{ key: 'New York', proximity: { city: 'New York', state: 'NY' }, maxDistance: 50, internmentType: { value: 2 }, funeralType: { value: 3 }, wakeType: { value: 2 }, religionType: { value: 3 }, expectedAttendanceType: { value: 5 }, servicePreferences: '', remainsChoiceType: 1, casketMaterialType: { value: 2 }, casketSizeType: { value: 3 }, casketColorType: { value: 4 }, casketManufacturerType: { value: 1 }, burialContainerType: { value: 2 }, transportationChoiceType: 2, flowerSprayType: { value: 2 }, primaryFlowerType: { value: $enum.USER_SPECIFIED_VALUE, specified: 'Tulips' }, secondaryFlowerType: { value: $enum.USER_SPECIFIED_VALUE, specified: 'Forget Me Not' }, accentFlowerType: { value: 3 } },
					{ key: 'Harlem', proximity: { city: 'Harlem', state: 'NY' }, maxDistance: 15, internmentType: { value: $enum.USER_SPECIFIED_VALUE, specified: 'Cremation' }, funeralType: { value: 2 }, wakeType: { value: 1 }, religionType: { value: 2 }, expectedAttendanceType: { value: 7 }, servicePreferences: '', remainsChoiceType: 3, casketMaterialType: { value: 1 }, casketSizeType: { value: 1 }, casketColorType: { value: 2 }, casketManufacturerType: { value: 1 }, burialContainerType: { value: 2 }, transportationChoiceType: 1, transportationOfFamilyType: { value: 4 }, flowerSprayType: { value: 2 }, primaryFlowerType: { value: 3 }, secondaryFlowerType: { value: $enum.USER_SPECIFIED_VALUE, specified: 'Lillies' }, accentFlowerType: { value: 3 } },
				],

				schedule: [
					{ key: 'February 19th & 20th', scheduleChoiceType: 1, wakeDate: '02/19/2014', ceremonyDate: '02/20/2014' },
					{ key: 'March 18th', scheduleChoiceType: 1, wakeDate: '03/18/2014' },
					{ key: 'April 1st & 3rd', scheduleChoiceType: 1, wakeDate: '04/01/2014', ceremonyDate: '04/03/2014' },
					{ key: 'Pre-Planning', scheduleChoiceType: 2 },
				]
			},

			provider = {
				registration: [
					{ key: 'Beck Funeral Home', member: { email: 'provider1@synchro-soft.com', firstName: 'Mike', lastName: 'Ness' }, password: password, confirmPassword: password, principal: { name: 'Beck Funeral Home', address: { line1: '401 Depot St', city: 'Ann Arbor', state: 'MI', zip: '48103' } } },
					{ key: 'Orchard Funeral Home', member: { email: 'provider2@synchro-soft.com', firstName: 'Billy Joe', lastName: 'Armstrong' }, password: password, confirmPassword: password, principal: { name: 'Orchard Funeral Home', address: { line1: '111 Main St', city: 'Ann Arbor', state: 'MI', zip: '48103' } } },
					{ key: 'Kayes Funeral Home', member: { email: 'provider3@synchro-soft.com', firstName: 'Johhny', lastName: 'Lawrence' }, password: password, confirmPassword: password, principal: { name: 'Kayes Funeral Home', address: { line1: '41640 Ford Rd', city: 'Canton', state: 'MI', zip: '48187' } } },
					{ key: 'Keeter & Family Funeral Home', member: { email: 'provider4@synchro-soft.com', firstName: 'Micah', lastName: 'Keeter' }, password: password, confirmPassword: password, principal: { name: 'Keeter & Family Funeral Home', address: { line1: '8057 Beechwood Blvd', city: 'Dexter', state: 'MI', zip: '48130' } } }
				],

				principal: [
					{ key: 'Beck Funeral Home', name: 'Beck Funeral Home', email: 'provider1@synchro-soft.com', phone: '734-123-4567', address: { line1: '148 Hoyt Stt', city: 'Brooklyn', state: 'NY', zip: '11217' } },
					{ key: 'Orchard Funeral Home', name: 'Orchard Funeral Home', email: 'provider2@synchro-soft.com', phone: '313-123-4567', address: { line1: '2271 Adam Clayton Powell Jr Blvd', city: 'New York', state: 'NY', zip: '10030' } },
					{ key: 'Kayes Funeral Home', name: 'Kayes Funeral Home', email: 'provider3@synchro-soft.com', phone: '734-867-5309', address: { line1: '15 E 7th St', city: 'New York', state: 'NY', zip: '10003' } },
					{ key: 'Keeter & Family Funeral Home', name: 'Keeter & Family Funeral Home', email: 'provider4@synchro-soft.com', phone: '928-987-6543', address: { line1: '16 Main St', city: 'Brooklyn', state: 'NY', zip: '11201' } }
				],

				profile: [
					{ key: 'Profile #1 ', description: 'This modern building, constructed as a funeral home in 1948, provides for our centralized professional, office, technical, transport and support staff. It is configured to accommodate up to four simultaneous visitations and can seat 250 at a service. We can record any service on video or audio and our state of the art PA system includes closed circuit TV, as well as the ability to play any personal musical selections.', businessEstablished: 4, facilityAge: 3, facilityStyle: 1, funeralDirectorExperience: 4, transportationFleetAge: 2 },
					{ key: 'Profile #2 ', description: 'We are a family owned and operated funeral home, residing in the beautiful Cochrane township.  We provide a compassionate, professional and caring experience when you need it the most.  We are a fully operating funeral home with a newly renovated chapel that seats 70 people.  For services, we utilize many of the beautiful churches or community centres that are available to us. The service may also be held at a restaurant, the family home or farm if required. We explore all of the options to make the service needed a comfortable experience. We are able to provide your family with a wide range of services, from pre-arrangements, to cremation and full burial services.', businessEstablished: 1, facilityAge: 2, facilityStyle: 4, funeralDirectorExperience: 2, transportationFleetAge: 4 },
					{ key: 'Profile #3 ', description: 'Our professional and caring staff take pride in providing high quality and affordable funeral services that meet the special needs of your family.  We offer a complete range of quality services from funerals to cremation, and are experienced at honoring many faiths and customs. We invite you to contact us with your questions, 24 hours a day, 7 days a week, and we are glad to arrange a tour of our elegant facilities. It is our goal to support you through every step of your arrangements and to pay tribute to the special memory of your loved one.', businessEstablished: 3, facilityAge: 1, facilityStyle: 3, funeralDirectorExperience: 3, transportationFleetAge: 3 },
					{ key: 'Profile #4 ', description: 'Doherty Funeral Homes has been serving the funeral needs of families in the Wilmington area for over 100 years. Today, we continue the tradition of compassion, personal attention, and professional services that started so long ago. Our distinction is that we provide the highest level of personal service to each family we serve.', businessEstablished: 2, facilityAge: 1, facilityStyle: 2, funeralDirectorExperience: 1, transportationFleetAge: 1 }
				]
			},

			login = [
				{ key: 'Ralph', email: 'consumer1@synchro-soft.com', password: password },
				{ key: 'Marty', email: 'consumer2@synchro-soft.com', password: password },
				{ key: 'Ed', email: 'consumer3@synchro-soft.com', password: password },
				{ key: 'Nate', email: 'consumer4@synchro-soft.com', password: password },
				{ key: 'Beck Funeral Home', email: 'provider1@synchro-soft.com', password: password },
				{ key: 'Orchard Funeral Home', email: 'provider2@synchro-soft.com', password: password },
				{ key: 'Kayes Funeral Home', email: 'provider3@synchro-soft.com', password: password },
				{ key: 'Keeter & Family Funeral Home', email: 'provider4@synchro-soft.com', password: password },
				{ key: 'Site Administrator', email: 'admin@synchro-soft.com', password: password }
			],

			payment = [
				{ key: 'Good Credit Card', name: 'Ralph Hale', cardNumber: '4111111111111111', expirationMonth: '12', expirationYear: '2014', securityCode: '123', cvv: undefined, postalCode: undefined, countryCode: undefined },
				{ key: 'Processor Failure', name: 'Ralph Hale', cardNumber: '4444444444444448', expirationMonth: '12', expirationYear: '2014', securityCode: '123', cvv: undefined, postalCode: undefined, countryCode: undefined },
				{ key: 'Tokenization Error', name: 'Ralph Hale', cardNumber: '4222222222222220', expirationMonth: '12', expirationYear: '2014', securityCode: '123', cvv: undefined, postalCode: undefined, countryCode: undefined },
				{ key: 'CVV Match Fail', name: 'Ralph Hale', cardNumber: '5112000200000002', expirationMonth: '12', expirationYear: '2014', securityCode: '200', cvv: undefined, postalCode: undefined, countryCode: undefined },
				{ key: 'CVV Unsupported', name: 'Ralph Hale', cardNumber: '4457000300000007', expirationMonth: '12', expirationYear: '2014', securityCode: '901', cvv: undefined, postalCode: undefined, countryCode: undefined },
			];

 		$window.profilesXtra = {

 			password: password,
 			consumer: consumer,
 			provider: provider,
 			login: login,
			payment: payment
 		};

 		return $window.profilesXtra;
	}]);
