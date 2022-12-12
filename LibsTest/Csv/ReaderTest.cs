using Libs;
using Libs.Csv;
using Xunit.Abstractions;

namespace LibsTest.Csv;

public class ReaderTest
{
    [Fact]
    private void ReadAllOneTest()
    {
        const string testFile =
            "C:\\Users\\ZP6177\\Creative Cloud Files\\Programmation\\C#\\Ineo Infracom\\CompileCsv\\LibsTest\\FileTest\\Search\\Classeur1.csv";
        var worker = new Reader(testFile);

        worker.ReadAll().Wait();
        var result = worker.GetResult().ToList();
        
        Assert.Equal(8, result[0].Dictionary.Count());
    }
    
    [Fact]
    private async Task ReadAllMultipleTest()
    {
        const string searchPath =
            "C:\\Users\\ZP6177\\Creative Cloud Files\\Programmation\\C#\\Ineo Infracom\\CompileCsv\\LibsTest\\FileTest\\Search";
        var search = new SearchWorker(searchPath, false);
        search.FindAll().Wait();

        var files = search.GetResults();
        
        var worker = new Reader(files);

        await worker.ReadAll();
        var result = worker.GetResult().ToList();
        result = result.OrderBy(s => s.Index).ToList();

        Assert.Equal(33, result.Count);
    }
    
}