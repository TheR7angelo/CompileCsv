using System.Collections.Generic;
using System.Windows;
using Libs;
using Ookii.Dialogs.Wpf;

namespace CompileCsv.Views;

public partial class SearchFiles
{
    public delegate void FindFilesHandler(IEnumerable<string> files);

    public event FindFilesHandler? SearchFilesOnFindFiles;
    
    public SearchFiles()
    {
        InitializeComponent();
    }

    internal async void ButtonSearchCsv_OnClick(object sender, RoutedEventArgs e)
    {
        var subDirectory = CheckBoxSubDirectory.IsChecked ?? false;

        var search = TextBoxSearchPath.Text;
        if (search.Equals(string.Empty)) return;
        
        var worker = new SearchWorker(search, subDirectory);
        await worker.FindAll();
        var files = worker.GetResults();
        
        OnFindFiles(files);
    }


    private void OnFindFiles(IEnumerable<string> files) => SearchFilesOnFindFiles?.Invoke(files);

    private void ButtonSearchPath_OnClick(object sender, RoutedEventArgs e)
    {
        var dialog = new VistaFolderBrowserDialog();
        if (!dialog.ShowDialog().Equals(true)) return;

        TextBoxSearchPath.Text = dialog.SelectedPath;
    }
}