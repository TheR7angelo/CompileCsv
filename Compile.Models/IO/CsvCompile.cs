namespace Compile.Models.IO;

public class CsvCompile
{
    public required Dictionary<string, List<Dictionary<string, object>>> Compile { get; set; }
    public List<CsvCompileError>? CsvCompileErrors { get; set; }
}

public class CsvCompileError
{
    public required string File { get; set; }
    public required Exception Exception { get; set; }
}