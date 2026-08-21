namespace Revival.Models;

public class ContactFormModel
{
    public string Name { get; set; } = "";
    public string Phone { get; set; } = "";
    public string Email { get; set; } = "";
    public string Reason { get; set; } = "";
    public string Message { get; set; } = "";

    /// <summary>Honeypot — real visitors never fill this field.</summary>
    public string Website { get; set; } = "";
}

public class ContactPageModel
{
    public ContactFormModel Form { get; set; } = new();
    public bool Submitted { get; set; }
    public HashSet<string> FieldErrors { get; set; } = new();
}
