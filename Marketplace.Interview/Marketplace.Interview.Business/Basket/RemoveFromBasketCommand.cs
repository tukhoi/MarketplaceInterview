using Marketplace.Interview.Business.Core.UnitOfWork;
namespace Marketplace.Interview.Business.Basket
{
    public class RemoveFromBasketCommand : BasketOperationBase, IRemoveFromBasketCommand
    {
        public RemoveFromBasketCommand(IUnitOfWork unitOfWork) : base(unitOfWork) { }

        public bool Invoke(int id)
        {
            var basket = GetBasket();

            basket.LineItems.RemoveWhere(li => li.Id == id);

            SaveBasket(basket);

            return true;
        }
    }
}