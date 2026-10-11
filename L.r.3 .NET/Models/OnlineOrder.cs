using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L.r._3_.NET.Models
{
    public class OnlineOrder : Order
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
            else{
                return 0;  // Самовивіз
            }
        }

        public override double GetTotalCost()
        {
            return base.GetTotalCost() + CalculateDeliveryCost();
        }

        public override string GetInfo()
        {
            var info = new StringBuilder();
            info.AppendLine($"Інтернет-замовлення №{Number}");
            info.AppendLine($"Дата: {Date.ToShortDateString()}");
            info.AppendLine($"Спосіб доставки: {DeliveryMethod}");
            info.AppendLine($"Товари:");
            foreach(string line in ItemLines)
            {
                info.AppendLine($"  {line}");
            }
            info.AppendLine($"Сума товарів: {TotalAmount:C}");
            info.AppendLine($"Вартість доставки: {CalculateDeliveryCost():C}");
            info.AppendLine($"Разом: {GetTotalCost():C}");
            return info.ToString();
        }
    }
}
