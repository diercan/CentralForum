using DataLayer;
using Models.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace CentralForumApi.Controllers
{
    public class BaseApiController : ApiController
    {
        protected readonly ApplicationDbContext dbContext;
        protected readonly DalUnitOfWork dalUnitOfWork;

        public BaseApiController()
        {
            dbContext = new ApplicationDbContext();
            dalUnitOfWork = new DalUnitOfWork(dbContext);
        }
    }
}