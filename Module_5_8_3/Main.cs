using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module_5_8_3
{
    [Transaction(TransactionMode.Manual)]
    public class Main : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiApp = commandData.Application;
            UIDocument uiDoc = uiApp.ActiveUIDocument;
            Document doc = uiDoc.Document;

            
            ViewPlan plan = new FilteredElementCollector(doc)
                .OfClass(typeof(ViewPlan))
                .Cast<ViewPlan>()
                .FirstOrDefault(vp =>
                    vp.Name== "Level 1");

            if (plan == null)
            {
                TaskDialog.Show("Ошибка", "Не найден план первого этажа!");
                return Result.Failed;
            }

            ImageExportOptions options = new ImageExportOptions
            {
                FilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Level1.png"),
                FitDirection = FitDirectionType.Horizontal,
                ImageResolution = ImageResolution.DPI_300,
                PixelSize = 2048,
                ExportRange = ExportRange.SetOfViews
            };

            options.SetViewsAndSheets(new List<ElementId> { plan.Id });

            try
            {
                doc.ExportImage(options);
                TaskDialog.Show("Успех", "Экспорт завершен успешно!");
                return Result.Succeeded;
            }
            catch (Exception ex)
            {
                TaskDialog.Show("Ошибка", $"Ошибка экспорта: {ex.Message}");
                return Result.Failed;
            }
        }
    }
}
