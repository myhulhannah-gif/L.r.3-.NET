using L.r._3_4_.NET.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderTests
{
    [TestClass]
    public class StoreOrderTests
    {
        // Успішне створення об'єкта
        [TestMethod]
        public void Constructor_WithValidData_InitializesCorrectly()
        {
            var order = new StoreOrder(1, "Іван");

            Assert.AreEqual(1, order.Number);
            Assert.AreEqual("Іван", order.CustomerName);
            Assert.AreEqual(0, order.TotalAmount);
            Assert.AreEqual(DateTime.Now.Date, order.Date.Date, "Дата має бути поточною.");
        }

        // Некоректний номер
        [TestMethod]
        public void Constructor_WithNegativeNumber_ThrowsArgumentException()
        {
            Assert.ThrowsException<ArgumentException>(() => new StoreOrder(-1, "Іван"));
        }

        // Порожнє ім'я клієнта при створенні
        [TestMethod]
        public void Constructor_WithEmptyCustomerName_ThrowsArgumentException()
        {
            Assert.ThrowsException<ArgumentException>(() => new StoreOrder(1, ""));
        }

        // Додавання до чека збільшує суму
        [TestMethod]
        public void AddToReceipt_ValidItems_IncreasesTotalAmount()
        {
            var order = new StoreOrder(1, "Іван");

            order.AddToReceipt("Хліб", 25);
            order.AddToReceipt("Молоко", 40);

            Assert.AreEqual(65, order.TotalAmount, 0.001);
        }

        // Порожня назва товару
        [TestMethod]
        public void AddToReceipt_EmptyItemName_ThrowsArgumentException()
        {
            var order = new StoreOrder(1, "Іван");

            Assert.ThrowsException<ArgumentException>(() => order.AddToReceipt("", 25));
        }

        // Некоректна ціна
        [TestMethod]
        public void AddToReceipt_NegativePrice_ThrowsArgumentException()
        {
            var order = new StoreOrder(1, "Іван");

            Assert.ThrowsException<ArgumentException>(() => order.AddToReceipt("Хліб", -5));
        }

        // Якщо правильний телефон, картка лояльності є
        [TestMethod]
        public void LoyaltyPhone_ValidNumber_SetsCard()
        {
            var order = new StoreOrder(1, "Іван");
            order.LoyaltyPhone = "0961234567";
            Assert.IsTrue(order.HasLoyaltyCard);
        }

        // Якщо неправильний телефон, то помилка
        [TestMethod]
        public void LoyaltyPhone_InvalidFormat()
        {
            var order = new StoreOrder(1, "Іван");
            Assert.ThrowsException<ArgumentException>(() => order.LoyaltyPhone = "12345");
        }

        // Без картки знижки немає
        [TestMethod]
        public void GetDiscountAmount_WithoutCard_ReturnsZero()
        {
            var order = new StoreOrder(1, "Іван");
            order.AddToReceipt("Хліб", 100);
            Assert.AreEqual(0, order.GetDiscountAmount(), 0.001);
        }

        // З карткою знижка 3%
        [TestMethod]
        public void GetDiscountAmount_WithCard_Returns3Percent()
        {
            var order = new StoreOrder(1, "Іван");
            order.LoyaltyPhone = "0961234567";
            order.AddToReceipt("Хліб", 100);
            Assert.AreEqual(3, order.GetDiscountAmount(), 0.001);
        }

        // Тестування методу GetInfo
        [TestMethod]
        public void GetInfo_ReturnsCorrectInfoString()
        {
            var order = new StoreOrder(3, "Іван");
            order.LoyaltyPhone = "0961234567";
            order.AddToReceipt("Хліб", 40);
            order.AddToReceipt("Молоко", 60);

            string info = order.GetInfo();

            StringAssert.Contains(info, "Замовлення в магазині №3");
            StringAssert.Contains(info, $"Дата: {DateTime.Now.ToShortDateString()}");
            StringAssert.Contains(info, "Клієнт: Іван");
            StringAssert.Contains(info, "Номер телефона для знижки: 0961234567");
            StringAssert.Contains(info, "Чек:");
            StringAssert.Contains(info, $"Хліб: {40.0:C}");
            StringAssert.Contains(info, $"Молоко: {60.0:C}");
            StringAssert.Contains(info, $"Знижка: -{3:C}");
            StringAssert.Contains(info, $"Разом: {97.0:C}");
        }
    }
}
