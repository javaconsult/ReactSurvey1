//see GlobalUsings.cs

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
// Add services to the container.
IServiceCollection services = builder.Services;
services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();


ConfigurationManager configurationManager = builder.Configuration;
//get key vault uri from appsettings https://javaconsultvault1.vault.azure.net
string vaultUri = configurationManager["KeyVault:VaultUri"];

//enable reading from key vault
IWebHostEnvironment webHostEnvironment = builder.Environment;
string connectionString;
if (webHostEnvironment.IsProduction())
{
    //https://docs.microsoft.com/en-us/dotnet/api/azure.identity.defaultazurecredential
    //DefaultAzureCredential searches through credentials including EnvironmentCredential, ManagedIdentityCredential, VisualStudioCredential and AzureCliCredential
    //When deployed to Azure, the application uses the System Assigned Managed Identity to authenticate
    configurationManager.AddAzureKeyVault(new Uri(vaultUri), new DefaultAzureCredential()); //The vault setting supercedes appsettings.json
    //retrieve connection string from keyvault secret named ConnectionStrings--AzureConnection if present, otherwise from ConnectionStrings/AzureConnection in appsettings.json
    connectionString = configurationManager["ConnectionStrings:AzureConnection"];    
}
else
{
    //used by migrations. retrieve connection string from ConnectionStrings/AzureConnection in appsettings.Development.json    
    connectionString = configurationManager["ConnectionStrings:AzureConnection"];
    //connectionString = configurationManager["ConnectionStrings:SqlServerLocalConnection"];   
}

//add DbContext to service collection
services.AddDbContext<SurveyDbContext>(opt => opt.UseSqlServer(connectionString));
//services.AddDbContext<SurveyDbContext>(opt => opt.UseSqlite(connectionString));

//enables injecting SecretClient into KeyVaultController
services.AddAzureClients(builder => builder.AddSecretClient(new Uri(vaultUri)));

//in production, CORS not required as api and react application are hosted by same server

if (webHostEnvironment.IsDevelopment() )
{
    services.AddCors(opt =>
    {
        opt.AddPolicy("CorsPolicy", policy =>
        {
            //policy.AllowAnyMethod().AllowAnyHeader().WithOrigins("http://localhost:3000");
            policy.AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin();
        });
    });
}

//Dependency injection of Repository and Service interfaces
services.AddTransient<ISurveyRepository, SurveyRepository>(ctx => new SurveyRepository(ctx.GetService<SurveyDbContext>()!));
services.AddTransient<IParticipantRepository, ParticipantRepository>(ctx => new ParticipantRepository(ctx.GetService<SurveyDbContext>()!));
services.AddTransient<IAccountService, AccountService>();

services.AddTransient<ISurveyService, SurveyService>();
services.AddTransient<IParticipantService, ParticipantService>();
//enable DI of TokenService, scoped to lifetime of HTTP request
services.AddScoped<TokenService>();
//https://www.c-sharpcorner.com/article/integrate-automapper-in-net-core-web-api2/
services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

//Adds and configures the identity system for the specified User type.
services.AddIdentityCore<AdminUser>(opt =>
{
    opt.SignIn.RequireConfirmedAccount = false;
    opt.Password.RequireNonAlphanumeric = false;
    opt.Password.RequireDigit = false;
    opt.Password.RequireLowercase = false;
    opt.Password.RequireUppercase = false;
    opt.Password.RequiredLength = 3;
})
    //Adds an Entity Framework implementation of identity information stores.
    .AddEntityFrameworkStores<SurveyDbContext>()
    //Adds a SignInManager<TUser> for the UserType.
    //also need to add app.UseAuthentication();
    .AddSignInManager<SignInManager<AdminUser>>();


//JWT authentication
//Microsoft.AspNetCore.Authentication.JwtBearer package
//To verify JWT signature, get 256 bit (32 character) token key from keyvault secret named Settings--TokenKey if present, otherwise from Settings/TokenKey in appsettings 
string key = configurationManager["Settings:TokenKey"];

services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(jwtBearerOptions =>
    {
        jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });






// Configure the HTTP request pipeline.
WebApplication webApplication = builder.Build(); //IApplicationBuilder
if (webHostEnvironment.IsDevelopment())
{
    webApplication.UseSwagger();
    webApplication.UseSwaggerUI();
}
webApplication.UseHttpsRedirection();
//webApplication.UseRouting();
webApplication.UseDefaultFiles(); //looks for index.html in wwwroot folder
webApplication.UseStaticFiles(); //serve static files from wwwroot
webApplication.UseCors("CorsPolicy");
webApplication.UseAuthentication();//must be before authorization
webApplication.UseAuthorization();
webApplication.MapControllers();
//without this, there would be a 404 response to a url such as /login or /home
webApplication.MapFallbackToController("Index", "Fallback"); //handles react router urls 

webApplication.UseDeveloperExceptionPage();

await webApplication.RunAsync();
