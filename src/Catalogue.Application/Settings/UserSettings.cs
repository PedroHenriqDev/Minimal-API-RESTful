namespace Catalogue.Application.Settings;

public struct UserSettings
{
    public static DateTime MaxDate => DateTime.Now;
    public static DateTime MinDate => new DateTime(1880, 1, 1);
    public static int MaxEmail => 256;
    public static int MaxName => 255;
    public static int MaxPassword => 256;
}
