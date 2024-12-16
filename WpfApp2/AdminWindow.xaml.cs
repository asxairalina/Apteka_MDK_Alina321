using System;
using System.Data;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using WpfApp2;

namespace Pharmacy321
{
    public partial class AdminWindow : Window
    {
        private Database database;
        private apteka1Entities1 context;
        public AdminWindow()
        {
            InitializeComponent();
            database = new Database();
            LoadContractsGrid();  // Загрузка договоров
            LoadEmployeesGrid();// Загрузка сотрудников
            LoadSuppliersGrid(); 
        }
        private void AddSupplierButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Проверяем, что TextBox не пустые
                if (string.IsNullOrEmpty(UR_NazvanieTextBox.Text) ||
                    string.IsNullOrEmpty(UR_AdresTextBox.Text) ||
                    string.IsNullOrEmpty(INNTextBox.Text) ||
                    string.IsNullOrEmpty(NumberDogovorTextBox.Text) ||
                    string.IsNullOrEmpty(KodOKPOTextBox.Text) ||
                    string.IsNullOrEmpty(TelefonaTextBox.Text) ||
                    string.IsNullOrEmpty(KontaktOtvetLicaTextBox.Text))
                {
                    MessageBox.Show("Пожалуйста, заполните все поля.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Создаем новый объект поставщика
                var newSupplier = new Postavcik
                {
                    UR_Nazvanie = UR_NazvanieTextBox.Text,
                    UR_Adres = UR_AdresTextBox.Text,
                    INN = int.Parse(INNTextBox.Text),
                    Number_Dogovor = int.Parse(NumberDogovorTextBox.Text),
                    Kod_OKPO = int.Parse(KodOKPOTextBox.Text),
                    Telefon = TelefonaTextBox.Text,
                    Kontakt_Otvet_Lica = int.Parse(KontaktOtvetLicaTextBox.Text)
                };

                // Подключение к базе данных
                using (var context = new apteka1Entities1()) // Обязательно инициализируйте контекст
                {
                    context.Postavcik.Add(newSupplier); // Добавляем объект поставщика
                    context.SaveChanges(); // Сохраняем изменения
                }

                MessageBox.Show("Поставщик успешно добавлен!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                // Очистить поля после добавления
                UR_NazvanieTextBox.Clear();
                UR_AdresTextBox.Clear();
                INNTextBox.Clear();
                NumberDogovorTextBox.Clear();
                KodOKPOTextBox.Clear();
                TelefonaTextBox.Clear();
                KontaktOtvetLicaTextBox.Clear();

                LoadSuppliersGrid(); // Обновляем список поставщиков
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadSuppliersGrid()
        {
            try
            {
                // Инициализируем контекст, если он не был инициализирован
                if (context == null)
                {
                    context = new apteka1Entities1();
                }

                var suppliersList = context.Postavcik.ToList();
                SuppliersDataGrid.ItemsSource = suppliersList;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке списка поставщиков: {ex.Message}");
            }
        }



        private void LoadContractsGrid()
        {
            DataTable contracts = database.GetContracts(); // Метод для получения договоров из БД
            ContractsDataGrid.ItemsSource = contracts.DefaultView;
        }
        private void LoadEmployeesGrid()
        {
            DataTable employees = database.GetEmployees(); // Метод для получения сотрудников из БД
            EmployeesDataGrid.ItemsSource = employees.DefaultView;
        }

        private void AddEmployeeButton_Click(object sender, RoutedEventArgs e)
        {
            string FName = FNameTextBox.Text;
            string Name = NameTextBox.Text;
            string Othestvo = OthestvoTextBox.Text;
            string Adres = AdresTextBox.Text;
            string Telefon= TelefonTextBox.Text;
            string Poshta = PoshtaTextBox.Text;
            string Doljnost = DoljnostTextBox.Text;
            _ = Shas_RabotTextBox.Text;

            int telefon;
            if (!int.TryParse(TelefonTextBox.Text, out telefon))
            {
                MessageBox.Show("Введите корректный номер телефона.");
                return;
            }

            
            

            int shasRabot;
            if (!int.TryParse(Shas_RabotTextBox.Text, out shasRabot))
            {
                MessageBox.Show("Введите корректное количество часов работы.");
                return;
            }

            database.AddEmployee(FName, Name, Othestvo, Adres, Telefon, Poshta, Doljnost, shasRabot);

            // Метод для добавления сотрудника в БД
            MessageBox.Show("Сотрудник добавлен успешно!");

            // Очистка полей после добавления
            FNameTextBox.Clear();
            NameTextBox.Clear();
            OthestvoTextBox.Clear();
            AdresTextBox.Clear();
            TelefonTextBox.Clear();
            PoshtaTextBox.Clear();
            DoljnostTextBox.Clear();
            Shas_RabotTextBox.Clear();
        }


        private void CreateContractButton_Click(object sender, RoutedEventArgs e)
        {
            string Nomer_Dogovota = Nomer_DogovotaTextBox.Text;

            if (string.IsNullOrWhiteSpace(Nomer_Dogovota))
            {
                MessageBox.Show("Пожалуйста, введите номер договора.");
                return;
            }

            if (int.TryParse(Nomer_Dogovota, out int nomerDogovora))
            {
                try
                {
                    database.CreateContract(nomerDogovora); // Создание договора в БД
                    MessageBox.Show("Договор создан успешно!");

                    // Очистка полей после добавления
                    Nomer_DogovotaTextBox.Clear();

                    LoadContractsGrid(); // Обновление списка договоров
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Произошла ошибка: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("Номер договора должен быть числом.");
            }
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            // Логика выхода из окна администратора
            MainWindow mainWindow = new MainWindow(); // Переход на окно авторизации
            mainWindow.Show();
            this.Close(); // Закрытие текущего окна
        }

    }
}