using Libs;
using Libs.Csv;

namespace LibsTest.Csv;

public class WiterTest
{
    [Fact]
    private void Writer()
    {
        const string output =
            "C:\\Users\\ZP6177\\Creative Cloud Files\\Programmation\\C#\\Ineo Infracom\\CompileCsv\\LibsTest\\Csv\\test.csv";
        
        const string searchPath =
            "C:\\Users\\ZP6177\\Creative Cloud Files\\Programmation\\C#\\Ineo Infracom\\CompileCsv\\LibsTest\\FileTest\\Search";
        var search = new SearchWorker(searchPath, false);
        search.FindAll().Wait();
        var files = search.GetResults();
        
        var workerReader = new Reader(files);

        workerReader.ReadAll().Wait();
        var readerResult = workerReader.GetResult().ToList();

        var complileWorker = new CompileWorker(readerResult);
        complileWorker.CheckHeader().Wait();
        var check = complileWorker.GetErrorHeader();
        
        var data = complileWorker.GetCompile();
        data.Wait();
        
        var worker = new Writer(data.Result);
        
        worker.Write(output);
        
        Assert.True(File.Exists(output));
    }
}