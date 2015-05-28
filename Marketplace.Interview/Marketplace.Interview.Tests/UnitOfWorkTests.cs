using Marketplace.Interview.Business.Basket;
using Marketplace.Interview.Business.Core.UnitOfWork;
using Marketplace.Interview.Business.Shipping;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketplace.Interview.Tests
{
    [TestFixture]
    public class UnitOfWorkTests
    {
        [SetUp]
        public void TestStart()
        { }

        [TearDown]
        public void TestEnd()
        {
            string file = Path.Combine(Environment.GetEnvironmentVariable("temp"), "basket.xml");
            if (File.Exists(file))
                File.Delete(file);
        }

        [Test]
        public void CommitUnitOfWorkTest()
        {
            var unitOfWork = UnitOfWorkFactory.Create();
            var readBasketCommand = new GetBasketQuery(unitOfWork);
            var oldBasket = readBasketCommand.Invoke(new BasketRequest());
            var addBasketCommand = new Marketplace.Interview.Business.Basket.AddToBasketCommand(unitOfWork);

            var newLineItem = new LineItem()
            {
                Amount = 1,
                DeliveryRegion = RegionShippingCost.Regions.UK,
                ProductId = "111111",
                SupplierId = 1,
                Shipping = new FlatRateShipping() { FlatRate = 1.5m }
            };

            addBasketCommand.Invoke(new AddToBasketRequest()
            {
                LineItem = newLineItem
            });

            unitOfWork.Commit();
            var newBasket = readBasketCommand.Invoke(new BasketRequest());
            Assert.That(oldBasket.LineItems.Count < newBasket.LineItems.Count);
            
            var persistItem = newBasket.LineItems.Last();
            Assert.That(newLineItem.ProductId == persistItem.ProductId);
        }

        [Test]
        public void RollbackUnitOfWorkTest()
        {
            var unitOfWork = UnitOfWorkFactory.Create();
            var readBasketCommand = new GetBasketQuery(unitOfWork);
            var oldBasket = readBasketCommand.Invoke(new BasketRequest());
            var addBasketCommand = new Marketplace.Interview.Business.Basket.AddToBasketCommand(unitOfWork);

            var newLineItem = new LineItem()
            {
                Amount = 1,
                DeliveryRegion = RegionShippingCost.Regions.UK,
                ProductId = "222222",
                SupplierId = 1,
                Shipping = new FlatRateShipping() { FlatRate = 1.5m }
            };
            addBasketCommand.Invoke(new AddToBasketRequest()
            {
                LineItem = newLineItem
            });

            unitOfWork.Rollback();

            var newBasket = readBasketCommand.Invoke(new BasketRequest());
            Assert.AreEqual(oldBasket.LineItems.Count, newBasket.LineItems.Count);
            
            var saved = oldBasket.LineItems.Any(x => x.ProductId == newLineItem.ProductId);
            Assert.That(saved == false);
        }
    }
}
