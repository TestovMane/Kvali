using Furnitura4Coursed.BD;
using Furnitura4Coursed.Windows;
using System;
using System.Collections.Generic;
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

namespace Furnitura4Coursed
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<User> userObj;
        private string UserFIO;
        private string Zdravie = "Добро пожаловать, Гость!";
        public MainWindow()
        {
            InitializeComponent();
            userObj = Furnitura4coursedEntities.GetContext().User.ToList();
        }

        private void Enter_Click(object sender, RoutedEventArgs e)
        {
            var CurrentUsers = userObj.Where(user => user.UserLogin == log.Text && user.UserPassword == pass.Password).FirstOrDefault();
            if (CurrentUsers != null)
            {
                UserFIO = $"Добро пожаловать, {CurrentUsers.UserSurname} {CurrentUsers.UserName} {CurrentUsers.UserPatronymic}!";
                if (CurrentUsers.UserRole == 1)
                {
                    TovarFura a = new TovarFura(1, UserFIO);
                    a.Show();
                    this.Close();
                }
                else if (CurrentUsers.UserRole == 2)
                {
                    TovarFura m = new TovarFura(2, UserFIO);
                    m.Show();
                    this.Close();
                }
                else if (CurrentUsers.UserRole == 3)
                {
                    TovarFura c = new TovarFura(3, UserFIO);
                    c.Show();
                    this.Close();
                }
            }
            else MessageBox.Show("Вы ввели неправильный логин или пароль!", "Внимание!", MessageBoxButton.OK);
        }

        private void GuestEnter_Click(object sender, RoutedEventArgs e)
        {
            TovarFura g = new TovarFura(2, Zdravie);
            g.Show();
            this.Close();
        }
    }
}
