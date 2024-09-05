namespace Catalogue.Application.Settings;

public struct ProductSettings
{
    public static int MaxName => 120;
    public static int MaxDescription => 255;
    public static int MaxPrice => 99999999;
    public static int MinPrice => 0;
}
