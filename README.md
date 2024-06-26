# ExtendedCms.TinyMceEnhancements

ExtendedCms.TinyMceEnhancements contains set of improvements for Optimizely TinyMCE integration.

![TinyMceEnhancements](documentation/assets/TinyMceEnhancements.jpg "TinyMceEnhancements")

---

Table of contents:
* [Managing image dimensions attributes](#managing-image-dimensions)
* [Adding custom attributes](#adding-custom-attributes)
* [Set ALT text](#set-alt-text)
* [Full Width TinyMCE editor](#full-width-tinymce-editor)
* [Video files](#video-files)
* [webp server side support](#webp-sever-side-support)
* [Macro variables](#macro-variables)
* [Configuring specific features](#configuring-specific-features)

## Getting Started

To get started, you need to install ExtendedCms.TinyMceEnhancements package from EPiServer's [NuGet feed](https://nuget.episerver.com/package/?id=ExtendedCms.TinyMceEnhancements).

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

When adding an image to the HTML editor, TinyMCE automatically sets the height and width in attributes.

For example:
````
<img src="/EPiServer/CMS/Content/globalassets/en/startpage/polarbearonice.png,,128?epieditmode=false" width="300" height="175">
````

This means that the image is resized on the client, but the browser still returns the full-size image. In many cases, we would like to return the image at the size that is currently displayed on the screen.

![TinyMceEnhancements](documentation/assets/TinyMceEnhancements_set_size.jpg "TinyMceEnhancements")

To solve this problem, you can use one of the popular plug-ins, such as [Baaijte.Optimizely.ImageSharp.Web](https://github.com/vnbaaij/Baaijte.Optimizely.ImageSharp.Web). With this plugin, after entering the width and height in querystring, the image in given dimensions is returned from the server.

Unfortunately TinyMCE only adds height and width as attributes. Using **TinyMceEnhancements** it is possible to change the way the editor works. To set the names of the querystring parameters used for height and width use `ImageSizeSettings` options:

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

From now on, when adding an image to the editor or changing image size, the querystring will also be changed:

````
<img src="/EPiServer/CMS/Content/globalassets/en/startpage/polarbearonice.png,,128?epieditmode=false&amp;width=300&amp;height=17">
````

Then Baaijte.Optimizely.ImageSharp.Web (or another plugin) will return the resized image.

### Limiting image size

The plugin additionally allows you to limit the maximum size of images. This will prevent your images from becoming too large.
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

Using the code above, a `format=webp` querystring will be added to each image:

````
<img src="/EPiServer/CMS/Content/globalassets/en/startpage/polarbearonice.png,,128?epieditmode=false&amp;format=webp">
````

## Set ALT text

Alt text attribute is very important, because it makes images more accessible for both people and search engines.

Of course TinyMCE allows to set ALT attribute, but it's not mandatory.

The TinyMceEnhancements plugin can be configured to display a dialog box to enter ALT text when adding an image.

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

When you add an image to the editor, the ALT text dialog with hint will be displayed.

![TinyMceEnhancements](documentation/assets/TinyMceEnhancements_set_alt_text.jpg "TinyMceEnhancements")

## Full Width TinyMCE editor

All Optimizely properties have very similar width. This width is suitable for fields such as ContentReference or short text, but often the TinyMCE property used to store the main content of an article contains much more text and we would like the field to be wider.

![TinyMceEnhancements](documentation/assets/TinyMceEnhancements_full_width_default.jpg "TinyMceEnhancements")

Using TinyMceEnhancements we can make the TinyMCE field larger. The assumption is that the HTML editor will be the only property on the tab.
To increase the width of the editor, the `FullSizeTinyMce` attribute must be added to the property model. The attribute has two configuration options:

### Centered

The TinyMCE editor will be centred

```csharp
[Display(Name = "HTML Editor 2", GroupName = "Test2", Order = 20)]
[FullSizeTinyMce(EditorWidth = WidthType.Centered)]
public virtual XhtmlString HtmlEditorCentered { get; set; }
```

![TinyMceEnhancements](documentation/assets/TinyMceEnhancements_full_width_centered.jpg "TinyMceEnhancements")

### FullWidth

The TinyMCE editor will be stretched to the full width of the editing area

```csharp
[Display(Name = "HTML Editor 3", GroupName = "Test3", Order = 20)]
[FullSizeTinyMce(EditorWidth = WidthType.Full)]
public virtual XhtmlString HtmlEditorFullWidth { get; set; }
```
![TinyMceEnhancements](documentation/assets/TinyMceEnhancements_full_width.jpg "TinyMceEnhancements")


Feature has to be turned on in options:

```csharp
services.Configure<TinyMceEnhancementsOptions>(options =>
{
    options.FullWidthEnabled = true;
});
```

## Video files

Optimizely integration supports adding links, images and content from Assets Pane. With TinyMceEnhancements you can also add video files.

![TinyMceEnhancements](documentation/assets/TinyMceEnhancements_video.jpg "TinyMceEnhancements")

Plugin will use video tag when files has mp4, webm or ogg extension.

When addon is enabled, then whenever adding video file from Assets Pane, the video tag is created. For example:

```html
<video controls="controls" width="400" data-mce-selected="1">
    <source src="/EPiServer/CMS/Content/globalassets/en/alloy-track/alloy-track-video/alloytouch.mp4,,55?epieditmode=false" type="video/mp4" data-mce-src="/EPiServer/CMS/Content/globalassets/en/alloy-track/alloy-track-video/alloytouch.mp4,,55?epieditmode=false">
    Download the
    <a href="/EPiServer/CMS/Content/globalassets/en/alloy-track/alloy-track-video/alloytouch.mp4,,55?epieditmode=false" data-mce-href="/EPiServer/CMS/Content/globalassets/en/alloy-track/alloy-track-video/alloytouch.mp4,,55?epieditmode=false">mp4</a>
</video>
```

Feature has to be turned on in options:

```csharp
services.Configure<TinyMceEnhancementsOptions>(options =>
{
    options.VideoFilesEnabled = true;
});
```

## webp sever side support

When `DetectFormatOptimization` option is on, then server will detect if webp format is supported by the browser and when yes it will set the format.

![TinyMceEnhancements](documentation/assets/TinyMceEnhancements_serwer_side_webp_support.jpg "TinyMceEnhancements")

```csharp
services.Configure<TinyMceEnhancementsOptions>(options =>
{
    options.DetectFormatOptimization = true;
});
```

## Macro variables

Macro variables is an extension that allows inserting dynamic fields (macro) into TinyMCE editor that are replaced with custom value when rendering the page.

![TinyMceEnhancements](documentation/assets/marco_support.png "TinyMceEnhancements")

In the image above, when the page is rendered, the %%%USER_NAME%%% field will be replaced with the name of the logged-in user name from PrincipalAccessor principalAccessor?.Principal?.Identity?.Name

![TinyMceEnhancements](documentation/assets/marco_support_view.png "TinyMceEnhancements")

Similar behaviour could be achieved using blocks inside TinyMCE. However, blocks are rendered as DIV elements which makes it difficult to render dynamic values as inline inside the text.

Besides that, I have to mention, that plugin does not handle caching, so it has to be configured by the developer.

### Registering custom macro

By default, only UserName variable is registered, because sites can have different requirements for marcos.

But the plugin provides a simple way to register your own macros. All you need to do is register a class that implements the `ITinyMceMacroVariable` interface and implement fields:

| Name | Required | Description |
| ---- | ---- | ---- |
| Key  | Yes  | Unique identifier for marco variable. It will be used inside TinyMCE editor as variable. |
| DisplayName | No | Text displayed in TinyMCE toolbar dropdown. By default Key is used. |
| Rank | No | Used to sort macros. By default 100 is used. |
|GetValue | Yes | Function that returns macro value used in view mode. |

No Javascript code is required to register new macro.

### Macro example

For example , we would like to register a new macro variable, which will be replaced with a phone number for the contact. The number will be stored as a string property on StartPage.

![TinyMceEnhancements](documentation/assets/marco_support_property.png "TinyMceEnhancements")

Now we have to create `TinyMcePhoneMacro` class that implements `ITinyMceMacroVariable` interface and register it in container using `ServiceConfiguration` attribute.

When resolving macro value, we get StartPage and read ContactPhone property.

````csharp
[ServiceConfiguration(typeof(ITinyMceMacroVariable))]
public class TinyMcePhoneMacro(IContentLoader contentLoader): ITinyMceMacroVariable
{
    public string Key => "CONTACT_PHONE";
 
    public string DisplayName => "Contact phone";
 
    public string GetValue() => contentLoader.Get<StartPage>(ContentReference.StartPage).ContactPhone;
}
````

That’s it. The variable is automatically registered in TinyMCE list of available macros and Editor can start using it.

![TinyMceEnhancements](documentation/assets/marco_support_custom.png "TinyMceEnhancements")

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


