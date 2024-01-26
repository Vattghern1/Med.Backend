using Med.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Med.Backend.DAL.Data.Entities;

public class Inspection
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime CreateDate = DateTime.UtcNow;
    public DateTime InspectionDate { get; set; }
    public string? Anamnesis { get; set; }
    public string? Complaints { get; set; }
    public string? Treatment { get; set;}
    public Conclusion Conclusion { get; set; }
    public DateTime? NextVisitDate { get; set; }
    public DateTime? DeathDate { get; set; }
    public Guid? PreviousInspectionId { get; set; }
    public List<Diagnos> Diagnoses { get; set; } = new List<Diagnos>();
    public Guid DoctorId { get; set; }
    public string DoctorName { get; set; }
    public Guid PatientId { get; set; }
    public string PatientName { get; set; }
    public List<Consultation> Consultations { get; set; } = new List<Consultation>();
}
