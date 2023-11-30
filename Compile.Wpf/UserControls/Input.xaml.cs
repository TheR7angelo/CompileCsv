using System.IO;
using System.Windows;
using Compile.Wpf.Dialog;

namespace Compile.Wpf.UserControls;

public partial class Input
{
    public static readonly DependencyProperty ButtonContentProperty = DependencyProperty.Register(nameof(ButtonContent),
        typeof(string), typeof(Input), new PropertyMetadata(default(string)));

    public static readonly DependencyProperty TextBoxTextProperty = DependencyProperty.Register(nameof(TextBoxText),
        typeof(string), typeof(Input), new PropertyMetadata(default(string)));

    public static readonly DependencyProperty TextBoxHelperProperty = DependencyProperty.Register(nameof(TextBoxHelper),
        typeof(string), typeof(Input), new PropertyMetadata(default(string)));

    public static readonly DependencyProperty DialogTypeProperty = DependencyProperty.Register(nameof(DialogType),
        typeof(Type), typeof(Input), new PropertyMetadata(default(Type), (o, args) =>
        {
            var sender = (Input)o;
            sender.Dialog = (IDialog)Activator.CreateInstance((Type)args.NewValue, null, null, null)!;

            if ((Type)args.NewValue != typeof(FolderDialog)) return;

            sender.SvgViewBox.Source = new Uri("/Assets/024-folder-16.svg", UriKind.Relative);
            sender.SvgViewBox.Visibility = Visibility.Visible;

        }));

    public static readonly DependencyProperty ButtonValidIconProperty =
        DependencyProperty.Register(nameof(ButtonValidIcon), typeof(Uri), typeof(Input),
            new PropertyMetadata(new Uri("/Assets/009-check.svg", UriKind.Relative)));

    public Input()
    {
        InitializeComponent();
    }

    public Uri ButtonValidIcon
    {
        get => (Uri)GetValue(ButtonValidIconProperty);
        set => SetValue(ButtonValidIconProperty, value);
    }

    public string ButtonContent
    {
        get => (string)GetValue(ButtonContentProperty);
        set => SetValue(ButtonContentProperty, value);
    }

    public string TextBoxText
    {
        get => (string)GetValue(TextBoxTextProperty);
        set => SetValue(TextBoxTextProperty, value);
    }

    public string TextBoxHelper
    {
        get => (string)GetValue(TextBoxHelperProperty);
        set => SetValue(TextBoxHelperProperty, value);
    }

    public Type DialogType
    {
        get => (Type)GetValue(DialogTypeProperty);
        set => SetValue(DialogTypeProperty, value);
    }

    private IDialog? Dialog { get; set; }

    private void ButtonDialog_OnClick(object sender, RoutedEventArgs e)
    {
        var dialog = (IDialog)Activator.CreateInstance(DialogType, ButtonContent, null, true)!;

        var files = dialog.GetFile();

        if (files is null) return;

        TryInsertText(files);
    }

    private void TextBox_PreviewDragOver(object sender, DragEventArgs e)
    {
        e.Handled = true;
    }

    private void TextBoxInput_Drop(object sender, DragEventArgs e)
    {
        if (!e.Data.GetDataPresent(DataFormats.FileDrop)) return;
        var files = ((string[])e.Data!.GetData(DataFormats.FileDrop)!).ToList();

        TryInsertText(files);
    }

    private void TryInsertText(IReadOnlyList<string> files)
    {
        var extensions = Dialog!.Extension.ToList();
        var msgError = Dialog!.ErrorMessage;

        if (extensions.Count.Equals(0) && Directory.Exists(files[0]))
        {
            TextBoxText = files[0];
        }
        else
        {
            var filesInfo = files
                .Select(chemin => new FileInfo(chemin))
                .Where(fichier => extensions.Contains(fichier.Extension));

            var uniqueFiles = filesInfo
                .GroupBy(file => Path.Join(file.Directory!.FullName, Path.GetFileNameWithoutExtension(file.Name)))
                .Select(group => group.First())
                .ToList();

            if (uniqueFiles.Count.Equals(0))
            {
                MessageBox.Show(msgError);
            }
            else
            {
                TextBoxText = string.Join('|', uniqueFiles);
            }
        }
    }

    public event EventHandler? ButtonPressed;

    protected virtual void OnButtonPressed(EventArgs e)
        => ButtonPressed?.Invoke(this, e);

    private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        => OnButtonPressed(EventArgs.Empty);

    internal void SetEnable(bool isEnable) => Grid.IsEnabled = isEnable;
}