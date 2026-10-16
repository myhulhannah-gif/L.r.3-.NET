using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L.r._3_4_.NET.Models
{
    public class CorporateOrder: Order, IDeliverable, IDiscountable
    {
        private string companyName = "";
        private string edrpou = "";

        public string CompanyName
        {
            get { return companyName; }
            set
            {
                if(string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Назва компанії не може бути порожньою.");
                }

                companyName = value;
            }
        }

        public string EDRPOU
        {
            get { return edrpou; }
            set
            {

                string code;
                if (value == null) code = "";
                else code = value.Trim();

                if (code.Length != 8 || !code.All(char.IsDigit))
                {
                    throw new ArgumentException("Код ЄДРПОУ повинен містити 8 цифр, наприклад 12345678.");
                }

                edrpou = code;
            }
        }

        public string DeliveryMethod { get; } = "Корпоративна кур'єрська доставка";

        public CorporateOrder(int number, string companyName, string edrpou): base(number)
        {
            CompanyName = companyName;
            EDRPOU = edrpou;
        }

        // Реалізація IDeliverable
        public double CalculateDeliveryCost()
        {
            double sumAfterDiscount = TotalAmount - GetDiscountAmount();
            if (sumAfterDiscount >= 5000)
                return 0;
            else
                return 200;
        }

        // Реалізація IDiscountable: завжди 10% від суми товарів
        public double GetDiscountAmount()
        {
            return TotalAmount * 0.10;
        }

        public override double GetTotalCost()
        {
            return base.GetTotalCost() - GetDiscountAmount() + CalculateDeliveryCost();
        }

        public override string GetInfo()
        {
            var info = new StringBuilder();
            info.AppendLine($"Корпоративне замовлення №{Number}");
            info.AppendLine($"Дата: {Date.ToShortDateString()}");
            info.AppendLine($"Компанія: {CompanyName}");
            info.AppendLine($"ЄДРПОУ: {EDRPOU}");
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
