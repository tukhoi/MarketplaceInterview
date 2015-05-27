using System.Collections.Generic;
using NUnit.Framework;
using Marketplace.Interview.Business.Basket;
using Marketplace.Interview.Business.Shipping;

namespace Marketplace.Interview.Tests
{
    [TestFixture]
    public class ShippingOptionTests
    {
        [Test]
        public void FlatRateShippingOptionTest()
        {
            var flatRateShippingOption = new FlatRateShipping {FlatRate = 1.5m};
            var shippingAmount = flatRateShippingOption.GetAmount(new LineItem(), new Basket());

            Assert.That(shippingAmount, Is.EqualTo(1.5m), "Flat rate shipping not correct.");
        }

        [Test]
        public void PerRegionShippingOptionTest()
        {
            var perRegionShippingOption = new PerRegionShipping()
                                              {
                                                  PerRegionCosts = new[]
                                                                       {
                                                                           new RegionShippingCost()
                                                                               {
                                                                                   DestinationRegion =
                                                                                       RegionShippingCost.Regions.UK,
                                                                                   Amount = .75m
                                                                               },
                                                                           new RegionShippingCost()
                                                                               {
                                                                                   DestinationRegion =
                                                                                       RegionShippingCost.Regions.Europe,
                                                                                   Amount = 1.5m
                                                                               }
                                                                       },
                                              };

            var shippingAmount = perRegionShippingOption.GetAmount(new LineItem() {DeliveryRegion = RegionShippingCost.Regions.Europe}, new Basket());
            Assert.That(shippingAmount, Is.EqualTo(1.5m));

            shippingAmount = perRegionShippingOption.GetAmount(new LineItem() { DeliveryRegion = RegionShippingCost.Regions.UK}, new Basket());
            Assert.That(shippingAmount, Is.EqualTo(.75m));
        }

        [Test]
        public void BasketShippingTotalTest()
        {
            var perRegionShippingOption = new PerRegionShipping()
            {
                PerRegionCosts = new[]
                                                                       {
                                                                           new RegionShippingCost()
                                                                               {
                                                                                   DestinationRegion =
                                                                                       RegionShippingCost.Regions.UK,
                                                                                   Amount = .75m
                                                                               },
                                                                           new RegionShippingCost()
                                                                               {
                                                                                   DestinationRegion =
                                                                                       RegionShippingCost.Regions.Europe,
                                                                                   Amount = 1.5m
                                                                               }
                                                                       },
            };

            var flatRateShippingOption = new FlatRateShipping {FlatRate = 1.1m};

            var basket = new Basket()
                             {
                                 LineItems = new List<LineItem>
                                                 {
                                                     new LineItem()
                                                         {
                                                             DeliveryRegion = RegionShippingCost.Regions.UK,
                                                             Shipping = perRegionShippingOption
                                                         },
                                                     new LineItem()
                                                         {
                                                             DeliveryRegion = RegionShippingCost.Regions.Europe,
                                                             Shipping = perRegionShippingOption
                                                         },
                                                     new LineItem() {Shipping = flatRateShippingOption},
                                                 }
                             };

            var calculator = new ShippingCalculator();

            decimal basketShipping = calculator.CalculateShipping(basket);

            Assert.That(basketShipping, Is.EqualTo(3.35m));
        }

        [Test]
        public void CustomPerRegionShippingOptionTest()
        {
            var customPerRegionShippingOption = new CustomPerRegionShipping()
            {
                PerRegionCosts = new[]
                                                                       {
                                                                           new CustomRegionShippingCost()
                                                                               {
                                                                                   DestinationRegion =
                                                                                       RegionShippingCost.Regions.UK,
                                                                                   Amount = .75m,
                                                                                   DeductedAmount = .05m
                                                                               },
                                                                           new CustomRegionShippingCost()
                                                                               {
                                                                                   DestinationRegion =
                                                                                       RegionShippingCost.Regions.Europe,
                                                                                   Amount = 1.5m,
                                                                                   DeductedAmount = .05m
                                                                               }
                                                                       },
            };

            var shippingAmount = customPerRegionShippingOption.GetAmount(new LineItem() { DeliveryRegion = RegionShippingCost.Regions.Europe }, new Basket());
            Assert.That(shippingAmount, Is.EqualTo(1.5m));

            shippingAmount = customPerRegionShippingOption.GetAmount(new LineItem() { DeliveryRegion = RegionShippingCost.Regions.UK }, new Basket());
            Assert.That(shippingAmount, Is.EqualTo(.75m));
        }

        [Test]
        public void BasketCustomShippingTotalTest()
        {
            var customPerRegionShippingOption = new CustomPerRegionShipping()
            {
                PerRegionCosts = new[]
                    {
                        new CustomRegionShippingCost()
                            {
                                DestinationRegion =
                                    RegionShippingCost.Regions.UK,
                                Amount = .75m,
                                DeductedAmount = .05m
                            },
                        new CustomRegionShippingCost()
                            {
                                DestinationRegion =
                                    RegionShippingCost.Regions.Europe,
                                Amount = 1.5m,
                                DeductedAmount = .05m
                            }
                    },
            };

            var flatRateShippingOption = new FlatRateShipping { FlatRate = 1.1m };

            var basket = new Basket()
            {
                LineItems = new List<LineItem>
                    {
                        new LineItem()
                            {
                                DeliveryRegion = RegionShippingCost.Regions.UK,
                                SupplierId = 1,
                                Shipping = customPerRegionShippingOption
                            },
                        new LineItem()
                            {
                                DeliveryRegion = RegionShippingCost.Regions.Europe,
                                SupplierId = 1,
                                Shipping = customPerRegionShippingOption
                            },
                        new LineItem() {Shipping = flatRateShippingOption},
                        new LineItem()
                            {
                                DeliveryRegion = RegionShippingCost.Regions.UK,
                                SupplierId = 1,
                                Shipping = customPerRegionShippingOption
                            },
                            new LineItem()
                            {
                                DeliveryRegion = RegionShippingCost.Regions.UK,
                                SupplierId = 2,
                                Shipping = customPerRegionShippingOption
                            },
                    }
            };

            var calculator = new ShippingCalculator();

            decimal basketShipping = calculator.CalculateShipping(basket);

            Assert.AreEqual(0.7m, basket.LineItems[0].ShippingAmount);
            Assert.AreEqual(1.5m, basket.LineItems[1].ShippingAmount);
            Assert.AreEqual(1.1m, basket.LineItems[2].ShippingAmount);
            Assert.AreEqual(0.7m, basket.LineItems[3].ShippingAmount);
            Assert.AreEqual(0.75m, basket.LineItems[4].ShippingAmount);
            Assert.That(basketShipping, Is.EqualTo(4.75m));
        }

    }
}