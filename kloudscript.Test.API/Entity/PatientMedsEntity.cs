using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
using System.Globalization;

namespace kloudscript.Test.API.Entity
{
    public class  PatientMedsEntity
    {
        public int PatientId { get; set; }
        public string? MedicationName { get; set; }
        public double DaysSupply { get; set; }
        public DateTime StartDate { get; set; }
        public double Cost { get; set; }

        [Ignore]
        public double FixedCost { get; set; }
    }
    
}
