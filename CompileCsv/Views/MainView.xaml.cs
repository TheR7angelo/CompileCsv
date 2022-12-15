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
        DisplayItems.MainView = this;
        
        SearchFiles.SearchFilesOnFindFiles += SearchFiles_OnFindFiles;
    }

    #region Actions

    private async void ButtonCheckFile_OnClick(object sender, RoutedEventArgs e)
    {
        var files = await ReadFiles();
        var check = new CompileWorker(files);
        await check.CheckHeader();
        var errors = check.GetErrorHeader();

        if (errors is not null)
        {
            var data = DisplayItems.GetFiles().ToList();
            foreach (var error in errors)
            {
                var p = data.First(s => s.Path.Equals(error));
                p.IsChecked = false;
            }

            DisplayItems.ListBoxFiles.Items.Refresh();

            LabelNumberSelected.Content = DisplayItems.CountSelectedFile();
            var messageTxt = new List<string>()
            {
                "Des fichiers non compatible on étais détecter, ils ont étais automatiquement retirer.",
                "Voulez vous lancer quand même lancer la compilation ?"
            };

            var msg = MessageBox.Show(string.Join('\n', messageTxt),
                "Des erreurs ont étais détecter", MessageBoxButton.YesNo, MessageBoxImage.Error);

            if (msg.Equals(MessageBoxResult.No)) return;
        }

        var csvs = await check.GetCompile();

        var writer = new Writer(csvs);
        writer.Write("Je suis un test trop bien.csv");
        
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

        DisplayItems.ListBoxFiles.ItemsSource = data;
        LabelNumberFind.Content = data.Count;
        LabelNumberSelected.Content = DisplayItems.CountSelectedFile();
    }

    #endregion

    #region Function

    // private void CountSelectedFile() => LabelNumberSelected.Content = DisplayItems.GetFiles().Count(s => s.IsChecked);

    // private IEnumerable<SelectedFile> GetFiles() => ListBoxFiles.Items.Cast<SelectedFile>();
    
    private async Task<IEnumerable<SIndexedDict>> ReadFiles()
    {
        var data = DisplayItems.GetFiles();
        var reader = new Reader(data.Where(s => s.IsChecked).Select(s => s.Path));
        await reader.ReadAll();
        return reader.GetResult();
    }

    #endregion
    
}