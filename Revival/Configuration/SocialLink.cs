namespace Revival.Configuration;

public class SocialLink
{
    public string Name { get; set; } = "";
    public string Url { get; set; } = "";
    public string Icon { get; set; } = "";

    public bool IsActive => !string.IsNullOrWhiteSpace(Url);
}
