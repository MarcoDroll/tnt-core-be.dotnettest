using Api.Sqs;
using Api.VlsDomain;
using Api.VlsDomain.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Net;
using System.Text.Json;

WebApplicationBuilder? builder = WebApplication.CreateBuilder(args);
Microsoft.IdentityModel.Logging.IdentityModelEventSource.ShowPII = true;

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.Authority = "https://cognito-idp.eu-west-1.amazonaws.com/eu-west-1_BFBujgJ7r";
    options.TokenValidationParameters = GetCognitoTokenValidationParams();
});

builder.Services.AddDbContext<IVlsContext, VlsContext>(options => options.UseNpgsql(builder.Configuration["ConnectionStrings:VlsConnection"]));
builder.Services.AddHostedService<SqsReceiver>();
builder.Services.AddSingleton<EpcisObjectEventHander>();
builder.Services.AddScoped<EpcisObjectEventRepository>();

WebApplication? app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();


TokenValidationParameters GetCognitoTokenValidationParams()
{
    string cognitoUrl = "https://cognito-idp.eu-west-1.amazonaws.com/eu-west-1_g9y4gDyhR";
    string jwtKeySetUrl = $"{cognitoUrl}/.well-known/jwks.json";

    return new TokenValidationParameters
    {
        IssuerSigningKeyResolver = (s, securityToken, identifier, parameters) =>
        {
            string json = new HttpClient().GetStringAsync(jwtKeySetUrl).Result;
            return JsonConvert.DeserializeObject<JsonWebKeySet>(json).Keys;
        },
        ValidIssuer = cognitoUrl,
        ValidateIssuerSigningKey = true,
        ValidateIssuer = true,
        ValidateLifetime = true,
        ValidateAudience = false
    };
}
