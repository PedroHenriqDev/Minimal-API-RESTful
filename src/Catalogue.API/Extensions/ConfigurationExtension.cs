using dotenv.net;

namespace Catalogue.API.Extensions;

public static class ConfigurationExtension
{
    public static void LoadEnv(this IConfiguration configuration) 
    {
        DotEnv.Load(options: new DotEnvOptions(envFilePaths: new[]
        {
            configuration["Env:Path"], ".env"
        }));
    }
}
