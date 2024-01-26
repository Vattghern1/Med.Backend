using Med.Common.Enums;

namespace Med.Common.DataTransferObjects;
public class IcdRootsReportRecordModel
{
    public string Code { get; set; }
    public List<ReportPatientModelDto> Patients { get; set; }
}

