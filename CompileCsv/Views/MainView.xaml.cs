using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CompileCsv.Type;
using Libs.Csv;
using Libs.Type;

namespace CompileCsv.Views;

public partial class MainView
{
    public MainView()
    {
        InitializeComponent();
        DisplayItems.MainView = this;
        ExportItems.MainView = this;
        
        SearchFiles.SearchFilesOnFindFiles += SearchFiles_OnFindFiles;
    }

    #region Actions

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

        DisplayItems.ListBoxFiles.ItemsSource = data;
        ExportItems.LabelNumberFind.Content = data.Count;
        ExportItems.LabelNumberSelected.Content = DisplayItems.CountSelectedFile();
    }

    #endregion

    #region Function

    internal async Task<IEnumerable<SIndexedDict>> ReadFiles()
    {
        var data = DisplayItems.GetFiles();
        var reader = new Reader(data.Where(s => s.IsChecked).Select(s => s.Path));
        await reader.ReadAll();
        return reader.GetResult();
    }

    internal void ClearSearch() => DisplayItems.ListBoxFiles.ItemsSource = null;

    #endregion
    
}