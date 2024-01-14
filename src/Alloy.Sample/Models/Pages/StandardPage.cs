﻿using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using System.ComponentModel.DataAnnotations;
 using AlloyTemplates.Business.Rendering;
 using AlloyTemplates.Models.Blocks;

 namespace AlloyTemplates.Models.Pages
{
    /// <summary>
    /// Used for the pages mainly consisting of manually created content such as text, images, and blocks
    /// </summary>
    [SiteContentType(GUID = "9CCC8A41-5C8C-4BE0-8E73-520FF3DE8267")]
    [SiteImageUrl(Global.StaticGraphicsFolderPath + "page-type-thumbnail-standard.png")]
    public class StandardPage : SitePageData
    {
        [Display(
            GroupName = SystemTabNames.Content,
            Order = 310)]
        [CultureSpecific]
        public virtual XhtmlString MainBody { get; set; }

        [Display(
            GroupName = SystemTabNames.Content,
            Order = 320)]
        public virtual ContentArea MainContentArea { get; set; }

        [Display(
            Name = "Image",
            Description = "Please choose an image rendition",
            Order = 10,
            GroupName = SystemTabNames.Content)]
        public virtual ImageBlock ImageBlock { get; set; }
        
        
        [Display(
            GroupName = SystemTabNames.Content,
            Order = 320)]
        [CultureSpecific]
        public virtual ContentArea LocalizableContentArea { get; set; }
    }

    [SiteContentType(GUID = "3D629546-E59E-4DDC-81AF-BB2E982B38F1")]
    [SiteImageUrl(Global.StaticGraphicsFolderPath + "page-type-thumbnail-standard.png")]
    public class NumberEditorTest : PageData, IContainerPage
    {
        [Display(
            Name = "Map - Latitude",
            Description = "",
            GroupName = SystemTabNames.Content,
            Order = 330)]
        [Range(-90, 90)]
        public virtual double Latitude { get; set; }
    }

    [SiteContentType(
        GUID = "F0A2E2DB-0B9B-4709-A89D-505C4D58100A",
        GroupName = SystemTabNames.Content)]
    public class ImageBlock : SiteBlockData     {
        [Display(
            Name = "Image",
            GroupName = SystemTabNames.Content,
            Order = 10)]
        public virtual ContentReference Image { get; set; }

        [Display(
            Name = "Alt Text",
            GroupName = SystemTabNames.Content,
            Order = 20)]
        [CultureSpecific]
        public virtual string AltText { get; set; }

    }
}
