using Libs;

namespace LibsTest;

public class SearchTest
{
    private static readonly string FileSearch =
        "C:\\Users\\ZP6177\\Creative Cloud Files\\Programmation\\C#\\Ineo Infracom\\CompileCsv\\LibsTest\\FileTest\\Search";
    
    [Fact]
    private void SearchWorkerWithNoSubDirTest()
    {
        var worker = new SearchWorker(FileSearch, false);
        worker.FindAll().Wait();

        var files = worker.GetResults();
        
        Assert.Equal(33, files.Count);
    }
    
    [Fact]
    private void SearchWorkerWithSubDirTest()
    {
        var worker = new SearchWorker(FileSearch, true);
        worker.FindAll().Wait();

        var files = worker.GetResults();
        
        Assert.Equal(97, files.Count);
    }
}