using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketplace.Interview.Business.Core.UnitOfWork
{
    public class BasketUnitOfWork : IUnitOfWork
    {
        private static readonly string file = Path.Combine(Environment.GetEnvironmentVariable("temp"), "basket.xml");

        private Basket.Basket _dirtyObject = null;

        #region IUnitOfWork

        public void Commit()
        {
            if (_dirtyObject != null)
            {
                SaveBasket(_dirtyObject);
                _dirtyObject = null;
            }
        }

        public void Rollback()
        {
            _dirtyObject = null;
        }

        public void RegisterNew(object entity)
        {
            throw new NotImplementedException();
        }

        public void RegisterDirty(object entity)
        {
            var basket = entity as Basket.Basket;
            if (basket == null)
                throw new InvalidDataException("Only instance of Basket accepted");
            _dirtyObject = basket;
        }

        public void RegisterDeleted(object entity)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Private

        private void SaveBasket(Basket.Basket basket)
        {
            using (var sw = new StreamWriter(file, false))
            {
                sw.Write(SerializationHelper.DataContractSerialize(basket));
            }
        }

        #endregion
    }
}
