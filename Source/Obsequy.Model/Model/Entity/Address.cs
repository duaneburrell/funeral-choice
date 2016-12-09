using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver.GeoJsonObjectModel;
using MongoRepository;
using Obsequy.Utility;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Obsequy.Model
{
	public class Address : IGeoCodeable
	{
		#region Data Properties

		[BsonRepresentation(BsonType.String)]
		public string Line1 { get; set; }

		[BsonRepresentation(BsonType.String)]
		public string Line2 { get; set; }

		[BsonRepresentation(BsonType.String)]
		public string City { get; set; }

		[BsonRepresentation(BsonType.String)]
		public string State { get; set; }

		[BsonRepresentation(BsonType.String)]
		public string Zip { get; set; }

		[BsonRepresentation(BsonType.Double)]
		public double? Latitude { get; set; }

		[BsonRepresentation(BsonType.Double)]
		public double? Longitude { get; set; }

		[BsonRepresentation(BsonType.Double)]
		public double[] Location { get; set; }

		[BsonRepresentation(BsonType.Int32)]
		public int? GeocodingStatusCode { get; set; }

		#endregion

		#region Properties

		#region Can Geo-Code
		[BsonIgnore]
		public bool CanGeoCode
		{
			get
			{
				if (!string.IsNullOrEmpty(this.Line1) &&
					!string.IsNullOrEmpty(this.City) &&
					!string.IsNullOrEmpty(this.State) &&
					!string.IsNullOrEmpty(this.Zip))
					return true;

				return false;
			}
		}
		#endregion

		#region Geocoding Coordinates
		[BsonIgnore]
		public string GeocodingCoordinates
		{
			// NOTE: I don't know if this is correct or not, but I'd rather use this when determining
			// distances between 2 points.
			get { return string.Format("{0},{1}", this.Latitude, this.Longitude); }
		}
		#endregion

		#region Geocoding Status
		[BsonIgnore]
		public GoogleMapsApi.Entities.Geocoding.Response.Status? GeocodingStatus
		{
			get
			{
				if (this.GeocodingStatusCode != null)
				{
					if (this.GeocodingStatusCode.Value == Convert.ToInt32(GoogleMapsApi.Entities.Geocoding.Response.Status.OK))
						return GoogleMapsApi.Entities.Geocoding.Response.Status.OK;
					if (this.GeocodingStatusCode.Value == Convert.ToInt32(GoogleMapsApi.Entities.Geocoding.Response.Status.ZERO_RESULTS))
						return GoogleMapsApi.Entities.Geocoding.Response.Status.ZERO_RESULTS;
					if (this.GeocodingStatusCode.Value == Convert.ToInt32(GoogleMapsApi.Entities.Geocoding.Response.Status.OVER_QUERY_LIMIT))
						return GoogleMapsApi.Entities.Geocoding.Response.Status.OVER_QUERY_LIMIT;
					if (this.GeocodingStatusCode.Value == Convert.ToInt32(GoogleMapsApi.Entities.Geocoding.Response.Status.REQUEST_DENIED))
						return GoogleMapsApi.Entities.Geocoding.Response.Status.REQUEST_DENIED;
					if (this.GeocodingStatusCode.Value == Convert.ToInt32(GoogleMapsApi.Entities.Geocoding.Response.Status.INVALID_REQUEST))
						return GoogleMapsApi.Entities.Geocoding.Response.Status.INVALID_REQUEST;
				}

				return null;
			}
			set
			{
				if (value != null)
					this.GeocodingStatusCode = Convert.ToInt32(value);
				else
					this.GeocodingStatusCode = null;
			}
		}
		#endregion

		#region Geocoding Query
		[BsonIgnore]
		public string GeocodingQuery
		{
			get { return string.Format("{0}, {1} {2} {3}", this.Line1, this.City, this.State, this.Zip); }
		}
		#endregion

		#region Has Location
		[BsonIgnore]
		public bool HasLocation
		{
			get
			{
				if (this.Latitude != null && this.Longitude != null)
					return true;
				return false;
			}
		}
		#endregion

		#endregion

		#region Constructors

		public Address()
			: base()
		{
		}

		#endregion

		#region Methods

		#region Clone
		public Address Clone()
		{
			return new Address()
			{
				Line1 = this.Line1,
				Line2 = this.Line2,
				City = this.City,
				State = this.State,
				Zip = this.Zip,
				Latitude = this.Latitude,
				Longitude = this.Longitude,
				GeocodingStatusCode = this.GeocodingStatusCode
			};
		}
		#endregion

		#region Geo-Code Location
		public void GeoCodeLocation()
		{
            // call the geo-coder
            var result = GeoCoder.LookUpGeoLocation(this);

            if (result.Action == GeoCoder.GeoCodeResult.GeoCodeAction.Clear)
            {
                // set the status
                this.GeocodingStatus = result.GeocodingStatus;

                // set the latitude, longitude, and DB location
                this.Latitude = null;
                this.Longitude = null;
                this.Location = null;
            }

            else if (result.Action == GeoCoder.GeoCodeResult.GeoCodeAction.Set)
            {
                // set the status
                this.GeocodingStatus = result.GeocodingStatus;

                // set the latitude, longitude, and DB location
                this.Latitude = Convert.ToDouble(result.Latitude);
                this.Longitude = Convert.ToDouble(result.Longitude);
                this.Location = new[] { this.Longitude.Value, this.Latitude.Value };
            }
		}
		#endregion

		#region Has Changed
		public bool HasChanged(Address address)
		{
			if (this.Latitude == null || this.Longitude == null)
				return true;

			if (this.Line1 != address.Line1.Scrub() ||
				this.Line2 != address.Line2.Scrub() ||
				this.City != address.City.Scrub() ||
				this.State != address.State.Scrub() ||
				this.Zip != address.Zip.Scrub())
				return true;

			return false;
		}
		#endregion

		#region Is Geo Coding Required
		public bool IsGeoCodingRequired(Address address)
		{
			if (!this.HasLocation)
				return true;

			if (HasChanged(address))
				return true;

			return false;
		}
		#endregion

		#region To String
		public override string ToString()
		{
			return string.Format("{0}, {1}", City, State);
		}
		#endregion

		#region Update
		public void Update(Address address)
		{
			// update all properties	
			this.Line1 = address.Line1.Scrub();
			this.Line2 = address.Line2.Scrub();
			this.City = address.City.Scrub();
			this.State = address.State.Scrub();
			this.Zip = address.Zip.Scrub();
		}
		#endregion
		
		#endregion
	}
}