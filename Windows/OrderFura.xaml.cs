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
    /// Логика взаимодействия для OrderFura.xaml
    /// </summary>
    public partial class OrderFura : Window, INotifyPropertyChanged
    {
        public OrderFura(int UR)
        {
            InitializeComponent();
            DataContext = this;
            OrderList = Furnitura4coursedEntities.GetContext().Order.ToList();
            StatusList = Furnitura4coursedEntities.GetContext().Status.ToList();
            StatusList.Insert(0, new Status { StatusName = "Все статусы" });
            if (UR == 3)
            {
                Wrapik.Height = 0;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private IEnumerable<Order> _OrderList;
        private string SearchFilter = "";
        private int StatusFilterID = 0;
        private int SortType = 0;
        public List<Status> StatusList { get; set; }
        public string[] SortList { get; set; } =
        {
            "Без сортировки",
            "Название по повзрастанию",
            "Название по убыванию"
        };

        private void Invalidate(string ComponentName = "OrderList")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("OrderList"));
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(ComponentName));
        }
        public IEnumerable<Order> OrderList
        {
            get
            {
                var Result = _OrderList;

                switch (SortType)
                {
                    case 0:
                        Result = Furnitura4coursedEntities.GetContext().Order.ToList();
                        break;
                    case 1:
                        Result = Furnitura4coursedEntities.GetContext().Order.OrderBy(p => p.OrderName);
                        break;
                    case 2:
                        Result = Furnitura4coursedEntities.GetContext().Order.OrderByDescending(p => p.OrderName);
                        break;
                }

                if (SearchFilter != "")
                    Result = Result.Where(p =>
                    p.OrderName.IndexOf(SearchFilter, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    p.OrderPickupPoint.IndexOf(SearchFilter, StringComparison.OrdinalIgnoreCase) >= 0);

                if (StatusFilterID > 0)
                    Result = Result.Where(i => i.OrderStatus == StatusComboBox.SelectedIndex);

                return Result.Take(100);
            }
            set
            {
                _OrderList = value;
                Invalidate();
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

        private void StatusComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            StatusFilterID = StatusComboBox.SelectedIndex;
            Invalidate();
        }
    }
}