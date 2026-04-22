using AutoTrust.Application.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;

namespace AutoTrust.Infrastructure.Seed
{
    public static class CitySeeder
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var cityService = scope.ServiceProvider.GetRequiredService<ICityService>();

            var json = await File.ReadAllTextAsync("Seed/SeedData/cities.json");
            await cityService.LoadCitiesAsync(json, CancellationToken.None);
        }
    }
}