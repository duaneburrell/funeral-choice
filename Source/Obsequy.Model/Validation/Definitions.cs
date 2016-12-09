using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Obsequy.Model
{
    public enum ValidationStatus
    {
        None = 0,
        Ok = 1,
        Invalid = 2,
        Required = 3,
        Unauthorized = 4,
		Stale = 5
    };

    public enum ValidationSeverity
    {
        None = 0,
        Ok = 1,
        Info = 2,
        Error = 3
    };

    public enum ValidationMode
    {
        None = 0,
		Create = 1,
		Update = 2,
		Available = 3,
		Pending = 5,
        Recalled = 5,
        Delete = 6,
		Accept = 7,
		Reject = 8,
		Dismiss = 9,
		Reset = 10,
		Save = 11,
		State = 12
    };

    public enum OperationResult
    {
        None = 0,
        Success = 1,
        Invalid = 2,
        Unauthorized = 3,
        Error = 4
    };

    #region State Array
    
    public static class StateArray
	{
		#region Fields
		
		private static List<UsState> _states;

		#endregion

		#region Properties

		#region Abbreviations

		public static List<string> AllAbbreviations
		{
			get { return _states.Select(item => item.Abbreviation).ToList(); }
		}

		public static List<string> LimitedAbbreviations
		{
			get { return _states.Select(item => item.Abbreviation).ToList(); }
		}

		#endregion

		#region Names

		public static List<string> AllNames
		{
			get { return _states.Select(item => item.Name).ToList(); }
		}

		public static List<string> LimitedNames
		{
			get { return AllStates.Where(item => LimitedAbbreviations.Contains(item.Abbreviation)).Select(item => item.Name).ToList(); }
		}

		#endregion

		#region States

		public static List<UsState> AllStates
		{
			get { return _states; }
		}

		public static List<UsState> LimitedStates
		{
			get { return AllStates.Where(item => LimitedAbbreviations.Contains(item.Abbreviation)).ToList(); }
		}

		#endregion

		#endregion

		static StateArray()
        {
			_states = new List<UsState>();

            _states.Add(new UsState("AL", "Alabama"));
            _states.Add(new UsState("AK", "Alaska"));
            _states.Add(new UsState("AZ", "Arizona"));
            _states.Add(new UsState("AR", "Arkansas"));
            _states.Add(new UsState("CA", "California"));
            _states.Add(new UsState("CO", "Colorado"));
            _states.Add(new UsState("CT", "Connecticut"));
            _states.Add(new UsState("DE", "Delaware"));
            _states.Add(new UsState("DC", "District Of Columbia"));
            _states.Add(new UsState("FL", "Florida"));
            _states.Add(new UsState("GA", "Georgia"));
            _states.Add(new UsState("HI", "Hawaii"));
            _states.Add(new UsState("ID", "Idaho"));
            _states.Add(new UsState("IL", "Illinois"));
            _states.Add(new UsState("IN", "Indiana"));
            _states.Add(new UsState("IA", "Iowa"));
            _states.Add(new UsState("KS", "Kansas"));
            _states.Add(new UsState("KY", "Kentucky"));
            _states.Add(new UsState("LA", "Louisiana"));
            _states.Add(new UsState("ME", "Maine"));
            _states.Add(new UsState("MD", "Maryland"));
            _states.Add(new UsState("MA", "Massachusetts"));
            _states.Add(new UsState("MI", "Michigan"));
            _states.Add(new UsState("MN", "Minnesota"));
            _states.Add(new UsState("MS", "Mississippi"));
            _states.Add(new UsState("MO", "Missouri"));
            _states.Add(new UsState("MT", "Montana"));
            _states.Add(new UsState("NE", "Nebraska"));
            _states.Add(new UsState("NV", "Nevada"));
            _states.Add(new UsState("NH", "New Hampshire"));
            _states.Add(new UsState("NJ", "New Jersey"));
            _states.Add(new UsState("NM", "New Mexico"));
            _states.Add(new UsState("NY", "New York"));
            _states.Add(new UsState("NC", "North Carolina"));
            _states.Add(new UsState("ND", "North Dakota"));
            _states.Add(new UsState("OH", "Ohio"));
            _states.Add(new UsState("OK", "Oklahoma"));
            _states.Add(new UsState("OR", "Oregon"));
            _states.Add(new UsState("PA", "Pennsylvania"));
            _states.Add(new UsState("RI", "Rhode Island"));
            _states.Add(new UsState("SC", "South Carolina"));
            _states.Add(new UsState("SD", "South Dakota"));
            _states.Add(new UsState("TN", "Tennessee"));
            _states.Add(new UsState("TX", "Texas"));
            _states.Add(new UsState("UT", "Utah"));
            _states.Add(new UsState("VT", "Vermont"));
            _states.Add(new UsState("VA", "Virginia"));
            _states.Add(new UsState("WA", "Washington"));
            _states.Add(new UsState("WV", "West Virginia"));
            _states.Add(new UsState("WI", "Wisconsin"));
            _states.Add(new UsState("WY", "Wyoming"));
        }
    }

    public class UsState
	{
		#region Properties

		public string Abbreviation { get; set; }
		public string Name { get; set; }

		#endregion

		public UsState()
        {
            Name = string.Empty;
			Abbreviation = string.Empty;
        }

		public UsState(string abbreviation, string name)
        {
			Abbreviation = abbreviation;
			Name = name;
        }

        public override string ToString()
        {
            return string.Format("{0} - {1}", Abbreviation, Name);
        }
    } 

    #endregion
}
