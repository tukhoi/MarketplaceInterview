using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketplace.Interview.Business.Shipping
{
    public class CustomRegionShippingCost : RegionShippingCost
    {
        public decimal DeductedAmount { get; set; }
    }
}
