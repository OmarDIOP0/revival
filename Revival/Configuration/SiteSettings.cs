namespace Revival.Configuration;

public class SiteSettings
{
    public string Phone { get; set; } = "";
    public string WhatsApp { get; set; } = "";
    public string Email { get; set; } = "";
    public string AddressFr { get; set; } = "";
    public string AddressEn { get; set; } = "";
    public string HoursFr { get; set; } = "";
    public string HoursEn { get; set; } = "";
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public int Beds { get; set; } = 23;
    public int VipRooms { get; set; } = 3;
}
