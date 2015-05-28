using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketplace.Interview.Business.Core.UnitOfWork
{
    public interface IUnitOfWork
    {
        void Commit();
        void Rollback();
        void RegisterNew(object entity);
        void RegisterDirty(object entity);
        void RegisterDeleted(object entity);
    }
}
