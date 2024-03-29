﻿using System;
using System.Drawing;

using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

using PasteItCleaned.Plugin.Helpers;
using PasteItCleaned.Core.Helpers;
using PasteItCleaned.WmfConverter.Controllers.Entities;

namespace PasteItCleaned.WmfConverter.Controllers
{
    [ApiController]
    [Route("v1/convert")]
    [EnableCors("Default")]
    public class MetafileConverterController : ControllerBase
    {
        // GET v1/convert
        [HttpGet()]
        public ActionResult Get()
        {
            return Ok("Up and running");
        }

        // POST v1/convert
        [HttpPost()]
        public ActionResult Post([FromBody] ConvertObject obj)
        {
            Console.WriteLine("MetafileConverterController::Post");

            try
            {
                var bytes = Base64Helper.GetBytes(obj.wmfBase64);
                var metafileOriginal = ImageHelper.ConvertToMetafile(bytes);
                var pngOriginal = ImageHelper.ConvertToPng(ImageHelper.GetBytes(metafileOriginal));
                var pngResized = (Image)null;

                if (obj.width > 0 && obj.height > 0)
                {
                    pngResized = ImageHelper.ResizeImage(pngOriginal, obj.width, obj.height);
                }

                var pngBase64 = ImageHelper.GetBase64(pngResized != null ? pngResized : pngOriginal);

                return Ok(new Success(pngBase64));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ex.StackTrace);

                try
                {
                    var img = ResourceHelper.GetEmbeddedImage<MetafileConverterController>("Error.png");

                    if (obj.width > 0 && obj.height > 0)
                    {
                        var minSize = Math.Min(obj.width, obj.height);

                        img = ImageHelper.ResizeImage(img, minSize, minSize);
                    }

                    var errorBase64 = ImageHelper.GetBase64(img);

                    return Ok(new Success(errorBase64));
                }
                catch (Exception inner)
                {
                    Console.WriteLine(inner.Message + inner.StackTrace);
                }
            }

            return Ok(new Success(""));
        }
    }

    public class ConvertObject
    {
        public string wmfBase64 { get; set; }
        public int width { get; set; }
        public int height { get; set; }
    }
}
