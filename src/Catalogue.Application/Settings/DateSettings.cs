namespace Catalogue.Application.Settings;

public struct DateSettings
{
    public static DateTime MaxDate => DateTime.Now;
    public static DateTime MinDate => new DateTime(1880, 1, 1);
}
