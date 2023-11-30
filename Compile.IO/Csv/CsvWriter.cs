using System.Globalization;
using Compile.Models.IO;
using CsvHelper.Configuration;

namespace Compile.IO.Csv;

public static class CsvWriter
{
    public static void WriteCsv(this CsvCompile csvCompile, string fileToSave)
    {
        var configuration = new CsvConfiguration(CultureInfo.InvariantCulture) { Delimiter = ";" };

        var extension = Path.GetExtension(fileToSave);
        fileToSave = Path.GetFileNameWithoutExtension(fileToSave);

        var compiles = csvCompile.Compile;
        var needIndex = compiles.Keys.Count > 1;
        foreach (var kvp in compiles.Select((value, i) => new { i, value }))
        {
            var records = kvp.value.Value;

            var filePath = needIndex
                ? $"{fileToSave}_{kvp.i + 1}{extension}"
                : $"{fileToSave}{extension}";

            using var writer = new StreamWriter(filePath);
            using var csv = new CsvHelper.CsvWriter(writer, configuration);

            if (records.Count.Equals(0)) continue;

            foreach (var key in records.First().Keys)
            {
                csv.WriteField(key);
            }
            csv.NextRecord();

            foreach (var record in records)
            {
                foreach (var obj in record.Values)
                {
                    csv.WriteField(obj);
                }
                csv.NextRecord();
            }
        }
    }
}