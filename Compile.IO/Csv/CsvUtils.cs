using Compile.Models.IO;

namespace Compile.IO.Csv;

public static class CsvUtils
{
    public static CsvCompile CompileCsvFiles(this IEnumerable<string> files)
    {
        var compiles = new Dictionary<string, List<Dictionary<string, object>>>();
        var fileError = new List<CsvCompileError>();

        foreach (var file in files)
        {
            List<Dictionary<string, object>> records;
            try
            {
                records = file.ReadCsv().ToList();
            }
            catch (Exception e)
            {
                fileError.Add(new CsvCompileError
                {
                    Exception = e,
                    File = file
                });
                continue;
            }

            if (records.Count == 0) continue;

            var header = string.Join(";", records.First().Keys);

            if (compiles.TryGetValue(header, out var compile))
            {
                compile.AddRange(records);
            }
            else
            {
                compiles[header] = records;
            }
        }

        var result = new CsvCompile
        {
            Compile = compiles,
            CsvCompileErrors = fileError
        };

        return result;
    }
}