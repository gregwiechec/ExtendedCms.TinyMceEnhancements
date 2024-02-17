ExtendedCms.TinyMceEnhancements

Installation
============


In order to start using ExtendedCms.TinyMceEnhancements you need to add it explicitly to your site.
Please add the following statement to your Startup.cs

public class Startup
{
    ...
    public void ConfigureServices(IServiceCollection services)
    {
        ...

        services.Configure<TinyMceEnhancementsOptions>(options =>
        {
            options.ImageAttributes = new ()
            {
                StaticAttributes = new[]
                {
                    new ImageQueryStringAttribute
                    {
                        Name = "format",
                        Value = "webp"
                    }
                },
                ImageSizeSettings = new ()
                {
                    WidthName = "width",
                    HeightName = "height"
                }
            };
            options.ImageRestrictions = new ()
            {
                MaxWidth = 300,
                MaxHeight = 200,
                KeepRatio = true
            };
            options.ImageAltTextSettings = new ()
            {
                ImageAltAttributes = new[] { "copyright" }
            };
            options.FullWidthEnabled = true;
            options.VideoFilesEnabled = true;
        });

        ...
        services.AddTinyMceEnhancements();
        ...
    }
    ...
}
