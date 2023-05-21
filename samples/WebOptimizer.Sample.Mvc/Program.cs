using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Options;

namespace WebOptimizer.Sample.Mvc
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddRazorPages();
            builder.Services.AddResponseCompression(o =>
            {
                o.EnableForHttps = true;
                o.Providers.Clear();
                o.Providers.Add(new BrotliCompressionProvider(new BrotliCompressionProviderOptions { }));
                o.Providers.Add(new GzipCompressionProvider(new GzipCompressionProviderOptions { }));
                o.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
                    new[] { "text/javascript" }
                );
            });
            builder.Services.AddResponseCaching();

            builder.Services.AddWebOptimizer(pipeline =>
            {
                pipeline.AddJavaScriptBundle("/js/bundle.js",
                    "/js/site1.js",
                    "/js/site2.js"
                );
            });

            var app = builder.Build();


            app.UseResponseCaching(); 
            app.UseResponseCompression();

            app.UseHttpsRedirection();
            app.UseWebOptimizer();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapRazorPages();

            app.Run();
        }
    }
}