using App.Metrics.Formatters.Prometheus;

namespace Fruitkha.Client
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseMetricsWebTracking(options =>
                {
                    options.OAuth2TrackingEnabled = true;
                })
                .UseMetricsEndpoints(options =>
                {
                    options.EnvironmentInfoEndpointEnabled = false;
                    options.MetricsTextEndpointOutputFormatter = new MetricsPrometheusTextOutputFormatter();
                    options.MetricsEndpointOutputFormatter = new MetricsPrometheusProtobufOutputFormatter();
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}