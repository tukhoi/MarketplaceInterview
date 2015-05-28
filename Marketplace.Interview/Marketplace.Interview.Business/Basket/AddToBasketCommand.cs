﻿using Marketplace.Interview.Business.Core.UnitOfWork;
namespace Marketplace.Interview.Business.Basket
{
    public class AddToBasketCommand : BasketOperationBase, IAddToBasketCommand
    {
        public AddToBasketCommand(IUnitOfWork unitOfWork) : base(unitOfWork) {}

        public AddToBasketResponse Invoke(AddToBasketRequest request)
        {
            var basket = GetBasket();

            request.LineItem.Id = basket.LineItems.MaxOrDefault(li => li.Id) + 1;

            basket.LineItems.Add(request.LineItem);

            SaveBasket(basket);

            return new AddToBasketResponse(){LineItemCount = basket.LineItems.Count};
        }
    }

    public class AddToBasketRequest
    {
        public LineItem LineItem { get; set; }
    }

    public class AddToBasketResponse
    {
        public int LineItemCount { get; set; }
    }
}