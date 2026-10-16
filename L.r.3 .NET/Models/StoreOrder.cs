using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L.r._3_4_.NET.Models
{
    public class StoreOrder : Order, IDiscountable
    {

        private string customerName = "";
        private string loyaltyPhone = "";
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

        public string LoyaltyPhone
        {
            get { return loyaltyPhone; }
            set
            {
                string phone;

                if(value == null) phone = "";
                else phone = value.Trim();

                if(phone != "" && !IsValidPhone(phone))
                {
                    throw new ArgumentException("Телефон повинен містити 10 цифр, наприклад 0961234567.", nameof(value));
                }
                loyaltyPhone = phone;
            }
        }

        public bool HasLoyaltyCard
        {
            get
            {
                if (loyaltyPhone != "") return true;
                else return false;
            }
        } 

        private bool IsValidPhone(string phone)
        {
            return phone.Length == 10 && phone.All(char.IsDigit);
        }

        public StoreOrder(int number, string customerName) : base(number)
        {
            CustomerName = customerName;
        }

        public void AddToReceipt(string itemName, double price)
        {
            AddItem(itemName, price);
        }

        public double GetDiscountAmount()
        {
            if (HasLoyaltyCard)
            {
                return TotalAmount * 0.03; // 3% знижки
            }
            return 0;
        }

        public override double GetTotalCost()
        {
            return base.GetTotalCost() - GetDiscountAmount();
        }

        public override string GetInfo()
        {
            var info = new StringBuilder();
            info.AppendLine($"Замовлення в магазині №{Number}");
            info.AppendLine($"Дата: {Date.ToShortDateString()}");
            info.AppendLine($"Клієнт: {CustomerName}");

            if(HasLoyaltyCard)
                info.AppendLine($"Номер телефона для знижки: {LoyaltyPhone}");

            info.AppendLine("Чек:");
            foreach (string line in ItemLines)
            {
                info.AppendLine($"  {line}");
            }

            if (HasLoyaltyCard)
                info.AppendLine($"Знижка: -{GetDiscountAmount():C}");

            info.AppendLine($"Разом: {GetTotalCost():C}");
            return info.ToString();
        }
    }
}
