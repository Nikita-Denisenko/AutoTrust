using AutoTrust.Application.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;

namespace AutoTrust.Infrastructure.Seed
{
    public static class BrandSeeder
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var brandService = scope.ServiceProvider.GetRequiredService<IBrandService>();

            var json = await File.ReadAllTextAsync("Seed/SeedData/brands.json");
            await brandService.LoadBrandsAsync(json, CancellationToken.None);
        }
    }
}