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
            Bind<BoilerMVCEntities>().ToSelf();
            Bind<IUnitOfWork>().ToMethod(item => item.Kernel.Get<BoilerMVCEntities>());
            Bind<DbContext>().ToMethod(item => item.Kernel.Get<BoilerMVCEntities>());
            Bind(typeof(IRepository<>)).To(typeof(EntityRepository<>));
        }
    }
}