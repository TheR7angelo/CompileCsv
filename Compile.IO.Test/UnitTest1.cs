using Compile.IO.Csv;

namespace Compile.IO.Test;

public class UnitTest1
{
    [Fact]
    public void Test2()
    {
        const string directory = @"T:\- 4 Suivi Appuis\20-APCOM\3_Historique des dépôts\CSV\RIP33";
        var files = Directory.GetFiles(directory, "*.csv");

        var result = files.CompileCsvFiles();
        result.WriteCsv("test.csv");
    }

    [Fact]
    public void Test1()
    {
        const string directory = @"T:\- 4 Suivi Appuis\20-APCOM\3_Historique des dépôts\CSV\RIP33";
        var files = Directory.GetFiles(directory, "*.csv");

        var result = files.CompileCsvFiles();
    }
}