using L.r._3_4_.NET.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderTests
{
    [TestClass]
    public class OnlineOrderTests
    {
        // Успішне створення об'єкта
        [TestMethod]
        public void Constructor_WithValidData_InitializesCorrectly()
        {
            var order = new OnlineOrder(1, "Кур'єр");

            Assert.AreEqual(1, order.Number);
            Assert.AreEqual("Кур'єр", order.DeliveryMethod);
            Assert.AreEqual(0, order.TotalAmount);
            Assert.AreEqual(DateTime.Now.Date, order.Date.Date, "Дата має бути поточною.");
        }

        // Некоректний номер
        [TestMethod]
        public void Constructor_WithNegativeNumber_ThrowsArgumentException()
        {
            Assert.ThrowsException<ArgumentException>(() => new OnlineOrder(-1, "Кур'єр"));
        }

        // Невідомий спосіб доставки
        [TestMethod]
        public void Constructor_WithUnknownDeliveryMethod_ThrowsArgumentException()
        {
            Assert.ThrowsException<ArgumentException>(() => new OnlineOrder(1, "Літак"));
        }

        // Порожній спосіб доставки при створенні
        [TestMethod]
        public void Constructor_WithEmptyDeliveryMethod_ThrowsArgumentException()
        {
            Assert.ThrowsException<ArgumentException>(() => new OnlineOrder(1, ""));
        }

        // Порожня назва товару
        [TestMethod]
        public void AddItem_EmptyName_ThrowsArgumentException()
        {
            var order = new OnlineOrder(1, "Кур'єр");

            Assert.ThrowsException<ArgumentException>(() => order.AddItem("", 500));
        }

        // Додавання товару збільшує суму товарів
        [TestMethod]
        public void AddItem_ValidPrices_IncreasesTotalAmount()
        {
            var order = new OnlineOrder(1, "Кур'єр");

            order.AddItem("Хліб", 200);
            order.AddItem("Сметана", 300);

            Assert.AreEqual(500, order.TotalAmount, 0.001);
        }

        // Некоректна ціна товару
        [TestMethod]
        public void AddItem_NegativePrice_ThrowsArgumentException()
        {
            var order = new OnlineOrder(1, "Кур'єр");

            Assert.ThrowsException<ArgumentException>(() => order.AddItem("Сіль", -50));
        }

        // Вартість доставки для кожного способу
        [TestMethod]
        public void CalculateDeliveryCost_Courier_Returns100()
        {
            var order = new OnlineOrder(1, "Кур'єр");

            Assert.AreEqual(100, order.CalculateDeliveryCost(), 0.001);
        }

        [TestMethod]
        public void CalculateDeliveryCost_NovaPoshta_Returns70()
        {
            var order = new OnlineOrder(1, "Нова пошта");

            Assert.AreEqual(70, order.CalculateDeliveryCost(), 0.001);
        }

        [TestMethod]
        public void CalculateDeliveryCost_Pickup_ReturnsZero()
        {
            var order = new OnlineOrder(1, "Самовивіз");

            Assert.AreEqual(0, order.CalculateDeliveryCost(), 0.001);
        }

        // Сума товарів менше 1000: знижки немає
        [TestMethod]
        public void GetDiscountAmount_Below1000_ReturnsZero()
        {
            var order = new OnlineOrder(1, "Самовивіз");
            order.AddItem("Товар", 999);

            Assert.AreEqual(0, order.GetDiscountAmount(), 0.001);
        }

        // Сума товарів рівно 1000: знижка 5%
        [TestMethod]
        public void GetDiscountAmount_From1000_Returns5Percent()
        {
            var order = new OnlineOrder(1, "Самовивіз");
            order.AddItem("Товар", 1000);

            Assert.AreEqual(50, order.GetDiscountAmount(), 0.001);
        }

        // Сума товарів рівно 3000: знижка 10%
        [TestMethod]
        public void GetDiscountAmount_From3000_Returns10Percent()
        {
            var order = new OnlineOrder(1, "Самовивіз");
            order.AddItem("Товар", 3000);

            Assert.AreEqual(300, order.GetDiscountAmount(), 0.001);
        }

        // Підсумкова вартість без знижки: товари + доставка (500 + 100)
        [TestMethod]
        public void GetTotalCost_IncludesDelivery()
        {
            var order = new OnlineOrder(1, "Кур'єр");
            order.AddItem("Риба", 500);

            Assert.AreEqual(600, order.GetTotalCost(), 0.001);
        }

        // Підсумкова вартість зі знижкою: товари - знижка + доставка (2000 - 100 + 100)
        [TestMethod]
        public void GetTotalCost_WithDiscount_AppliesDiscountAndDelivery()
        {
            var order = new OnlineOrder(1, "Кур'єр");
            order.AddItem("Телевізор", 2000);

            Assert.AreEqual(2000, order.GetTotalCost(), 0.001);
        }

        // Тестування методу GetInfo (без знижки)
        [TestMethod]
        public void GetInfo_without_Discount()
        {
            var order = new OnlineOrder(7, "Нова пошта");
            order.AddItem("М'ясо", 500);

            string info = order.GetInfo();

            StringAssert.Contains(info, "Інтернет-замовлення №7");
            StringAssert.Contains(info, $"Дата: {DateTime.Now.ToShortDateString()}");
            StringAssert.Contains(info, "Спосіб доставки: Нова пошта");
            StringAssert.Contains(info, "Товари:");
            StringAssert.Contains(info, $"М'ясо: {500.0:C}");
            StringAssert.Contains(info, $"Сума товарів: {500.0:C}");
            StringAssert.Contains(info, $"Вартість доставки: {70.0:C}");
            StringAssert.Contains(info, $"Разом: {570.0:C}");
        }

        // Тестування методу GetInfo (зі знижкою)
        [TestMethod]
        public void GetInfo_with_Discount()
        {
            var order = new OnlineOrder(8, "Нова пошта");
            order.AddItem("Шафа", 4000);

            string info = order.GetInfo();

            StringAssert.Contains(info, "Інтернет-замовлення №8");
            StringAssert.Contains(info, $"Дата: {DateTime.Now.ToShortDateString()}");
            StringAssert.Contains(info, "Спосіб доставки: Нова пошта");
            StringAssert.Contains(info, "Товари:");
            StringAssert.Contains(info, $"Шафа: {4000.0:C}");
            StringAssert.Contains(info, $"Сума товарів: {4000.0:C}");
            StringAssert.Contains(info, $"Знижка: -{400.0:C}");
            StringAssert.Contains(info, $"Вартість доставки: {70.0:C}");
            StringAssert.Contains(info, $"Разом: {3670.0:C}");
        }
    }
}
