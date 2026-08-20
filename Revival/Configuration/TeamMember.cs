namespace Revival.Configuration;

public class TeamMember
{
    public string Name { get; set; } = "";
    public string RoleFr { get; set; } = "";
    public string RoleEn { get; set; } = "";
    public string Photo { get; set; } = "";
    public string BioShortFr { get; set; } = "";
    public string BioShortEn { get; set; } = "";

    public bool IsPublishable =>
        !string.IsNullOrWhiteSpace(Name) &&
        !string.IsNullOrWhiteSpace(RoleFr) &&
        !string.IsNullOrWhiteSpace(RoleEn);
}
