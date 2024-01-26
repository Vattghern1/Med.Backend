namespace Med.Common.DataTransferObjects;

public  class IcdRootsReportModel
{
    public IcdRootsReportFiltersModel Filters { get; set; }
    public List<IcdRootsReportRecordModel> Records { get; set; }
} 
