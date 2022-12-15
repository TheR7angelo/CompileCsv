using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Libs.Csv;

namespace CompileCsv.Views;

public partial class ProgressDialog
{
    private readonly CancellationTokenSource _cancellationTokenSource = new();
    
    
    private IEnumerable<Dictionary<string, string>> CsvData { get; }
    private string SavePath { get; }

    public ProgressDialog(IEnumerable<Dictionary<string, string>> csvData, string savePath)
    {
        CsvData = csvData;
        SavePath = savePath;
        InitializeComponent();
    }

    private async void StartWork()
    {
        var progress = new Progress<float>(percent => ProgressBar.Value = percent);
        var writer = new Writer(CsvData);
        await Task.Run(() => writer.Write(SavePath, ";", progress), _cancellationTokenSource.Token);
        Close();
    }
    
    private void ProgressDialog_OnClosing(object? sender, CancelEventArgs e) => _cancellationTokenSource.Cancel();

    private void ProgressDialog_OnActivated(object? sender, EventArgs e) => StartWork();

    private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
        _cancellationTokenSource.Cancel();
        Close();
    }
}