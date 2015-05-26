using Marketplace.Interview.Business.Basket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketplace.Interview.Business.Shipping
{
    public class CustomPerRegionShipping : ShippingBase
    {
        public IEnumerable<CustomRegionShippingCost> PerRegionCosts { get; set; }

        //public decimal Deducted

        public override string GetDescription(LineItem lineItem, Basket.Basket basket)
        {
            return string.Format("Shipping to {0}", lineItem.DeliveryRegion);
        }

        public override decimal GetAmount(LineItem lineItem, Basket.Basket basket)
        {
            var shippingCost = PerRegionCosts.SingleOrDefault(prc => prc.DestinationRegion == lineItem.DeliveryRegion);
            if (shippingCost == null) throw new ApplicationException("No defined DeliveryRegion found");

            if (basket.LineItems == null || basket.LineItems.Count == 0)
                return shippingCost.Amount;

            var hasSameShipping = basket.LineItems.Any(li => 
                li != lineItem
                && li.Shipping.GetType().Equals(lineItem.Shipping.GetType())
                && li.DeliveryRegion == lineItem.DeliveryRegion
                && li.SupplierId == lineItem.SupplierId);

            return hasSameShipping ? shippingCost.Amount - shippingCost.DeductedAmount : shippingCost.Amount;
        }
    }
}
