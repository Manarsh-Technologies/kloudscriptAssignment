using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;

namespace kloudscript.Test.API.Services
{
    public interface ICsvParserService
    {
        Task<List<T>> ReadCsvFile<T>(string filePath);
        Task<bool> WriteCsvFile<T>(List<T> listObj, string filePath);
    }
    public class CsvParserService: ICsvParserService
    {
        public async Task<List<T>> ReadCsvFile<T>(string filePath)
        {
            return await Task.Run(() =>
            {
                List<T> ReturnContents = new List<T>();

                var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    HasHeaderRecord = false,
                };
                using (TextReader fileReader = File.OpenText(filePath))
                {
                    fileReader.ReadLine();
                    using (CsvReader ReadCsv = new CsvReader(fileReader, config))
                    {
                        //ReturnContents.AddRange(ReadCsv.GetRecords<ColorShapeEntity>());

                        var data = ReadCsv.GetRecords<T>();
                        ReturnContents.AddRange(data);
                    }
                }
                return ReturnContents;
            }); 
        }

        public async Task<bool> WriteCsvFile<T>(List<T> listObj, string filePath)
        {
            await Task.Run(() =>
            {
                using (var writer = new StreamWriter(filePath))
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    csv.WriteHeader<T>();
                    csv.NextRecord();
                    foreach (var record in listObj)
                    {
                        csv.WriteRecord(record);
                        csv.NextRecord();
                    }
                }
            });
            return true;
        }
    }
}
