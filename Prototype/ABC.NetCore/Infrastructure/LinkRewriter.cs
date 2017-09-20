using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.AspNetCore.Mvc;
using ABC.NetCore.Models;

namespace ABC.NetCore.Infrastructure
{
    public class LinkRewriter
    {
        private readonly IUrlHelper _urlhelper;

        public LinkRewriter(IUrlHelper urlHelper)
        {
            _urlhelper = urlHelper;
        }

        public Link Rewrite(Link original)
        {
            if (original == null) return null;

            return new Link
            {
                Href = _urlhelper.Link(original.RouteName, original.RouteValues),
                Method = original.Method,
                Relations = original.Relations
            };
        }
    }
}
