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
            //this process is setting the subject
            //we are constructing the explicit type here so that it is injected with additional configuration
            //we have to specify a connection string that a client is going to need
            services.AddSingleton<IMongoClient>(serviceprovider =>
            {
                //whenever mongo sees guid in any entities,it should actually serialize them as a string in database(for guid and datetimeoffset)
                BsonSerializer.RegisterSerializer(new GuidSerializer (BsonType.String));
                BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer (BsonType.String));

                var settings = Configuration.GetSection(nameof(MongoDbSettings)).Get<MongoDbSettings>();
                //.get changes the output from IConfiguration to MongoDbsettings
                return new MongoClient(settings.ConnectionString);
                //construct IMongoClient instance
            });

            //we have declared the type of the explicit type of the dependency to inject
            //services.AddSingleton<IItemsRepository,InMemItemsRepository>();
            services.AddSingleton<IItemsRepository,MongoDbItemsRepository>();
            //now switching to mongo client for saving data

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Catalog", Version = "v1" });
            });
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

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
