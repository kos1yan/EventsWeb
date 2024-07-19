using BusinessLogicLayer.MappingProfiles;
using Events.Extensions;
using Events.Middlewares;
using Microsoft.AspNetCore.HttpOverrides;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureCors();
builder.Services.ConfigureRepositoryManager();
builder.Services.ConfigureServiceManager();
builder.Services.AddRoleRequirementHandler();
builder.Services.AddValidation();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.ConfigureSqlContext(builder.Configuration);
builder.Services.AddCloudinaryConfiguration(builder.Configuration);
builder.Services.AddAuthentication();
builder.Services.AddAuthorizationPolicy();
builder.Services.ConfigureIdentity();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigureSwagger();
builder.Services.AddAutoMapper(typeof(EventMappingProfile).Assembly);
builder.Services.ConfigureJWT(builder.Configuration);
builder.Services.AddJwtConfiguration(builder.Configuration);


var app = builder.Build();

FirebaseApp.Create(new AppOptions()
{
    Credential = GoogleCredential.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "events-d5903-firebase-adminsdk-joccw-ecc37b2593.json"))
});

app.ConfigureExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(s =>
    {
        s.SwaggerEndpoint("/swagger/v1/swagger.json", "Event API v1");
    });
}

app.UseHttpsRedirection();

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.All
});

app.UseCors("CorsPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
