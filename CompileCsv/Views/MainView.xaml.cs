using System.Collections.Generic;
using System.IO;
using CompileCsv.Type;

namespace CompileCsv.Views;

public partial class MainView
{
    public MainView()
    {
        InitializeComponent();
        
        SearchFiles.SearchFilesOnFindFiles += SearchFiles_OnFindFiles;
    }

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
    }
}