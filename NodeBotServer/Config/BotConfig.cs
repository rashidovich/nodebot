using Microsoft.Extensions.Configuration;

namespace NodeBotServer.Config
{
    public class BotConfig
    {
        public Auth Auth { get; set; }

        public static BotConfig Bind(IConfiguration configuration, string section = "BotConfig")
        {
            var jobConfig = new BotConfig();
            configuration.GetSection(section).Bind(jobConfig);

            return jobConfig;
        }
    }

    public class Auth 
    {
        public string Value { get; set; }
    }
}
