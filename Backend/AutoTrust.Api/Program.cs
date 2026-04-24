using AutoTrust.Application.Interfaces.Repositories;
using AutoTrust.Application.Interfaces.Services;
using AutoTrust.Application.Interfaces.Validators;
using AutoTrust.Application.Mappings;
using AutoTrust.Application.Services;
using AutoTrust.Application.Validators;
using AutoTrust.Infrastructure.Data;
using AutoTrust.Infrastructure.Seed;
using AutoTrust.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddAutoMapper(typeof(SharedMappingProfile).Assembly);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
                new MySqlServerVersion(new Version(9, 5, 0))
    )
);

builder.Services.AddScoped(typeof(IRepository<>), typeof(IRepository<>));


builder.Services.AddScoped<IAccountValidator, AccountValidator>();
builder.Services.AddScoped<IBrandValidator, BrandValidator>();
builder.Services.AddScoped<IBuyListingValidator, BuyListingValidator>();
builder.Services.AddScoped<ICarValidator, CarValidator>();
builder.Services.AddScoped<IChatValidator, ChatValidator>();
builder.Services.AddScoped<ICommentValidator, CommentValidator>();
builder.Services.AddScoped<IFollowValidator, FollowValidator>();
builder.Services.AddScoped<ILocationValidator, LocationValidator>();
builder.Services.AddScoped<IMessageValidator, MessageValidator>();
builder.Services.AddScoped<IModelValidator, ModelValidator>();
builder.Services.AddScoped<INotificationValidator, NotificationValidator>();
builder.Services.AddScoped<IReactionValidator, ReactionValidator>();
builder.Services.AddScoped<IReviewValidator, ReviewValidator>();
builder.Services.AddScoped<ISaleListingValidator, SaleListingValidator>();
builder.Services.AddScoped<IUserValidator, UserValidator>();


builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IBrandService, BrandService>();
builder.Services.AddScoped<ICarService, CarService>();
builder.Services.AddScoped<IChatService, ChatService>();
builder.Services.AddScoped<ICityService, CityService>();
builder.Services.AddScoped<ICommentService, CommentService>();
builder.Services.AddScoped<ICountryService, CountryService>();
builder.Services.AddScoped<IFollowService, FollowService>();
builder.Services.AddScoped<IListingService, ListingService>();
builder.Services.AddScoped<IMessageService, MessageService>();
builder.Services.AddScoped<IModelService, ModelService>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<IReactionService, ReactionService>();
builder.Services.AddScoped<IReviewService, ReviewService>();
builder.Services.AddScoped<IUserService, UserService>();


builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IBrandService, BrandService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    await CountrySeeder.SeedAsync(scope.ServiceProvider);
    await CitySeeder.SeedAsync(scope.ServiceProvider);
}

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.Run();