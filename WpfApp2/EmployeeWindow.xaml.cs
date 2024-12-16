using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
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
using WpfApp2;

namespace Pharmacy321
{
    /// <summary>
    /// Логика взаимодействия для EmployeeWindow.xaml
    /// </summary>
    public partial class EmployeeWindow : Window
    {
        // Создайте коллекцию для хранения препаратов
        private ObservableCollection<Preparat> preparations;
        private Database database;
        public class Preparat
        {
            public int ID_Preparat { get; set; }
            public string Nazvanie { get; set; }
            public int Kolishestvo { get; set; }
            public int Price { get; set; }
            public int? Skidka { get; set; } // Используем int? для возможности null
        }

        public EmployeeWindow()
        {
            InitializeComponent();
            database = new Database(); // Инициализируем базу данных
            preparations = new ObservableCollection<Preparat>();
            ProductsDataGrid.ItemsSource = preparations; // Установите источник данных для DataGrid

            LoadPreparations();
        }
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var newPreparat = new Preparat
            {
                Nazvanie = NameTextBox.Text,
                Price = int.TryParse(PriceTextBox.Text, out int price) ? price : 0,
                Kolishestvo = int.TryParse(QuantityTextBox.Text, out int quantity) ? quantity : 0,
                Skidka = int.TryParse(DiscountTextBox.Text, out int discount) ? discount : (int?)null
            };

            // Добавление препарата в базу данных
            database.AddPreparat(newPreparat.Nazvanie, newPreparat.Kolishestvo, newPreparat.Price, newPreparat.Skidka);

            // Очистка полей ввода
            NameTextBox.Clear();
            QuantityTextBox.Clear();
            PriceTextBox.Clear();
            DiscountTextBox.Clear();

            // Обновление списка препаратов
            LoadPreparations();
        }

        // Метод для загрузки препаратов из базы данных
        // Метод для загрузки препаратов из базы данных
        // Метод для загрузки препаратов из базы данных
        private void LoadPreparations()
        {
            preparations.Clear(); // Очищаем текущий список
            var preparats = database.GetPreparations(); // Получите все препараты из базы данных
            foreach (var preparat in preparats)
            {
                // Убедитесь, что здесь используется класс Preparat из Pharmacy321.EmployeeWindow
                preparations.Add(new Pharmacy321.EmployeeWindow.Preparat
                {
                    ID_Preparat = preparat.ID_Preparat,
                    Nazvanie = preparat.Nazvanie,
                    Kolishestvo = (int)preparat.Kolishestvo,
                    Price = (int)preparat.Price,
                    Skidka = preparat.Skidka
                });
            }
        }


    }
}