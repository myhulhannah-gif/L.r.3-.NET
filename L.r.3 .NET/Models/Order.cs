using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace L.r._3_4_.NET.Models
{
    public abstract class Order : IDescribable
    {
        public int Number { get; }
        public DateTime Date { get; }
        
        public double TotalAmount { get; private set; } // Сума товарів

        private readonly List<string> _itemLines = new List<string>(); // Рядки товарів

        public IReadOnlyList<string> ItemLines
        {
            get { return _itemLines; }
        }

        public Order(int number)
        {
            if (number <= 0)
            {
                throw new ArgumentException("Номер замовлення має бути додатним числом.", nameof(number));
            }

            Number = number;
            Date = DateTime.Now;
            TotalAmount = 0;
        }

        public void AddItem(string name, double price)
        {
            if (price <= 0)
            {
                throw new ArgumentException("Ціна товару має бути більшою за нуль.", nameof(price));
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Назва товару не може бути порожньою.", nameof(name));
            }

            TotalAmount += price;
            _itemLines.Add($"{name.Trim()}: {price:C}");
        }

        // Віртуальний метод: Підсумкова вартість замовлення
        public virtual double GetTotalCost()
        {
            return TotalAmount;
        }

        // Абстрактний метод: текстовий опис замовлення
        public abstract string GetInfo();
    }
}
