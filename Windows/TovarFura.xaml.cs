using Furnitura4Coursed.BD;
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
    /// Логика взаимодействия для TovarFura.xaml
    /// </summary>
    public partial class TovarFura : Window, INotifyPropertyChanged
    {
        public string UserFIOass;
        public event PropertyChangedEventHandler PropertyChanged;
        private IEnumerable<Tovar> _TovarList;
        private int DiscountAmountFilterID = 0;
        private int SortType = 0;
        public List<Role> DiscountAmountList { get; set; }
        public string[] SortList { get; set; } =
        {
            "Без сортировки",
            "Стоимость по возрастанию",
            "Стоимость по убыванию",
            "Производитель по алфавиту",
            "Производитель против алфавита",
            "Количество на складе по возрастанию",
            "Количество на складе по убыванию",
        };

        private void Invalidate(string ComponentName = "TovarList")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("TovarList"));
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(ComponentName));
        }

        private string SearchFilter = "";

        public IEnumerable<Tovar> TovarList
        {
            get
            {
                var Result = _TovarList;

                switch (SortType)
                {
                    case 0:
                        Result = Furnitura4coursedEntities.GetContext().Tovar.ToList();
                        break;
                    case 1:
                        Result = Furnitura4coursedEntities.GetContext().Tovar.OrderBy(p => p.TovarCost);
                        break;
                    case 2:
                        Result = Furnitura4coursedEntities.GetContext().Tovar.OrderByDescending(p => p.TovarCost);
                        break;
                    case 3:
                        Result = Furnitura4coursedEntities.GetContext().Tovar.OrderBy(p => p.Manufacturer.ManufacturerName);
                        break;
                    case 4:
                        Result = Furnitura4coursedEntities.GetContext().Tovar.OrderByDescending(p => p.Manufacturer.ManufacturerName);
                        break;
                    case 5:
                        Result = Furnitura4coursedEntities.GetContext().Tovar.OrderBy(p => p.TovarQuantityInStock);
                        break;
                    case 6:
                        Result = Furnitura4coursedEntities.GetContext().Tovar.OrderByDescending(p => p.TovarQuantityInStock);
                        break;
                }
                if (SearchFilter != "")
                    Result = Result.Where(p =>
                    p.TovarName.IndexOf(SearchFilter, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    p.TovarDescription.IndexOf(SearchFilter, StringComparison.OrdinalIgnoreCase) >= 0);

                if (DiscountAmountComboBox.SelectedIndex == 1)
                {
                    Result = Result.Where(i => i.TovarDiscountAmount >= 0 && i.TovarDiscountAmount <= 10.99m);
                }
                if (DiscountAmountComboBox.SelectedIndex == 2)
                {
                    Result = Result.Where(i => i.TovarDiscountAmount >= 11 && i.TovarDiscountAmount <= 14.99m);
                }
                if (DiscountAmountComboBox.SelectedIndex == 3)
                {
                    Result = Result.Where(i => i.TovarDiscountAmount >= 15);
                }

                return Result.Take(100);
            }
            set
            {
                _TovarList = value;
                Invalidate();
            }
        }

        int UR { get; set; }

        public TovarFura(int UserRoles, string Faio)
        {
            UR = UserRoles;
            InitializeComponent();
            DataContext = this;
            TovarList = Furnitura4coursedEntities.GetContext().Tovar.ToList();
            DiscountAmountList = Furnitura4coursedEntities.GetContext().Role.ToList();
            DiscountAmountList.Clear();
            DiscountAmountList.Insert(0, new Role { RoleName = "Все диапазоны" });
            DiscountAmountList.Insert(1, new Role { RoleName = "Скидки от 0% до 10,99%" });
            DiscountAmountList.Insert(2, new Role { RoleName = "Скидки от 11% до 14,99%" });
            DiscountAmountList.Insert(3, new Role { RoleName = "Скидка 15% и более" });
            UserFIOass = Faio;
            usersFaio.Text = UserFIOass;
            if (UserRoles == 0)
            {
                AddTovar.Visibility = Visibility.Hidden;
                TovarListView.Visibility = Visibility.Hidden;
                Wrapik.Height = 0;
                Update.Visibility = Visibility.Hidden;
                OrderList.Visibility = Visibility.Hidden;
            }
            if (UserRoles == 1)
            {
                TovarListView2.Visibility = Visibility.Hidden;
            }
            if (UserRoles == 2)
            {
                AddTovar.Visibility = Visibility.Hidden;
                TovarListView.Visibility = Visibility.Hidden;
                OrderList.Visibility = Visibility.Hidden;
                Update.Visibility = Visibility.Hidden;
                Wrapik.Height = 0;
            }
            if (UserRoles == 3)
            {
                AddTovar.Visibility = Visibility.Hidden;
                TovarListView.Visibility = Visibility.Hidden;
                Update.Visibility = Visibility.Hidden;
            }
        }

        private void SearchFilterTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            SearchFilter = SearchFilterTextBox.Text;
            Invalidate();
        }

        private void SortFilterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SortType = SortTypeComboBox.SelectedIndex;
            Invalidate();
        }

        private void DiscountAmountComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DiscountAmountFilterID = DiscountAmountComboBox.SelectedIndex;
            Invalidate();
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            TovarList = Furnitura4coursedEntities.GetContext().Tovar.ToList();
            TovarFura m = new TovarFura(UR, UserFIOass);
            m.Show();
            this.Close();
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            AddTovar add = new AddTovar((sender as Button).DataContext as Tovar);
            add.ShowDialog();
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            var TovarForRemoving = TovarListView.SelectedItems.Cast<Tovar>().ToList();
            if (MessageBox.Show($"Вы точно хотите удалить следующие {TovarForRemoving.Count()} элементы?", "Внимание!", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    Furnitura4coursedEntities.GetContext().Tovar.RemoveRange(TovarForRemoving);
                    Furnitura4coursedEntities.GetContext().SaveChanges();
                    MessageBox.Show("Данные удалены!");
                    TovarListView.ItemsSource = Furnitura4coursedEntities.GetContext().Tovar.ToList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            MainWindow m = new MainWindow();
            m.Show();
            this.Close();
        }

        private void AddTovar_Click(object sender, RoutedEventArgs e)
        {
            AddTovar add = new AddTovar(null);
            add.ShowDialog();
        }

        private void OrderList_Click(object sender, RoutedEventArgs e)
        {
            OrderFura add = new OrderFura(UR);
            add.ShowDialog();
        }
    }
}