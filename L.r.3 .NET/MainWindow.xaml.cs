using L.r._3_4_.NET.Models;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace L.r._3_4_.NET
{
    public partial class MainWindow : Window
    {
        // Список усіх створених замовлень
        private List<Order> _orders = new List<Order>();
        private List<string> _cartNames = new List<string>();
        private List<double> _cartPrices = new List<double>();
        // Лічильник для автонумерації
        private int _nextNumber = 1;

        public MainWindow()
        {
            InitializeComponent();

            DeliveryComboBox.ItemsSource = OnlineOrder.AvailableMethods; // Заповнення списку способів доставки
            DeliveryComboBox.SelectedIndex = 0;

            NoCardRadio.IsChecked = true;

            OrderTypeComboBox.SelectedIndex = 0;  //Перший тип замолення: онлайн-замовлення
        }

        private void OrderTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (OrderTypeComboBox.SelectedIndex == 0)   // Інтернет-замовлення
            {
                OnlineFields.Visibility = Visibility.Visible;
                StoreFields.Visibility = Visibility.Collapsed;
                CorporateFields.Visibility = Visibility.Collapsed;
            }
            else if (OrderTypeComboBox.SelectedIndex == 1) // Замовлення в магазині
            {
                OnlineFields.Visibility = Visibility.Collapsed;
                StoreFields.Visibility = Visibility.Visible;
                CorporateFields.Visibility = Visibility.Collapsed;
            }
            else
            {
                OnlineFields.Visibility = Visibility.Collapsed;
                StoreFields.Visibility = Visibility.Collapsed;
                CorporateFields.Visibility = Visibility.Visible;
            }
        }

        private void CardRadio_Changed(object sender, RoutedEventArgs e)
        {
            // Поле телефону видно, лише коли обрано «Є»
            if (HasCardRadio.IsChecked == true)
                PhonePanel.Visibility = Visibility.Visible;
            else
                PhonePanel.Visibility = Visibility.Collapsed;
        }

        private void AddItemButton_Click(object sender, RoutedEventArgs e)
        {
            string name = ItemNameTextBox.Text.Trim();

            if (name == "")
            {
                MessageBox.Show("Введіть назву товару.",
                    "Помилка введення", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!double.TryParse(PriceTextBox.Text, out double price))
            {
                MessageBox.Show("Ціна має бути числом, наприклад: 150 або 99,5 (дріб пишіть через кому).",
                    "Помилка введення", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (price <= 0)
            {
                MessageBox.Show("Ціна має бути більшою за нуль.",
                    "Помилка введення", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            _cartNames.Add(name);
            _cartPrices.Add(price);
            CartListBox.Items.Add(name + ": " + price.ToString("C"));

            ItemNameTextBox.Clear();
            PriceTextBox.Clear();
            ItemNameTextBox.Focus();
        }

        private void CreateOrderButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                if (_cartPrices.Count == 0)
                {
                    MessageBox.Show("Додайте хоча б один товар.",
                        "Помилка введення", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                Order order;

                if (OrderTypeComboBox.SelectedIndex == 0)
                {
                    string method = DeliveryComboBox.SelectedItem.ToString();
                    order = new OnlineOrder(_nextNumber, method);
                }
                else if (OrderTypeComboBox.SelectedIndex == 1)
                {

                    var store = new StoreOrder(_nextNumber, CustomerNameTextBox.Text);

                    if (HasCardRadio.IsChecked == true)
                    {
                        store.LoyaltyPhone = LoyaltyPhoneTextBox.Text;
                    }
                    order = store;
                }
                else
                {
                    order = new CorporateOrder(_nextNumber, CompanyNameTextBox.Text, EdrpouTextBox.Text);
                }

                for (int i = 0; i < _cartPrices.Count; i++)
                {
                    order.AddItem(_cartNames[i], _cartPrices[i]);
                }

                // Додаємо замовлення в список і показуємо його опис
                _orders.Add(order);
                OrdersListBox.Items.Add(order.GetInfo());

                UpdateAnalytics();

                _nextNumber++;

                ClearCart();
                CustomerNameTextBox.Clear();
                LoyaltyPhoneTextBox.Clear();
                CompanyNameTextBox.Clear();
                EdrpouTextBox.Clear();
                NoCardRadio.IsChecked = true;
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Помилка валідації",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Сталася неочікувана помилка: " + ex.Message, "Помилка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ClearCart()
        {
            _cartNames.Clear();
            _cartPrices.Clear();
            CartListBox.Items.Clear();
        }

        // Оновлює загальну вартість усіх замовлень із доставкою
        private void UpdateAnalytics()
        {
            OnlineTotalTextBlock.Text = OrderAnalysis.GetTotalCostWithDelivery(_orders).ToString("C");
            StoreTotalTextBlock.Text = OrderAnalysis.GetStoreTotal(_orders).ToString("C");
            CorporateTotalTextBlock.Text = OrderAnalysis.GetCorporateTotal(_orders).ToString("C");
            AllTotalTextBlock.Text = OrderAnalysis.GetAllOrdersTotal(_orders).ToString("C");

            List<IDiscountable> discountables = _orders.OfType<IDiscountable>().ToList();
            DiscountTotalTextBlock.Text = OrderAnalysis.GetTotalDiscount(discountables).ToString("C");
        }
    }
}