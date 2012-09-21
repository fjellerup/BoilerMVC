using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BoilerMVC.Common;

namespace BoilerMVC.Services
{
    public class BaseService
    {
        protected IUnitOfWork _unitOfWOrk;

        public BaseService(IUnitOfWork unitOfWork)
        {
            _unitOfWOrk = unitOfWork;
        }
    }
}