using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Libs;
using Ookii.Dialogs.Wpf;

namespace CompileCsv.Views;

public partial class ExportItems
{
    internal MainView MainView { get; set; } = null!;

    public ExportItems()
    {
        InitializeComponent();
    }
    
    private async void ButtonCheckFile_OnClick(object sender, RoutedEventArgs e)
    {
        var files = await MainView.ReadFiles();
        var check = new CompileWorker(files);
        await check.CheckHeader();
        var errors = check.GetErrorHeader();

        if (errors is not null)
        {
            var data = MainView.DisplayItems.GetFiles().ToList();
            foreach (var error in errors)
            {
                var p = data.First(s => s.Path.Equals(error));
                p.IsChecked = false;
            }

            MainView.DisplayItems.ListBoxFiles.Items.Refresh();

            MainView.ExportItems.LabelNumberSelected.Content = MainView.DisplayItems.CountSelectedFile();
            var messageTxt = new List<string>
            {
                "Des fichiers non compatible on étais détecter, ils ont étais automatiquement retirer.",
                "Voulez vous lancer quand même lancer la compilation ?"
            };

            var msg = MessageBox.Show(string.Join('\n', messageTxt),
                "Des erreurs ont étais détecter", MessageBoxButton.YesNo, MessageBoxImage.Error);

            if (msg.Equals(MessageBoxResult.No)) return;
        }

        var savePath = GetSavePath();
        if (savePath is null) return;
        
        var csvData = await check.GetCompile();

        var progress = new ProgressDialog(csvData, savePath);
        progress.ShowDialog();
    }

    private static string? GetSavePath()
    {
        string? result = null;

        var dialog = new VistaSaveFileDialog()
        {
            Filter = "Fichier csv (*.csv)|*.csv",
            DefaultExt = "csv"
        };
        if (dialog.ShowDialog().Equals(true))
        {
            result = dialog.FileName;
        }
        
        return result;
    }

    private void ButtonClearSearch_OnClick(object sender, RoutedEventArgs e)
    {
        MainView.DisplayItems.ListBoxFiles.ItemsSource = null;
        LabelNumberSelected.Content = "0";
        LabelNumberFind.Content = "0";
    }

    private void ButtonSearchCsv_OnClick(object sender, RoutedEventArgs e) =>
        MainView.SearchFiles.ButtonSearchCsv_OnClick(sender, e);
}