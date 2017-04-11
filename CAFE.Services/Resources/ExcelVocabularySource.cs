using CAFE.Core.Integration;
using CAFE.Core.Resources;
using ClosedXML.Excel;
using CsvHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAFE.Services.Resources
{
    public class ExcelVocabularySource : IExternalVocabularySource
    {
        private byte[] _file;
        private bool _isExcel;

        public ExcelVocabularySource(byte[] file, bool isExcel)
        {
            _file = file;
            _isExcel = isExcel;
        }

        public Dictionary<string, string> GetValuesFromFile()
        {
            var importData = new Dictionary<string, string>();
            var requiredFields = new[] { "Value", "Description" };
            if (_isExcel)
                using (var stream = new MemoryStream(_file))
                {
                    try
                    {
                        var xmlImportFile = new XLWorkbook(stream);
                        var ws = xmlImportFile.Worksheet(1);
                        var itemsToImportCount = ws.Columns().Max(x => x.CellsUsed().Count());

                        int dataColumnsCount = ws.Columns().Count();
                        int numberOfNonEmptyValues = ws.Columns().Max(x => x.CellsUsed().Count());

                        var workBookheaders = new List<string> { };

                        for (int i = 1; i <= dataColumnsCount; i++)
                        {
                            var dataColumn = ws.Column(i);
                            var cellValue = dataColumn.Cell(1).Value?.ToString();
                            if (String.Empty != cellValue)
                                workBookheaders.Add(cellValue);
                        }

                        if (workBookheaders.Count != requiredFields.Length)
                            throw new ArgumentException($"Required number of headers is: {requiredFields.Length}. Your excel has {workBookheaders.Count} fields.");

                        for (var i = 0; i < workBookheaders.Count; i++)
                            if (!requiredFields[i].Equals(workBookheaders[i]))
                                throw new ArgumentException($"File's header '{workBookheaders[i]}' not equial to required header '{requiredFields[i]}'.");

                        for (int i = 2; i <= numberOfNonEmptyValues; i++)
                        {
                            var row = ws.Row(i);
                            var objectsList = new Dictionary<string, string>();
                            int j = 0;
                            foreach (var property in requiredFields)
                            {
                                var excelValue = row.Cell(j+++1).Value?.ToString();
                                objectsList.Add(property, excelValue);
                            }
                            importData.Add(objectsList[requiredFields[0]], objectsList[requiredFields[1]]);
                        }

                    }
                    catch (Exception ex) { throw ex; }
                }
            else
            {
                try
                {
                    using (var memoryStream = new MemoryStream(_file))
                    using (var textReader = new StreamReader(memoryStream, Encoding.Default))
                    {
                        var csv = new CsvReader(textReader);
                        var row = 0;
                        while (csv.Read())
                        {
                            row++;
                            var objectsList = new Dictionary<string, string>();
                            foreach (var property in requiredFields)
                            {
                                var excelValue = csv.GetField<string>(property);
                                objectsList.Add(property, excelValue);
                            }
                            importData.Add(objectsList[requiredFields[0]], objectsList[requiredFields[1]]);
                        }
                    }
                }
                catch (Exception ex) { throw ex; }
            }

            return importData;
        }

        public List<Dictionary<string, object>> GetCollectionValuesFromFile(System.Type AIClassType)
        {
            return _isExcel ? GetAIDataArrayFromExcel(AIClassType) : GetAIDataArrayFromCSV(AIClassType);
        }

        bool IsSimple(System.Type type)
        {
            return type.IsPrimitive
              || type.IsEnum
              || type.Equals(typeof(string))
              || type.Equals(typeof(decimal));
        }

        public List<Dictionary<string, object>> GetAIDataArrayFromExcel(System.Type type)
        {
            var properties = type.GetProperties();

            var resultArray = new List<Dictionary<string, object>>();
            try
            {
                using (var stream = new MemoryStream(_file))
                {
                    var xmlImportFile = new XLWorkbook(stream);
                    var ws = xmlImportFile.Worksheet(1);
                    var itemsToImportCount = ws.Columns().Max(x => x.CellsUsed().Count());

                    int dataColumnsCount = ws.Columns().Count();
                    int numberOfNonEmptyValues = ws.Columns().Max(x => x.CellsUsed().Count());

                    var workBookheaders = new List<string> { };

                    for (int i = 1; i <= dataColumnsCount; i++)
                    {
                        var dataColumn = ws.Column(i);
                        var cellValue = dataColumn.Cell(1).Value?.ToString();
                        if (String.Empty != cellValue)
                            workBookheaders.Add(cellValue);
                    }

                    if (workBookheaders.Count != properties.Length)
                        throw new ArgumentException($"Required number of headers is: {properties.Length}. Your excel has {workBookheaders.Count} headers.");

                    for (var i = 0; i < workBookheaders.Count; i++)
                        if (!properties[i].Name.Equals(workBookheaders[i]))
                            throw new ArgumentException($"File's header '{workBookheaders[i]}' not equial to required header '{properties[i].Name}'.");
                       
                    for (int i = 2; i <= numberOfNonEmptyValues; i++)
                    {
                        var row = ws.Row(i);

                        var objectsList = new Dictionary<string, object>();
                        int j = 0;
                        foreach (var property in properties)
                        {
                            if (IsSimple(property.PropertyType))
                            {
                                try
                                {
                                    var excelValue = row.Cell(j + 1).Value;
                                    var converted = Convert.ChangeType(excelValue, property.PropertyType);
                                    objectsList.Add(property.Name, converted);
                                }
                                catch (Exception ex)
                                {
                                    throw new ArgumentException(
                                        String.Format("Unable to get value from row {0}, column {1}. Value should has type: {2}",
                                        i, property.Name, property.PropertyType.Name)
                                    );
                                }
                            }

                            else
                            {
                                var excelValue = row.Cell(j + 1).Value?.ToString();
                                objectsList.Add(property.Name, new
                                {
                                    value = excelValue,
                                    uri = String.Empty
                                });
                            }
                            j++;
                        }
                        resultArray.Add(objectsList);
                    }
                }
            }
            catch (Exception ex) { throw ex; }

            return resultArray;
        }

        public List<Dictionary<string, object>> GetAIDataArrayFromCSV(System.Type type)
        {
            var properties = type.GetProperties();
           
            var resultArray = new List<Dictionary<string, object>>();
            try
            {
                using (var memoryStream = new MemoryStream(_file))
                using (var textReader = new StreamReader(memoryStream, Encoding.Default))
                {
                    var csv = new CsvReader(textReader);
                    var row = 0;
                    while (csv.Read())
                    {
                        row++;
                        var objectsList = new Dictionary<string, object>();
                        foreach (var property in properties)
                        {
                            if (IsSimple(property.PropertyType))
                            {
                                try
                                {
                                    var excelValue = csv.GetField(property.PropertyType, property.Name);
                                    objectsList.Add(property.Name, excelValue);
                                }
                                catch (Exception ex)
                                {
                                    throw new ArgumentException(
                                        String.Format("Unable to get value from row {0}, column {1}. Value should has type: {2}",
                                        row, property.Name, property.PropertyType.Name)
                                    );
                                }
                            }

                            else
                            {
                                var excelValue = csv.GetField<string>(property.Name);
                                objectsList.Add(property.Name, new
                                {
                                    value = excelValue,
                                    uri = String.Empty
                                });
                            }
                        }
                        resultArray.Add(objectsList);
                    }
                }
            }
            catch (Exception ex) {
                throw ex;
            }

            return resultArray;
        }

        public byte[] CreateHeadersFile(System.Type type)
        {
            return _isExcel ? CreateExcelHeadersFile(type) : CreateCSVHeadersFile(type);
        }

        public byte[] CreateExcelHeadersFile(System.Type type)
        {
            try
            {
                var properties = type.GetProperties();
                var headers = properties.Select(p => p.Name).ToArray();

                using (var stream = new MemoryStream())
                {
                    var workbook = new XLWorkbook();
                    var worksheets = workbook.Worksheets.Add("Main");

                    for (var i = 0; i < properties.Length; i++)
                        worksheets.Cell(1, i+1).SetValue(properties[i].Name);

                    workbook.SaveAs(stream);
                    return stream.ToArray();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public byte[] CreateCSVHeadersFile(System.Type type)
        {
            try
            {
                var properties = type.GetProperties();
                var headers = properties.Select(p => p.Name).ToArray();

                using (var stream = new MemoryStream())
                using (var textWriter = new StreamWriter(stream))
                using (var csvWriter = new CsvWriter(textWriter))
                {
                    foreach (var header in headers)
                        csvWriter.WriteField(header);

                    csvWriter.NextRecord();
                    textWriter.Flush();
                    return stream.ToArray();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}