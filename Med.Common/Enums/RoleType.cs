using System.ComponentModel.DataAnnotations;

namespace Med.Common.Enums;

public enum RoleType
{
    [Display(Name = ApplicationRoleNames.Administrator)]
    Administrator,

    [Display(Name = ApplicationRoleNames.User)]
    User,
}
public class ApplicationRoleNames
{
    public const string Administrator = "Administrator";
    public const string User = "User";
}