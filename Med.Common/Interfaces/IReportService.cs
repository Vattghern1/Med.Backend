using Med.Common.DataTransferObjects;

namespace Med.Common.Interfaces;

public interface IReportService
{
    public Task<IcdRootsReportModel> GetIcdReport(DateTime start, DateTime finish, List<string>? icdRoots);

    public Task<List<FileKeyDto>> GenerateIcdReportExcel(DateTime start, DateTime finish, List<string>? icdRoots);
}