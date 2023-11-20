using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

namespace OrderApi.Tests
{
    public class ConfigurationSectionStub : IConfigurationSection
    {
        private readonly Dictionary<string, string> _data = new Dictionary<string, string>
    {
        { "SecretKey", "this is my secret key for order api" }
    };

        public string this[string key]
        {
            get => _data[key];
            set => _data[key] = value;
        }

        public IConfigurationSection GetSection(string SecretKey) => throw new NotImplementedException();
        public IEnumerable<IConfigurationSection> GetChildren() => throw new NotImplementedException();
        public IChangeToken GetReloadToken() => throw new NotImplementedException();
        public string Key => throw new NotImplementedException();
        public string Path => throw new NotImplementedException();

        public string? Value { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        string? IConfiguration.this[string key] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }

    public static class Configuration
    {
        public static IConfiguration InitConfiguration()
        {
            var config = new ConfigurationBuilder()
                 .SetBasePath(Directory.GetCurrentDirectory())
                 .AddJsonFile("appsettings.test.json")
                 .AddEnvironmentVariables()
                 .Build();
            return config;
        }
    }

}