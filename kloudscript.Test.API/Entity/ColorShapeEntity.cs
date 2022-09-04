using CsvHelper.Configuration.Attributes;

namespace kloudscript.Test.API.Entity
{
    public class ColorShapeEntity
    {
        [Index(0)]
        public string? Shape { get; set; }
        [Index(1)]
        public string? Color { get; set; }
    }

    public class ColorShortResult : ColorShapeEntity
    {
        public int? Rank { get; set; } = 0;
    }
}
