using Furnitura4Coursed.BD;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Shapes;

namespace Furnitura4Coursed.Windows
{
    /// <summary>
    /// Логика взаимодействия для AddTovar.xaml
    /// </summary>
    public partial class AddTovar : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private Tovar _currentproduct = new Tovar();
        public AddTovar(Tovar selectedTovar)
        {
            InitializeComponent();
            DataContext = this;
            ComboManufacturer.ItemsSource = Furnitura4coursedEntities.GetContext().Manufacturer.ToList();
            if (selectedTovar != null)
                _currentproduct = selectedTovar;
            DataContext = _currentproduct;

            if (_currentproduct.TovarID == 0)
            {
                id.Visibility = Visibility.Hidden;
                id2.Visibility = Visibility.Hidden;
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            if (string.IsNullOrWhiteSpace(_currentproduct.TovarName))
                errors.AppendLine("Укажите наименование товара!");
            if (_currentproduct.TovarCost < 0)
                errors.AppendLine("Цена не может быть отрицательной!");
            if (_currentproduct.TovarDiscountAmount < 0 || _currentproduct.TovarDiscountAmount > 99)
                errors.AppendLine("Скидка не может быть такого значения!");
            if (_currentproduct.TovarQuantityInStock < 0)
                errors.AppendLine("Кол-во на складе не может быть отрицательным!");
            if (string.IsNullOrWhiteSpace(_currentproduct.TovarDescription))
                errors.AppendLine("Укажите описание товара!");

            if (errors.Length > 0)
                MessageBox.Show(errors.ToString());
            else
            {
                if (_currentproduct.TovarID == 0)
                    Furnitura4coursedEntities.GetContext().Tovar.Add(_currentproduct);
                try
                {
                    Furnitura4coursedEntities.GetContext().SaveChanges();
                    MessageBox.Show("Информация сохранена!");
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }

        private void ChangePhoto_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog GetImageDialog = new OpenFileDialog();
            GetImageDialog.Filter = "Файлы изображений: (*.png, *.jpg, *.jpeg)|*.png; *.jpg; *.jpeg";
            GetImageDialog.InitialDirectory = Environment.GetEnvironmentVariable("/Resources/");
            if (GetImageDialog.ShowDialog() == true)
            {
                _currentproduct.TovarPhoto = GetImageDialog.FileName.Substring(Environment.CurrentDirectory.Length - 10);
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("_currentproduct"));
                }
            }
        }
    }
}