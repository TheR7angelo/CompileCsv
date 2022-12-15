using System.Collections.Generic;
using System.Linq;
using System.Windows;
using CompileCsv.Type;

namespace CompileCsv.Views;

public partial class DisplayItems
{
    internal MainView MainView { get; set; } = null!;

    public DisplayItems()
    {
        InitializeComponent();
    }

    internal int CountSelectedFile() => GetFiles().Count(s => s.IsChecked);
    
    internal IEnumerable<SelectedFile> GetFiles() => ListBoxFiles.Items.Cast<SelectedFile>();

    private void CheckBoxFile_OnClick(object sender, RoutedEventArgs e) =>
        MainView.ExportItems.LabelNumberSelected.Content = CountSelectedFile();
}