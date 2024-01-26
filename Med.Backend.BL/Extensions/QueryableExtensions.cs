using Med.Backend.DAL.Data.Entities;
using Med.Common.Enums;

namespace Med.Backend.BL.Extensions;

public static class QueryableExtensions
{
    public static IQueryable<Patient> PatientOrderBy(this IQueryable<Patient> source, Sorting? sort)
    {
        return sort switch
        {
            Sorting.CreateAsc => source.OrderBy(x => x.CreateDate),
            Sorting.CreateDesc => source.OrderByDescending(x => x.CreateDate),
            Sorting.NameAsc => source.OrderBy(x => x.Name),
            Sorting.NameDesc => source.OrderByDescending(x => x.Name),
            Sorting.InspectionAsc => source.OrderBy(x => x.Inspections!.Count()),
            Sorting.InspectionDesc => source.OrderByDescending(x => x.Inspections!.Count()),
            _ => source
        };
    }
}
