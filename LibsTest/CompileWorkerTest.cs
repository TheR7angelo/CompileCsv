using Libs;
using Libs.Csv;

namespace LibsTest;

public class CompileWorkerTest
{
    [Fact]
    private void CheckHeaderTest()
    {
        const string searchPath =
            "C:\\Users\\ZP6177\\Creative Cloud Files\\Programmation\\C#\\Ineo Infracom\\CompileCsv\\LibsTest\\FileTest\\Search";
        var search = new SearchWorker(searchPath, false);
        search.FindAll().Wait();
        var files = search.GetResults();
        
        var worker = new Reader(files);

        worker.ReadAll().Wait();
        var readerResult = worker.GetResult().ToList();


        var complileWorker = new ComplileWorker(readerResult);
        complileWorker.CheckHeader().Wait();
        var check = complileWorker.GetErrorHeader();

        var expect = new List<string>
        {
            "C:\\Users\\ZP6177\\Creative Cloud Files\\Programmation\\C#\\Ineo Infracom\\CompileCsv\\LibsTest\\FileTest\\Search\\Classeur1 - Copie - Copie (5).csv"
        };
        
        Assert.Equal(expect, check);
    }
}