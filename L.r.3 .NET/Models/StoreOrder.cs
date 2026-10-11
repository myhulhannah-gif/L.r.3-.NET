using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L.r._3_.NET.Models
{
    public class StoreOrder : Order
    {

        private string customerName = "";

        public string CustomerName
        {
            get { return customerName; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Ім'я клієнта не може бути порожнім.", nameof(value));
                }

                customerName = value.Trim();
            }
        }

        public StoreOrder(int number, string customerName) : base(number)
        {
            CustomerName = customerName;
        }

        public void AddToReceipt(string itemName, double price)
        {
            AddItem(itemName, price);
        }

        public override string GetInfo()
        {
            var info = new StringBuilder();
            info.AppendLine($"Замовлення в магазині №{Number}");
            info.AppendLine($"Дата: {Date.ToShortDateString()}");
            info.AppendLine($"Клієнт: {CustomerName}");
            info.AppendLine("Чек:");
            foreach (string line in ItemLines)
            {
                info.AppendLine($"  {line}");
            }
            info.AppendLine($"Разом: {GetTotalCost():C}");
            return info.ToString();
        }
    }
}
