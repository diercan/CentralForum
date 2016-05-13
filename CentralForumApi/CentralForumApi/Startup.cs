using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin;
using Owin;
using Models.Models;
using CentralForumApi.Controllers;

[assembly: OwinStartup(typeof(CentralForumApi.Startup))]

namespace CentralForumApi
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
        }
    }
}
