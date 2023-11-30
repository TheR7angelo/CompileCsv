using System.ComponentModel;

namespace Compile.Models.IO;

public class CsvCompile
{
    public required Dictionary<string, List<Dictionary<string, object>>> Compile { get; set; }
    public List<CsvCompileError>? CsvCompileErrors { get; set; }
}

public class CsvCompileError
{
    [DisplayName("Fichier")]
    public required string File { get; set; }

    [DisplayName("Erreur")]
    public required Exception Exception { get; set; }
}