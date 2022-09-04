namespace kloudscript.Test.API.Entity
{
    public class StateByZipEntity
    {
        public string? resultStatus { get; set; }
        public string? zip5 { get; set; }
        public string? defaultCity { get; set; }
        public string? defaultState { get; set; }
        public string? defaultRecordType { get; set; }
        public List<CityList>? citiesList { get; set; }
        
    }
    public class CityList
    {
        public string? city { get; set; }
        public string? state { get; set; }
    }
}
