using System.Globalization;
using System.Text;
using CsvHelper.Configuration;
using Ude;

namespace Compile.IO.Csv;

public static class CsvReader
{
    static CsvReader()
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
    }

    public static IEnumerable<Dictionary<string, object>> ReadCsv(this string filePath)
    {
        var csvConfiguration = new CsvConfiguration(CultureInfo.InvariantCulture) { DetectDelimiter = true };

        using var fileStream = File.OpenRead(filePath);

        var detector = new CharsetDetector();
        detector.Feed(fileStream);
        detector.DataEnd();

        var encodingName = string.IsNullOrEmpty(detector.Charset)
            ? "UTF-8"
            : detector.Charset;

        var encoding = Encoding.GetEncoding(encodingName);

        using var streamReader = new StreamReader(filePath, encoding, true, new FileStreamOptions { Access = FileAccess.Read, Share = FileShare.ReadWrite });

        using var csvReader = new CsvHelper.CsvReader(streamReader, csvConfiguration);

        var records = csvReader.GetRecords<dynamic>();

        var results = records.Select(record =>
        {
            var dictionary = ((IDictionary<string, object>)record).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            return dictionary;
        }).ToList();

        return results;
    }
}