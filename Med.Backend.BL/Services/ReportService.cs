using Med.Backend.DAL.Data;
using Med.Common.DataTransferObjects;
using Med.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;
using OfficeOpenXml;
using Microsoft.AspNetCore.Http;

namespace Med.Backend.BL.Services;

public class ReportService : IReportService
{
    private readonly ILogger<ReportService> _logger;
    private readonly BackendDbContext _backendDbContext;
    private readonly IFileService _fileService;


    public ReportService(ILogger<ReportService> logger, BackendDbContext backendDbContext, IFileService fileService)
    {
        _logger = logger;
        _backendDbContext = backendDbContext;
        _fileService = fileService;
    }

    public async Task<IcdRootsReportModel> GetIcdReport(DateTime start, DateTime finish, List<string>? icdRoots)
    {
        var response = await SearchIcdForReport(start, finish, icdRoots);
        return response;
    }

    public async Task<List<FileKeyDto>> GenerateIcdReportExcel(DateTime start, DateTime finish, List<string>? icdRoots)
    {
        var data = await SearchIcdForReport(start, finish, icdRoots);
        ExcelPackage excel = new();
        ExcelWorksheet sheet = excel.Workbook.Worksheets.Add("Отчет");

        sheet.Cells["A1"].Value = "МКБ код";
        sheet.Cells["B1"].Value = "Имя пациента";
        sheet.Cells["C1"].Value = "Дата рождения";
        sheet.Cells["D1"].Value = "Пол";

        var row = 2;
        var column = 1;
        foreach (var diagnos in data.Records)
        {
            sheet.Cells[row, column].Value = diagnos.Code;
            foreach (var patient in  diagnos.Patients)
            {
                sheet.Cells[row, column + 1].Value = patient.Name;
                sheet.Cells[row, column + 2].Value = patient.BirthDate.ToString();
                sheet.Cells[row, column + 3].Value = patient.Gender;
                row++;
            }
            row++;
        }

        sheet.Cells.AutoFitColumns();
        excel.Save();
        
        var byteArray = excel.GetAsByteArray();

        var stream = new MemoryStream(byteArray);

        List<IFormFile> files = new()
        {
            new FormFile(stream, 0, byteArray.Length, "report", $"report_file_{DateTime.UtcNow}.xlsx")
            {
                Headers = new HeaderDictionary() {},
                ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
            }
        };

        var filesKeys = await _fileService.UploadFiles(files);

        return filesKeys;
    }

    private async Task<IcdRootsReportModel> SearchIcdForReport(DateTime start, DateTime finish, List<string>? icdRoots)
    {
        string icdRootsRegex = "";
        int counter = 0;
        foreach (var icdRoot in icdRoots!)
        {
            icdRootsRegex += $"[{icdRoot[0]}-{icdRoot[4]}]";
            int start1 = Int32.Parse(Convert.ToString(icdRoot[1]) + Convert.ToString(icdRoot[2]));
            int finish1 = Int32.Parse(Convert.ToString(icdRoot[5]) + Convert.ToString(icdRoot[6]));
            if (start1 < 10)
            {
                icdRootsRegex += $"(0{start1}";
            }
            else
            {
                icdRootsRegex += $"({start1}";
            }
            while (start1 < finish1)
            {
                if (start1 + 1 < 10)
                {
                    icdRootsRegex += $"|0{start1 + 1}";
                }
                else
                {
                    icdRootsRegex += $"|{start1 + 1}";
                }
                start1++;
            }
            icdRootsRegex += $")";

            counter++;
            if (counter != icdRoots.Count())
            {
                icdRootsRegex += "|";
            }
        }
        var patientsByDiagnosis = _backendDbContext.Diagnos
            .Where(d => d.CreateTime > start && d.CreateTime < finish)
            .Where(d => Regex.IsMatch(d.IcdDiagnosisId, icdRootsRegex))
            .Include(d => d.Inspection)
            .Join(_backendDbContext.Patients, i => i.Inspection.PatientId, p => p.Id, (i, p) => new { Diagnos = i, Patient = p })
            .GroupBy(x => x.Diagnos, x => x.Patient, (diagnosis, patient) => new IcdRootsReportRecordModel
            {
                Code = diagnosis.IcdDiagnosisId,
                Patients = patient.Select(patient => new ReportPatientModelDto
                {
                    Id = patient.Id,
                    Name = patient.Name,
                    BirthDate = patient.BirthDate,
                    Gender = patient.Gender
                }).ToList(),
            })
            .ToList();

        var response = new IcdRootsReportModel
        {
            Records = patientsByDiagnosis,
            Filters = new IcdRootsReportFiltersModel
            {
                Start = start,
                End = finish,
                IcdRoots = icdRoots
            }
        };
        return response;
    }
}