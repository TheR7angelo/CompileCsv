using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using CompileCsv.Type;
using Libs;
using Libs.Csv;
using Libs.Type;

namespace CompileCsv.Views;

public partial class MainView
{
    public MainView()
    {
        InitializeComponent();
        
        SearchFiles.SearchFilesOnFindFiles += SearchFiles_OnFindFiles;
    }

    #region Actions

    private async void ButtonCheckFile_OnClick(object sender, RoutedEventArgs e)
    {
        var files = await ReadFiles();
        var check = new CompileWorker(files);
        await check.CheckHeader();
        var errors = check.GetErrorHeader();

        if (errors is null) return;

        var data = GetFiles().ToList();
        foreach (var error in errors)
        {
            var p =data.First(s => s.Path.Equals(error));
            p.IsChecked = false;
        }
        ListBoxFiles.Items.Refresh();
        CountSelectedFile();
    }
    
    private void CheckBoxFile_OnClick(object sender, RoutedEventArgs e) => CountSelectedFile();

    private void SearchFiles_OnFindFiles(IEnumerable<string> files)
    {
        var data = new List<SelectedFile>();
        foreach (var file in files)
        {
            var fileName = Path.GetFileName(file);
            data.Add(new SelectedFile
            {
                IsChecked = true,
                Name = fileName,
                Path = file
            });
        }

        ListBoxFiles.ItemsSource = data;
        LabelNumberFind.Content = data.Count;
        CountSelectedFile();
    }

    #endregion

    #region Function

    private void CountSelectedFile() => LabelNumberSelected.Content = GetFiles().Count(s => s.IsChecked);

    private IEnumerable<SelectedFile> GetFiles() => ListBoxFiles.Items.Cast<SelectedFile>();
    
    private async Task<IEnumerable<SIndexedDict>> ReadFiles()
    {
        var data = GetFiles();
        var reader = new Reader(data.Where(s => s.IsChecked).Select(s => s.Path));
        await reader.ReadAll();
        return reader.GetResult();
    }

    #endregion
    
}