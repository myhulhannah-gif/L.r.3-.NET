using L.r._3_4_.NET.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderTests
{
    [TestClass]
    public class OrderAnalysisTests
    {
        // Інтернет-замовлення: магазинні не враховуються
        [TestMethod]
        public void GetTotalCostWithDelivery_MixedOrders_CountsOnlyOnlineOrders()
        {
            var online = new OnlineOrder(1, "Нова пошта");
            online.AddItem("Хліб", 200);                 // 200 + 70 = 270

            var courier = new OnlineOrder(2, "Кур'єр");
            courier.AddItem("Масло", 100);                // 100 + 100 = 200

            var store = new StoreOrder(3, "Іван");
            store.AddToReceipt("Хліб", 30);      // не враховується

            var orders = new List<Order> { online, courier, store };

            double result = OrderAnalysis.GetTotalCostWithDelivery(orders);

            Assert.AreEqual(470, result, 0.001);
        }

        // Замовлення в магазині: інтернет-замовлення не враховуються
        [TestMethod]
        public void GetStoreTotal_MixedOrders_CountsOnlyStoreOrders()
        {
            var online = new OnlineOrder(1, "Кур'єр");
            online.AddItem("Масло", 100);                 // не враховується

            var store = new StoreOrder(2, "Іван");
            store.AddToReceipt("Хліб", 25);
            store.AddToReceipt("Молоко", 40);    // 25 + 40 = 65

            var orders = new List<Order> { online, store };

            double result = OrderAnalysis.GetStoreTotal(orders);

            Assert.AreEqual(65, result, 0.001);
        }

        // Корпоративні замовлення: інші не враховуються
        [TestMethod]
        public void GetCorporateTotal_MixedOrders_CountsOnlyCorporate()
        {
            var corp = new CorporateOrder(1, "ТОВ Ромашка", "12345678");
            corp.AddItem("Принтер", 1000);                  // 1000 - 100 + 200 = 1100

            var online = new OnlineOrder(2, "Кур'єр");
            online.AddItem("Масло", 100); // не враховується

            var orders = new List<Order> { corp, online };

            Assert.AreEqual(1100, OrderAnalysis.GetCorporateTotal(orders), 0.001);
        }

        // Загальна сума знижок для трьох класів через один інтерфейс
        [TestMethod]
        public void GetTotalDiscount_ThroughInterface_SumsDiscountsOfAllClasses()
        {
            var online = new OnlineOrder(1, "Самовивіз");
            online.AddItem("Товар", 1000);                  // 5% = 50

            var corp = new CorporateOrder(2, "ТОВ Ромашка", "12345678");
            corp.AddItem("Папір", 200);                     // 10% = 20

            var store = new StoreOrder(3, "Іван");
            store.LoyaltyPhone = "0961234567";
            store.AddToReceipt("Хліб", 100);                // 3% = 3

            var items = new List<IDiscountable> { online, corp, store };

            Assert.AreEqual(73, OrderAnalysis.GetTotalDiscount(items), 0.001);
        }

        // Усі замовлення: інтернет + магазин + корпоративні
        [TestMethod]
        public void GetAllOrdersTotal_MixedOrders_ReturnsSumOfAll()
        {
            var online = new OnlineOrder(1, "Нова пошта");
            online.AddItem("Хліб", 200);                 // 200 + 70 = 270

            var store = new StoreOrder(3, "Іван");
            store.AddToReceipt("Хліб", 30);      // 30

            var corp = new CorporateOrder(2, "ТОВ Ромашка", "12345678");
            corp.AddItem("Папір", 200);                     // 200 - 20 + 200 = 380

            var orders = new List<Order> { online, corp, store };

            double result = OrderAnalysis.GetAllOrdersTotal(orders);

            Assert.AreEqual(680, result, 0.001);
        }

        // Порожній список
        [TestMethod]
        public void AllMethods_EmptyList_ReturnZero()
        {
            var orders = new List<Order>();

            Assert.AreEqual(0, OrderAnalysis.GetTotalCostWithDelivery(orders), 0.001);
            Assert.AreEqual(0, OrderAnalysis.GetStoreTotal(orders), 0.001);
            Assert.AreEqual(0, OrderAnalysis.GetAllOrdersTotal(orders), 0.001);
        }
    }
}
