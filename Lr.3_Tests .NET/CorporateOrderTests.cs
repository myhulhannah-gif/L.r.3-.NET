using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using L.r._3_4_.NET.Models;

namespace OrderTests
{
    [TestClass]
    public class CorporateOrderTests
    {
        // Успішне створення об'єкта
        [TestMethod]
        public void Constructor_WithValidData_InitializesCorrectly()
        {
            var order = new CorporateOrder(1, "ТОВ Ромашка", "12345678");

            Assert.AreEqual(1, order.Number);
            Assert.AreEqual("ТОВ Ромашка", order.CompanyName);
            Assert.AreEqual("12345678", order.EDRPOU);
            Assert.AreEqual(0, order.TotalAmount);
        }

        // Порожня назва компанії
        [TestMethod]
        public void Constructor_WithEmptyCompany_ThrowsArgumentException()
        {
            Assert.ThrowsException<ArgumentException>(() => new CorporateOrder(1, "", "12345678"));
        }

        // Некоректний ЄДРПОУ (не 8 цифр)
        [TestMethod]
        public void Constructor_WithInvalidEdrpou_ThrowsArgumentException()
        {
            Assert.ThrowsException<ArgumentException>(() => new CorporateOrder(1, "ТОВ Ромашка", "123"));
        }

        // Вартість замовлення до 5000 - доставка 200 грн
        [TestMethod]
        public void CalculateDeliveryCost_Below5000AfterDiscount_Returns200()
        {
            var order = new CorporateOrder(1, "ТОВ Ромашка", "12345678");
            order.AddItem("Сервер", 5000); // знижка -500

            Assert.AreEqual(200, order.CalculateDeliveryCost(), 0.001);
        }

        // Вартість замовлення > 5000 - доставка безкоштовна
        [TestMethod]
        public void CalculateDeliveryCost_From5000AfterDiscount_ReturnsZero()
        {
            var order = new CorporateOrder(1, "ТОВ Ромашка", "12345678");
            order.AddItem("Сервер", 6000); // знижка -600

            Assert.AreEqual(0, order.CalculateDeliveryCost(), 0.001);
        }

        // Знижка завжди 10%
        [TestMethod]
        public void GetDiscountAmount_Returns10Percent()
        {
            var order = new CorporateOrder(1, "ТОВ Ромашка", "12345678");
            order.AddItem("Папір", 100);

            Assert.AreEqual(10, order.GetDiscountAmount(), 0.001);
        }

        // Підсумок: товари - знижка + доставка (1000 - 100 + 200)
        [TestMethod]
        public void GetTotalCost_AppliesDiscountAndDelivery()
        {
            var order = new CorporateOrder(1, "ТОВ Ромашка", "12345678");
            order.AddItem("Принтер", 1000);

            Assert.AreEqual(1100, order.GetTotalCost(), 0.001);
        }

        // Перевірка тексту GetInfo
        [TestMethod]
        public void GetInfo_ReturnsCorrectInfoString()
        {
            var order = new CorporateOrder(5, "ТОВ Ромашка", "12345678");
            order.AddItem("Принтер", 1000);

            string info = order.GetInfo();

            StringAssert.Contains(info, "Корпоративне замовлення №5");
            StringAssert.Contains(info, $"Дата: {DateTime.Now.ToShortDateString()}");
            StringAssert.Contains(info, "Компанія: ТОВ Ромашка");
            StringAssert.Contains(info, "ЄДРПОУ: 12345678");
            StringAssert.Contains(info, "Спосіб доставки: Корпоративна кур'єрська доставка");
            StringAssert.Contains(info, "Товари:");
            StringAssert.Contains(info, $"Принтер: {1000.0:C}");
            StringAssert.Contains(info, $"Сума товарів: {1000.0:C}");
            StringAssert.Contains(info, $"Знижка: -{100.0:C}");
            StringAssert.Contains(info, $"Вартість доставки: {200.0:C}");
            StringAssert.Contains(info, $"Разом: {1100.0:C}");
        }
    }
}
