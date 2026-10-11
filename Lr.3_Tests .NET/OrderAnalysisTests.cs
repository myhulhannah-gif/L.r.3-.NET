using L.r._3_.NET.Models;
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

        // Усі замовлення: інтернет + магазин
        [TestMethod]
        public void GetAllOrdersTotal_MixedOrders_ReturnsSumOfAll()
        {
            var online = new OnlineOrder(1, "Нова пошта");
            online.AddItem("Хліб", 200);                 // 200 + 70 = 270

            var courier = new OnlineOrder(2, "Кур'єр");
            courier.AddItem("Масло", 100);                // 100 + 100 = 200

            var store = new StoreOrder(3, "Іван");
            store.AddToReceipt("Хліб", 30);      // 30

            var orders = new List<Order> { online, courier, store };

            double result = OrderAnalysis.GetAllOrdersTotal(orders);

            Assert.AreEqual(500, result, 0.001);
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
