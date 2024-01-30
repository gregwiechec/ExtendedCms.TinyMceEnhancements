# ExtendedCms.TinyMceEnhancements

ExtendedCms.TinyMceEnhancements contains set of enhancements for Optimizely TinyMCE integration.

![TinyMceEnhancements](documentation/assets/TinyMceEnhancements.jpg "TinyMceEnhancements")

* [Managing image dimensions attributes](#managing-image-dimensions)
* [Adding custom attributes](#adding-custom-attributes)
* [Set ALT text](#set-alt-text)

## Getting Started

To get started you need to install ExtendedCms.TinyMceEnhancements package from EPiServer's [NuGet feed](https://nuget.episerver.com/).

```csharp
public void ConfigureServices(IServiceCollection services)
{
    //...

    services.Configure<TinyMceEnhancementsOptions>(uiOptions =>
    {
        uiOptions.ImageAttributes = new ()
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
        uiOptions.ImageRestrictions = new ()
        {
            MaxWidth = 300,
            MaxHeight = 200,
            KeepRatio = true
        };
        uiOptions.ImageAltTextSettings = new ()
        {
            ImageAltAttributes = new[] { "copyright" }
        };
    });
    
    services.......
        .AddTinyMce()
        .AddTinyMceEnhancements() // Turn on TinyMCEEnhancements addon

   // ...
}
````

## Managing image dimensions

When adding an image to the HTML editor, TinyMCE automatically sets the height and width in the attributes.

![TinyMceEnhancements](documentation/assets/TinyMceEnhancements_set_size.jpg "TinyMceEnhancements")

For example:
````
<img src="/EPiServer/CMS/Content/globalassets/en/startpage/polarbearonice.png,,128?epieditmode=false" width="300" height="175">.
````

This means that the image is resized on the client, but the browser still returns the full-size image. In many cases, we would like to return the image at the size that is currently displayed on the screen.

To solve this problem, you can use one of the popular plug-ins, such as [Baaijte.Optimizely.ImageSharp.Web](https://github.com/vnbaaij/Baaijte.Optimizely.ImageSharp.Web).

Thanks to it, after entering the width and height in querystring, the image in given dimensions is returned from the server.

Unfortunately TinyMCE only adds height and width as attributes. Using **TinyMceEnhancements** it is possible to change the way the editor works.

To do this, set the names of the querystring parameters used for height and width in the `ImageSizeSettings`:

````csharp
services.Configure<TinyMceEnhancementsOptions>(options =>
{
    options.ImageAttributes = new ()
    {
        ImageSizeSettings = new ()
        {
            WidthName = "width",
            HeightName = "height"
        }
    };
}
````

From now on, when adding an image to the editor, the querystring is also changed:

````
<img src="/EPiServer/CMS/Content/globalassets/en/startpage/polarbearonice.png,,128?epieditmode=false&amp;width=300&amp;height=17">.
````

so that Baaijte.Optimizely.ImageSharp.Web (or another plugin) will return the image at the resized size.

### Limiting image size

The plugin additionally allows you to limit the maximum size of images. This will prevent images from being enlarged too much.
To configure the maximum size of images, you need to configure the `ImageRestrictions` option:

````csharp
public void ConfigureServices(IServiceCollection services)
{
    services.Configure<TinyMceEnhancementsOptions>(uiOptions =>
    {
        uiOptions.ImageRestrictions = new ()
        {
            MaxWidth = 300,
            MaxHeight = 200,
            KeepRatio = true
        };
    });
}
````

## Adding custom attributes

Some plugins require additional query string parameters to be added. E.g. Baaijte.Optimizely.ImageSharp.Web will return images in webp format if the querystring contains `format=webp`.

To add static attributes to an image querystring using TinyMceEnhancements, configure `StaticAttributes`:


````csharp
public void ConfigureServices(IServiceCollection services)
{
    //...

    services.Configure<TinyMceEnhancementsOptions>(uiOptions =>.
    {
        uiOptions.ImageAttributes = new ()
        {
            StaticAttributes = new[]
            {
                new ImageQueryStringAttribute
                {
                    Name = "format",
                    Value = "webp"
                }
            }
        };
    });

   // ...
}
````

Using the code above, a `format=webp` will be added to each image:

````
<img src="/EPiServer/CMS/Content/globalassets/en/startpage/polarbearonice.png,,128?epieditmode=false&amp;format=webp">
````

## Set ALT text

Alt text attribute is very important, because it makes images more accessible for both people and search engines.

Of course TinyMCE allows to set ALT attribute, but it's not mandatory.

The TinyMceEnhancements plugin can be configured to display a dialog box to wrpowadz ALT text when adding an image.

In addition, you can configure the default ALT text completed when the dialog is displayed. To add a default ALT text, configure the `ImageAltTextSettings` option:

````csharp
public void ConfigureServices(IServiceCollection services)
{
    services.Configure<TinyMceEnhancementsOptions>(uiOptions =>
    {
        uiOptions.ImageAltTextSettings = new ()
        {
            ImageAltAttributes = new[] { "copyright" }
        };
    });
}
````

When you add an image to the editor, the ALT text hint dialog will be displayed.

![TinyMceEnhancements](documentation/assets/TinyMceEnhancements_set_alt_text.jpg "TinyMceEnhancements")

## Configuring specific features

By default managing both image size and ALT text are enabled. It's configurable using `AddTinyMceEnhancements` method:

```csharp
public void ConfigureServices(IServiceCollection services)
{
    //...

    var configureImageAttributes = false;
    var configureImageAlt = true;
    services.......
        .AddTinyMce()
        .AddTinyMceEnhancements(configureImageAttributes, configureImageAlt) // Turn on TinyMCEEnhancements addon

   // ...
}
````

You can also turn it on only for specific TinyMce instances, for example:

````csharp
public static class ServiceCollectionExtensions
{
    public static IServiceCollection CustomizeTinyMce(this IServiceCollection services)
    {
        services.Configure<TinyMceConfiguration>(config =>
        {
            config.For<ArticlePage>(t => t.SharedBody)
              .ConfigureImagePlugin()
              .ConfigureImageAltPlugin();
	}
    }
}
````


