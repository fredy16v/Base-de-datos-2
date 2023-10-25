using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using WebApiAutores.Entities;
using WebApiAutores.Filters;
using WebApiAutores.Middlewares;

namespace WebApiAutores;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers(options =>
        {
            options.Filters.Add(typeof(ExceptionFilter));//aplicamos a todos los controladores el filtro
        }).AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler
        = ReferenceHandler.IgnoreCycles);// para solucionar el error de entra en bucle el sql porque hay una relacion de muchos a muchos
        
        //Add DbContext
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
        });

        services.AddTransient<MiFiltro>();
        services.AddAutoMapper(typeof(Startup));
        
        // Add cache filter
        services.AddResponseCaching();

		//Add JwtConfig
		services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();

		//Add CORS
		services.AddCors(options =>
        {
            options.AddPolicy("CorsRule", rule =>
            {
                rule.AllowAnyHeader().AllowAnyMethod().WithOrigins("*");
            });
            
        });// para permitir que se conecte el backend con el forntend
        
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
    {
        /*app.Map("/semitas", app =>
        {
            app.Run(async contexto =>
            {
                await contexto.Response.WriteAsync("Interceptando la pipeline de procesos");
            });
        });*/
        //middleware
        app.UseLogginResponseHTTP();
        
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseResponseCaching();
        
        app.UseCors("CorsRule");
        
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}