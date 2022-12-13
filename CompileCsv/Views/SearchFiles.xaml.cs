using System.Collections.Generic;
using System.Windows;
using Libs;

namespace CompileCsv.Views;

public partial class SearchFiles
{
    public delegate void FindFilesHandler(IEnumerable<string> files);

    public event FindFilesHandler? SearchFilesOnFindFiles;
    
    public SearchFiles()
    {
        InitializeComponent();
    }

    private async void ButtonSearchCsv_OnClick(object sender, RoutedEventArgs e)
    {
        var subDirectory = CheckBoxSubDirectory.IsChecked ?? false;

        var search = TextBoxSearchPath.Text;
        var worker = new SearchWorker(search, subDirectory);
        await worker.FindAll();
        var files = worker.GetResults();
        
        OnFindFiles(files);
    }


    private void OnFindFiles(IEnumerable<string> files) => SearchFilesOnFindFiles?.Invoke(files);
}