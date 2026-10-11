using L.r._3_.NET.Models;
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

        // У магазині немає доставки: підсумок дорівнює сумі товарів
        [TestMethod]
        public void GetTotalCost_EqualsTotalAmount()
        {
            var order = new StoreOrder(1, "Іван");
            order.AddToReceipt("Хліб", 25);

            Assert.AreEqual(25, order.GetTotalCost(), 0.001);
        }

        // Тестування методу GetInfo
        [TestMethod]
        public void GetInfo_ReturnsCorrectInfoString()
        {
            var order = new StoreOrder(3, "Іван");
            order.AddToReceipt("Хліб", 25);
            order.AddToReceipt("Молоко", 40);

            string info = order.GetInfo();

            StringAssert.Contains(info, "Замовлення в магазині №3");
            StringAssert.Contains(info, $"Дата: {DateTime.Now.ToShortDateString()}");
            StringAssert.Contains(info, "Клієнт: Іван");
            StringAssert.Contains(info, "Чек:");
            StringAssert.Contains(info, $"Хліб: {25.0:C}");
            StringAssert.Contains(info, $"Молоко: {40.0:C}");
            StringAssert.Contains(info, $"Разом: {65.0:C}");
        }
    }
}
