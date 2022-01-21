using Api.Sqs;
using Api.VlsDomain;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Net;

var builder = WebApplication.CreateBuilder(args);
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
builder.Services.AddDbContext<VlsContext>(options => options.UseNpgsql(builder.Configuration["ConnectionStrings:VlsConnection"]));
builder.Services.AddHostedService<SqsReceiver>();
builder.Services.AddSingleton<EpcisObjectEventHander>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();


TokenValidationParameters GetCognitoTokenValidationParams()
{
    string cognitoIssuer = "https://cognito-idp.eu-west-1.amazonaws.com/eu-west-1_g9y4gDyhR";
    string jwtKeySetUrl = $"{cognitoIssuer}/.well-known/jwks.json";

    return new TokenValidationParameters
    {
        IssuerSigningKeyResolver = (s, securityToken, identifier, parameters) =>
        {
            // get JsonWebKeySet from AWS
            string json = new WebClient().DownloadString(jwtKeySetUrl);
            return JsonConvert.DeserializeObject<JsonWebKeySet>(json).Keys;
        },
        ValidIssuer = cognitoIssuer,
        ValidateIssuerSigningKey = true,
        ValidateIssuer = true,
        ValidateLifetime = true,
        ValidateAudience = false
    };
}
