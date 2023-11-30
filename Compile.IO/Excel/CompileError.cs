using Compile.Models.IO;
using OfficeOpenXml;
using OfficeOpenXml.Table;

namespace Compile.IO.Excel;

public static class CompileError
{
    static CompileError()
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
    }

    public static void WriteError(this IEnumerable<CsvCompileError> csvCompileErrors, string fileToSave)
    {
        fileToSave = Path.ChangeExtension(fileToSave, ".xlsx");

        using var package = new ExcelPackage();
        var workbook = package.Workbook;
        var worksheet = workbook.Worksheets.Add("Erreurs compilation");

        var range = worksheet.Cells["A1"].LoadFromCollection(csvCompileErrors, true, TableStyles.Medium7);
        range.AutoFitColumns();

        package.SaveAs(fileToSave);
    }
}