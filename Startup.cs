using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Catalog.Repositories;
using MongoDB.Driver;
using Catalog.Settings;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using System.Text.Json;
using System.Net.Mime;
using Microsoft.AspNetCore.Http;

namespace Catalog
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.It is used to register all the services.
        public void ConfigureServices(IServiceCollection services)
        {
            //whenever mongo sees guid in any entities,it should actually serialize them as a string in database(for guid and datetimeoffset)
            BsonSerializer.RegisterSerializer(new GuidSerializer (BsonType.String));
            BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer (BsonType.String));
            var mongoDbSettings = Configuration.GetSection(nameof(MongoDbSettings)).Get<MongoDbSettings>();

            //this process is setting the subject
            //we are constructing the explicit type here so that it is injected with additional configuration
            //we have to specify a connection string that a client is going to need
            services.AddSingleton<IMongoClient>(serviceprovider =>
            {
                

                //var settings = Configuration.GetSection(nameof(MongoDbSettings)).Get<MongoDbSettings>();
                //.get changes the output from IConfiguration to MongoDbsettings
                return new MongoClient(mongoDbSettings.ConnectionString);
                //construct IMongoClient instance
            });

            //we have declared the type of the explicit type of the dependency to inject
            //services.AddSingleton<IItemsRepository,InMemItemsRepository>();
            services.AddSingleton<IItemsRepository,MongoDbItemsRepository>();
            //now switching to mongo client for saving data

            services.AddControllers(options=>
            {
                options.SuppressAsyncSuffixInActionNames =false;
                //by default at runtime dotnet framework supress the async suffix, the above command is to fix that behaviour
            });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Catalog", Version = "v1" });
            });
            services.AddHealthChecks() 
                    .AddMongoDb(
                        mongoDbSettings.ConnectionString ,
                        name:"mongodb", 
                        timeout:TimeSpan.FromSeconds(3),
                        tags: new[]{ "ready" });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Catalog v1"));
            }

            if(env.IsDevelopment(){
                app.UseHttpsRedirection();
            })
            
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

                endpoints.MapHealthChecks("/health/ready", new HealthCheckOptions{
                    Predicate = (check)=> check.Tags.Contains("ready"),
                    ResponseWriter = async(context,report)=>
                    {
                        var result = JsonSerializer.Serialize(
                            new{
                                status = report.Status.ToString(),
                                checks = report.Entries.Select(entry => new{
                                    name = entry.Key,
                                    status = entry.Value.Status.ToString(),
                                    exception = entry.Value.Exception != null ? entry.Value.Exception.Message : "none",
                                    duration = entry.Value.Duration.ToString()
                                })
                            }
                        );
                        context.Response.ContentType = MediaTypeNames.Application.Json;
                        await context.Response.WriteAsync(result);
                    }
                });
                //middleware /health is route(name)
                //ready make sure that database is ready to serve request 

                endpoints.MapHealthChecks("/health/live", new HealthCheckOptions{
                    Predicate = (_)=> false
                    //predicate is filter
                    //life make sure that our site,our service is up and running                
                });
            });
        }
    }
}
