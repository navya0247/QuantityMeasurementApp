using System;
using System.IO;
using System.Text.Json;

namespace QuantityMeasurementApp.RepoLayer.Utilities
{
    public class ApplicationConfig
    {
        private static ApplicationConfig _instance;
        private readonly JsonDocument _config;

        private ApplicationConfig()
        {
            // Look in running directory first, then base directory
            string path = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory, "appsettings.json");

            if (!File.Exists(path))
                path = "appsettings.json";

            if (!File.Exists(path))
                throw new FileNotFoundException("appsettings.json not found.");

            string json = File.ReadAllText(path);
            _config = JsonDocument.Parse(json);
        }

        public static ApplicationConfig GetInstance()
        {
            _instance ??= new ApplicationConfig();
            return _instance;
        }

        public string GetConnectionString()
        {
            // System property override takes precedence
            string env = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");
            if (!string.IsNullOrWhiteSpace(env)) return env;

            return _config.RootElement
                .GetProperty("ConnectionStrings")
                .GetProperty("DefaultConnection")
                .GetString();
        }

        public string GetTestConnectionString()
        {
            return _config.RootElement
                .GetProperty("ConnectionStrings")
                .GetProperty("TestConnection")
                .GetString();
        }

        public string GetRepositoryType()
        {
            // System property override takes precedence
            string env = Environment.GetEnvironmentVariable("REPOSITORY_TYPE");
            if (!string.IsNullOrWhiteSpace(env)) return env.ToLower();

            return _config.RootElement
                .GetProperty("AppSettings")
                .GetProperty("RepositoryType")
                .GetString() ?? "database";
        }

        public int GetMaxPoolSize()
        {
            string env = Environment.GetEnvironmentVariable("MAX_POOL_SIZE");
            if (!string.IsNullOrWhiteSpace(env) && int.TryParse(env, out int envVal))
                return envVal;

            string val = _config.RootElement
                .GetProperty("AppSettings")
                .GetProperty("MaxPoolSize")
                .GetString();
            return int.TryParse(val, out int result) ? result : 10;
        }

        public string GetEnvironment()
        {
            return _config.RootElement
                .GetProperty("AppSettings")
                .GetProperty("Environment")
                .GetString() ?? "development";
        }
    }
}