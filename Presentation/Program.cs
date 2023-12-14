using DataAccess.Context;
using DataAccess.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Business.Services.Abtraction;
using Business.Services.Concered;
using DataAccess.Repositories.Abstract;
using DataAccess.Repositories.Concrete;
using Presentation.Middlewares;
using Business.MappingProfiles;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Business.Services.Abstract;
using Common.Entities;
using Microsoft.AspNetCore.Identity;
using Business.Helpers;
using Serilog;
using Serilog.Core;


var builder = WebApplication.CreateBuilder(args);






Logger log = new LoggerConfiguration()
      .WriteTo.Seq("http://localhost:5341")
      .WriteTo.File("C:\\Users\\ROG\\Desktop\\Loge\\ApiLog-.log", rollingInterval: RollingInterval.Day)
      .Enrich.FromLogContext()
      .MinimumLevel.Information()
      .CreateLogger();

builder.Host.UseSerilog(log);


// Add services to the container.





builder.Services.AddControllers();




// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();


builder.Services.AddSwaggerGen(options =>
{
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "Presentation.xml"));

    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "JWTToken_Auth_API",
        Version = "v1"
    });



    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});





builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["JWT:Issuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]))
    };
    });



// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle



builder.Services.Configure<RouteOptions>(x =>
{
    x.LowercaseUrls = true;
});

builder.Services.AddDbContext<AppDbContext>(x => x.UseSqlServer(builder.Configuration.GetConnectionString("Default"), x => x.MigrationsAssembly("DataAccess")));

builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireDigit = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.Password.RequiredLength = 0;
    options.Password.RequiredUniqueChars = 0;
})
    .AddEntityFrameworkStores<AppDbContext>();




//builder.Services.AddCors(p => p.AddDefaultPolicy(builder =>
//{
//    builder.WithOrigins("https://localhost:44395/");
//    builder.AllowAnyMethod();
//    builder.AllowAnyHeader();

//}));

//builder.Services.AddCors(p => p.AddPolicy("policy1", builder =>
//{
//    builder.WithOrigins("https://localhost:44400/");
//    builder.AllowAnyMethod();
//    builder.AllowAnyHeader();

//}));




builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddTransient<IEmailService, EmailService>();



#region Repositories

builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ITagRepository, TagRepository>();
builder.Services.AddScoped<ITeamPositionRepository, TeamPositionRepository>();
builder.Services.AddScoped<ITeamRepository, TeamRepository>();
builder.Services.AddScoped<ITestRepository ,TestRepository> ();
builder.Services.AddScoped<ICountryRepository, CountryRepository>();
builder.Services.AddScoped<IBlogRepository,BlogRepository>();
#endregion






#region UnitOfWork

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

#endregion

#region Services

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ITagService, TagService>();
builder.Services.AddScoped<ITeamPositionService, TeamPostionService>();
builder.Services.AddScoped<ITeamService, TeamService>();
builder.Services.AddScoped<ITestService, TestService>();    
builder.Services.AddScoped<ICountryService, CountryService>();
builder.Services.AddScoped<IBlogService, BlogService>();
#endregion





builder.Services.AddAutoMapper(x =>
{
x.AddProfile(new CategoryMappingProfile());
x.AddProfile(new RoleMappingProfile());
x.AddProfile(new AuthMappingProfile());
x.AddProfile(new ProductMappingProfile());
x.AddProfile(new TagMappingProfile());
x.AddProfile(new TeamPositionMappingProfile());
x.AddProfile(new TeamMappingProfile());
x.AddProfile(new TestMappingProfile());
x.AddProfile(new CountryMappingProfile());
x.AddProfile(new BlogMappingProfile());
});






var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
 
}







app.UseHttpsRedirection();
//app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.UseMiddleware<CustomExceptionMiddleware>();

app.Run();
