using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L.r._3_4_.NET.Models
{
    // Клас для обрахунку загальної вартості усіх замовлень разом із доставкою
    public class OrderAnalysis
    {
        // Загальна вартість онлайн-замовлень
        public static double GetTotalCostWithDelivery(List<Order> orders)
        {
            double total = 0;

            foreach (Order order in orders)
            {

                if (order is OnlineOrder)
                {
                    total += order.GetTotalCost();
                }
            }

            return total;
        }

        // Загальна вартість замовлень у магазині
        public static double GetStoreTotal(List<Order> orders)
        {
            double total = 0;

            foreach (Order order in orders)
            {
                if (order is StoreOrder)
                {
                    total += order.GetTotalCost();
                }
            }

            return total;
        }

        // Загальна вартість корпоративних замовлень
        public static double GetCorporateTotal(List<Order> orders)
        {
            double total = 0;

            foreach (Order order in orders)
            {

                if (order is CorporateOrder)
                {
                    total += order.GetTotalCost();
                }
            }

            return total;
        }

        // Загальна вартість усіх замовлень
        public static double GetAllOrdersTotal(List<Order> orders)
        {
            double total = 0;

            foreach (Order order in orders)
            {
                total += order.GetTotalCost();
            }

            return total;
        }

        // Загальна сума знижок 
        public static double GetTotalDiscount(List<IDiscountable> items)
        {
            double total = 0;

            foreach (IDiscountable item in items)
            {
                total += item.GetDiscountAmount();
            }
            return total;
        }
    }

}
