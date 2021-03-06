﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Routing;

using ABC.NetCore.Models;
using ABC.NetCore.Infrastructure;

namespace ABC.NetCore.Filters
{
    public class LinkRewriteFilter : IAsyncResultFilter
    {
        private readonly IUrlHelperFactory _urlHelperFectory;

        public LinkRewriteFilter(IUrlHelperFactory urlHelperFactory)
        {
            _urlHelperFectory = urlHelperFactory;
        }

        public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            var asObjectResult = context.Result as ObjectResult;
            bool shouldSkip = asObjectResult?.Value == null || asObjectResult?.StatusCode != (int)HttpStatusCode.OK;
            if (shouldSkip)
            {
                await next();
                return;
            }

            var rewriter = new LinkRewriter(_urlHelperFectory.GetUrlHelper(context));
            RewriteAllLinks(asObjectResult.Value, rewriter);

            await next();
        }

        public static void RewriteAllLinks(object model, LinkRewriter rewriter)
        {
            if (model == null) return;

            var allProperties = model
                .GetType().GetTypeInfo()
                .GetAllProperties().Where(p => p.CanRead)
                .ToArray();

            var linkProperties = allProperties
                .Where(p => p.CanWrite && p.PropertyType == typeof(Link));

            foreach (var linkProperty in linkProperties)
            {
                var rewritten = rewriter.Rewrite(linkProperty.GetValue(model) as Link);
                if (rewritten == null) continue;

                linkProperty.SetValue(model, rewritten);

                // Special check for handling of the hidden self property
                // (we have marked it ignored under Resource Class during serialization)
                if (linkProperty.Name == nameof(Resource.Self))
                {
                    allProperties.SingleOrDefault(p => p.Name == nameof(Resource.Href))
                        ?.SetValue(model, rewritten.Href);
                    allProperties.SingleOrDefault(p => p.Name == nameof(Resource.Method))
                        ?.SetValue(model, rewritten.Method);
                    allProperties.SingleOrDefault(p => p.Name == nameof(Resource.Relations))
                        ?.SetValue(model, rewritten.Relations);
                }
            }

            var arrayProperties = allProperties.Where(p => p.PropertyType.IsArray);
            RewriteLinksInArrays(arrayProperties, model, rewriter);

            var objectProperties = allProperties.Except(linkProperties).Except(arrayProperties);
            RewriteLinksInNestedObjects(objectProperties, model, rewriter);
        }

        private static void RewriteLinksInNestedObjects(IEnumerable<PropertyInfo> objectProperties, object obj, LinkRewriter rewriter)
        {
            foreach (var objectProperty in objectProperties)
            {
                if (objectProperty.PropertyType == typeof(string))
                {
                    continue;
                }

                var typeInfo = objectProperty.PropertyType.GetTypeInfo();
                if (typeInfo.IsClass)
                {
                    RewriteAllLinks(objectProperty.GetValue(obj), rewriter);
                }
            }
        }

        private static void RewriteLinksInArrays(IEnumerable<PropertyInfo> arrayProperties, object obj, LinkRewriter rewriter)
        {
            foreach (var arrayProperty in arrayProperties)
            {
                var array = arrayProperty.GetValue(obj) as Array ?? new Array[0];

                foreach (var element in array)
                {
                    RewriteAllLinks(element, rewriter);
                }
            }
        }
    }
}
