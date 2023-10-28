using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RestaurantBillCalc
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<MenuItem> BillItems { get; set; } = new ObservableCollection<MenuItem>();
        
        public MainWindow()
        {
            InitializeComponent();
            dgItemList.ItemsSource = BillItems;
            DataContext = this;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 0) return;

            var comboBox = (ComboBox)sender;
            var selectedItem = (ComboBoxItem)e.AddedItems[0];
            var price = double.Parse((string)selectedItem.Tag);

            BillItems.Add(new MenuItem { Item = (string)selectedItem.Content, Price = price });

            comboBox.SelectedIndex = -1;

            UpdateBill();

        }

        private void UpdateBill()
        {
            double subtotal = 0;
            foreach (var item in BillItems)
            {
                subtotal += item.Price;
            }

            double tax = subtotal * 0.13;
            double total = subtotal + tax;

            txtSubtotal.Text = $"${subtotal:F2}";
            txtTax.Text = $"${tax:F2}";
            txtTotal.Text = $"${total:F2}";
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (dgItemList.SelectedItems.Count > 0)
            {
                for (int i = dgItemList.SelectedItems.Count - 1; i >= 0; i--)
                {
                    BillItems.Remove((MenuItem)dgItemList.SelectedItems[i]);
                }

                UpdateBill();
            }
        }

        private void ClearBillButton_Click(object sender, RoutedEventArgs e)
        {
            BillItems.Clear();
            UpdateBill();
        }

        private void CompanyLogo_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo("http://www.centennialcollege.ca/")
            {
                UseShellExecute = true
            });
        }

        private void PrintBillButton_Click(object sender, RoutedEventArgs e)
        {
            if (BillItems.Count == 0)
            {
                MessageBox.Show("Please add items to the bill before generating an invoice.", "No items in the bill", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            StringBuilder invoiceDetails = new StringBuilder("Invoice:\n\n");

            foreach (var item in BillItems)
            {
                invoiceDetails.AppendLine($"{item.Item}");
            }

            invoiceDetails.AppendLine($"\nSubtotal: ${txtSubtotal.Text:F2}");
            invoiceDetails.AppendLine($"Tax: ${txtTax.Text:F2}");
            invoiceDetails.AppendLine($"Total: ${txtTotal.Text:F2}");

            MessageBox.Show(invoiceDetails.ToString(), "Invoice Details", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }

    public class MenuItem
    {
        public string Item { get; set; }

        public double Price { get; set; }

    }
}
