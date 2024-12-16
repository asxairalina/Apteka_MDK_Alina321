using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Windows;
using Microsoft.IdentityModel.Protocols;
using System.Runtime.Remoting.Contexts;
using WpfApp2;

namespace Pharmacy321
{
    public class Database
    {
        private apteka1Entities1 context;

        public Database()
        {
            // Инициализируем контекст
            context = new apteka1Entities1();
        }

        // Получение клиентов
        public DataTable GetClients()
        {
            DataTable clientsTable = new DataTable();

            // Заголовки столбцов
            clientsTable.Columns.Add("ID_Klient", typeof(int));
            clientsTable.Columns.Add("FName", typeof(string));
            clientsTable.Columns.Add("Name", typeof(string));
            clientsTable.Columns.Add("Othestvo", typeof(string));
            clientsTable.Columns.Add("Pochta", typeof(string));
            clientsTable.Columns.Add("Telefon", typeof(string));
            clientsTable.Columns.Add("Skidka", typeof(string));

            // Получаем данные клиентов из базы
            var clients = context.Klient.ToList();
            foreach (var client in clients)
            {
                clientsTable.Rows.Add(client.ID_Klient, client.FName, client.Name, client.Othestvo, client.Pochta, client.Telefon, client.Skidka);
            }

            return clientsTable;
        }

        // Получение специалистов
        public DataTable GetSpecialists()
        {
            DataTable specialistsTable = new DataTable();
            specialistsTable.Columns.Add("ID_Sotudnica", typeof(int));
            specialistsTable.Columns.Add("FullNameAndPosition", typeof(string));

            var specialists = context.Sotrudnic.ToList();
            foreach (var specialist in specialists)
            {
                string fullNameAndPosition = $"{specialist.FName} - {specialist.Doljnost}";
                specialistsTable.Rows.Add(specialist.ID_Sotudnica, fullNameAndPosition);
            }

            return specialistsTable;
        }

        // Добавление клиента
        public void AddClient(string FName, string Name, string Othestvo, string Pochta, string Telefon, int Skidka)
        {
            try
            {
                Klient newClient = new Klient
                {
                    FName = FName,
                    Name = Name,
                    Othestvo = Othestvo,
                    Pochta = Pochta,
                    Telefon = Telefon,
                    Skidka = Skidka
                };

                context.Klient.Add(newClient);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении клиента: {ex.Message}");
            }
        }


        // Получение сотрудников
        public DataTable GetEmployees()
        {
            DataTable employeesTable = new DataTable();
            employeesTable.Columns.Add("ID_Sotudnica", typeof(int));
            employeesTable.Columns.Add("FName", typeof(string));
            employeesTable.Columns.Add("Name", typeof(string));
            employeesTable.Columns.Add("Othestvo", typeof(string));
            employeesTable.Columns.Add("Adres", typeof(string));
            employeesTable.Columns.Add("Telefon", typeof(string));
            employeesTable.Columns.Add("Poshta", typeof(string));
            employeesTable.Columns.Add("Doljnost", typeof(string));
            employeesTable.Columns.Add("Shas_Rabot", typeof(string));

            var employees = context.Sotrudnic.ToList();
            foreach (var employee in employees)
            {
                employeesTable.Rows.Add(employee.ID_Sotudnica, employee.FName, employee.Name, employee.Othestvo, employee.Adres, employee.Telefon, employee.Poshta, employee.Doljnost, employee.Shas_Rabot);
            }

            return employeesTable;
        }

        // Получение договоров
        public DataTable GetContracts()
        {
            DataTable contractsTable = new DataTable();
            contractsTable.Columns.Add("ID_Dogovora", typeof(int));
            contractsTable.Columns.Add("Nomer_Dogovota", typeof(string));

            var contracts = context.Dogovor.ToList();
            foreach (var contract in contracts)
            {
                contractsTable.Rows.Add(contract.ID_Dogovora, contract.Nomer_Dogovota);
            }

            return contractsTable;
        }

        // Добавление сотрудника
        public void AddEmployee(string fName, string name, string othestvo, string adres, string telefon, string poshta, string doljnost, int shas_rabot)
        {
            try
            {
                Sotrudnic newEmployee = new Sotrudnic
                {
                    FName = fName,
                    Name = name,
                    Othestvo = othestvo,
                    Adres = adres,
                    Telefon = telefon,
                    Poshta = poshta,
                    Doljnost = doljnost,
                    Shas_Rabot = shas_rabot
                };

                context.Sotrudnic.Add(newEmployee);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении сотрудника: {ex.Message}");
            }
        }

        // Создание договора
        // Создание договора
        public void CreateContract(int nomerDogovota)
        {
            try
            {
                Dogovor contract = new Dogovor
                {
                    Nomer_Dogovota = nomerDogovota,
                  
                };

                context.Dogovor.Add(contract);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при создании договора: {ex.Message}\n{ex.StackTrace}");
            }
        }
        public void AddPreparat(string nazvanie, int kolishestvo, int price, int? skidka)
        {
            try
            {
                Preparat newPreparat = new Preparat
                {
                    Nazvanie = nazvanie,
                    Kolishestvo = kolishestvo,
                    Price = price,
                    Skidka = skidka
                };

                context.Preparat.Add(newPreparat);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении препарата: {ex.Message}");
            }
        }
        // Пример метода для получения препаратов из базы данных
        public List<Preparat> GetPreparations()
        {
            return context.Preparat.ToList(); // Предполагается, что у вас есть DbSet<Preparat> в вашем контексте базы данных
        }



    }
}
