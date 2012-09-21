using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BoilerMVC.Common
{
    public interface IUnitOfWork
    {
        void Commit();
    }
}