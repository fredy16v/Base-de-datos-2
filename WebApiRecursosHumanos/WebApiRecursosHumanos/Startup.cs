using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using WebApiRecursosHumanos.Entities;

namespace WebApiRecursosHumanos;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }
    
    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers().AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler
        = ReferenceHandler.IgnoreCycles);// para solucionar el error de entra en bucle el sql porque hay una relacion de muchos a muchos
        
        //Add DbContext
        services.AddDbContext<ApplicationDbContextRecursosHumanos>(options =>
        {
            options.UseSqlServer(Configuration.GetConnectionString("RecursosHumanosConnection"));
        });
        
        services.AddDbContext<ApplicationDbContextProyectos>(options =>
        {
            options.UseSqlServer(Configuration.GetConnectionString("ProyectosConnection"));
        });
        
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        
        app.UseHttpsRedirection();

        app.UseRouting();
        
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}