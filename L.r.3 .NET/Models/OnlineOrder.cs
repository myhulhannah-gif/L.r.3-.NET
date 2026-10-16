using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L.r._3_4_.NET.Models
{
    public class OnlineOrder : Order, IDeliverable, IDiscountable
    {
        // Допустимі способи доставки
        public static readonly string[] AvailableMethods = { "Кур'єр", "Нова пошта", "Самовивіз" };

        private string deliveryMethod = "";

        public string DeliveryMethod
        {
            get { return deliveryMethod; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Спосіб доставки не може бути порожнім.", nameof(value));
                }

                if (!AvailableMethods.Contains(value))
                {
                    throw new ArgumentException("Невідомий спосіб доставки: " + value, nameof(value));
                }

                deliveryMethod = value;
            }
        }

        public OnlineOrder(int number, string deliveryMethod) : base(number)
        {
            DeliveryMethod = deliveryMethod;
        }

        // Метод розрахунку вартості доставки залежно від способу
        public double CalculateDeliveryCost()
        {
            if (DeliveryMethod == "Кур'єр")
            {
                return 100;
            }

            else if (DeliveryMethod == "Нова пошта")
            {
                return 70;
            }
            else
            {
                return 0;  // Самовивіз
            }
        }

        // Реалізація IDiscountable
        public double GetDiscountAmount()
        {
            if (TotalAmount >= 3000)
            {
                return TotalAmount * 0.10; // 10% знижка
            }
            else if (TotalAmount >= 1000)
            {
                return TotalAmount * 0.05; // 5% знижка
            }
            else
            {
                return 0; // без знижки
            }
        }

        public override double GetTotalCost()
        {
            return base.GetTotalCost() - GetDiscountAmount() + CalculateDeliveryCost();
        }

        public override string GetInfo()
        {
            var info = new StringBuilder();
            info.AppendLine($"Інтернет-замовлення №{Number}");
            info.AppendLine($"Дата: {Date.ToShortDateString()}");
            info.AppendLine($"Спосіб доставки: {DeliveryMethod}");

            info.AppendLine($"Товари:");
            foreach (string line in ItemLines)
            {
                info.AppendLine($"  {line}");
            }

            info.AppendLine($"Сума товарів: {TotalAmount:C}");

            if (GetDiscountAmount() > 0)
                info.AppendLine($"Знижка: -{GetDiscountAmount():C}");

            info.AppendLine($"Вартість доставки: {CalculateDeliveryCost():C}");
            info.AppendLine($"Разом: {GetTotalCost():C}");
            return info.ToString();
        }
    }
}
