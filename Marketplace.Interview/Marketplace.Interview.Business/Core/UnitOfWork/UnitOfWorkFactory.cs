using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketplace.Interview.Business.Core.UnitOfWork
{
    public class UnitOfWorkFactory
    {
        public static IUnitOfWork Create()
        {
            return new BasketUnitOfWork();
        }
    }
}
