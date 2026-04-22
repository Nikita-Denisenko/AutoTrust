using AutoTrust.Application.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;

namespace AutoTrust.Infrastructure.Seed
{
    public static class CountrySeeder
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var countryService = scope.ServiceProvider.GetRequiredService<ICountryService>();

            var json = await File.ReadAllTextAsync("Seed/SeedData/countries.json");
            await countryService.LoadCountriesAsync(json, CancellationToken.None);
        }
    }
}