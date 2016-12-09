using FluentValidation.Results;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Obsequy.Utility;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Obsequy.Model
{
	public class ConsumerSchedule
	{
		#region Data Properties

		[BsonRepresentation(BsonType.Int32)]
		public ScheduleChoiceTypes ScheduleChoiceType { get; set; }

		public DateTime? WakeDate { get; set; }

		public DateTime? CeremonyDate { get; set; }

		#endregion

		#region Computed Properties

		#region Any Schedule Selections
		public bool AnyScheduleSelections
		{
			get
			{
				if (this.ScheduleChoiceType != ScheduleChoiceTypes.None || this.ScheduleChoiceType != ScheduleChoiceTypes.PrePlan)
					return (WakeDate != null || CeremonyDate != null);
				return false;
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

				if (this.ScheduleChoiceType == ScheduleChoiceTypes.Scheduled)
				{
					count += (this.WakeDate != null ? 1 : 0);
					count += (this.CeremonyDate != null ? 1 : 0);
				}
				if (this.ScheduleChoiceType == ScheduleChoiceTypes.PrePlan)
				{
					count += 1;
				}

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

				if (this.ScheduleChoiceType == ScheduleChoiceTypes.None)
				{
					count += 1;
				}
				if (this.ScheduleChoiceType == ScheduleChoiceTypes.Scheduled)
				{
					count += (this.WakeDate != null ? 1 : 0);
					count += (this.CeremonyDate != null ? 1 : 0);
				}
				if (this.ScheduleChoiceType == ScheduleChoiceTypes.PrePlan)
				{
					count += 1;
				}
				return count;
			}
		}
		#endregion

		#endregion

		#region Constructors

		public ConsumerSchedule()
			: base()
		{
		}

		#endregion

		#region Methods

		#region Clone
		public ConsumerSchedule Clone()
		{
			return new ConsumerSchedule()
			{
				ScheduleChoiceType = this.ScheduleChoiceType,
				CeremonyDate = this.CeremonyDate,
				WakeDate = this.WakeDate
			};
		}
		#endregion

		#region Has Changed
		public bool HasChanged(ConsumerSchedule schedule)
		{
			if (schedule == null)
				return false;

			if (this.ScheduleChoiceType != schedule.ScheduleChoiceType ||
                !DateTimeHelper.IsDateEqual(this.WakeDate, schedule.WakeDate) ||
                !DateTimeHelper.IsDateEqual(this.CeremonyDate, schedule.CeremonyDate))
			{
				return true;
			}

			return false;
		}
		#endregion

		#region To String
		public override string ToString()
		{
			if (this.ScheduleChoiceType == ScheduleChoiceTypes.None)
				return "No Schedule Type";
			
			if (this.ScheduleChoiceType == ScheduleChoiceTypes.PrePlan)
				return "Pre-Planning";

			if (WakeDate != null && CeremonyDate != null)
				return string.Format("Wake: {0:d}, Ceremony: {1:d}", WakeDate, CeremonyDate);
			else if (WakeDate != null && CeremonyDate == null)
				return string.Format("Wake: {0:d}", WakeDate);
			else if (WakeDate == null && CeremonyDate != null)
				return string.Format("Ceremony: {0:d}", CeremonyDate);

			return "???";
		}
		#endregion

		#region Update
		public void Update(ConsumerSchedule schedule)
		{
			this.ScheduleChoiceType = schedule.ScheduleChoiceType;

			if (schedule.ScheduleChoiceType == ScheduleChoiceTypes.Scheduled)
			{
                this.WakeDate = schedule.WakeDate;
                this.CeremonyDate = schedule.CeremonyDate;
			}
			else
			{
				this.WakeDate = null;
				this.CeremonyDate = null;
			}
		}
		#endregion

		#endregion

		#region Validation
		public ValidationResult Validate(AccountSession accountSession, ValidationMode validationMode)
		{
			return ((new ConsumerScheduleValidator(accountSession, validationMode)).Validate(this) as ValidationResult);
		}
		#endregion
	}
}