using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using BoilerMVC.Common;
using BoilerMVC.Data;
using Ninject;
using Ninject.Modules;

namespace BoilerMVC.Framework
{
    public class WebModule : NinjectModule
    {
        public override void Load()
        {
            IUnitOfWork shopContext = new BoilerMVCEntities();
            Kernel.Bind(typeof(IUnitOfWork)).ToConstant(shopContext);
            Kernel.Bind(typeof(IRepository<>)).To(typeof(EntityRepository<>));
        }
    }
}