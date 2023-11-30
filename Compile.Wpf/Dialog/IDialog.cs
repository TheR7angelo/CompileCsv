namespace Compile.Wpf.Dialog;

public interface IDialog
{
    public string ErrorMessage { get; }

    public IEnumerable<string> Extension { get; }

    public string[]? GetFile();

    public string? SaveFile();
}