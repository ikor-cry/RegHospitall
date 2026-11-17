using Microsoft.EntityFrameworkCore;
using RegHospital;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace RegHospitalDesctop.Windows
{
    /// <summary>
    /// Логика взаимодействия для AddAppWindow.xaml
    /// </summary>
    public partial class AddAppWindow : Window
    {
        RegHospitalContext db = new RegHospitalContext();
        public AddAppWindow()
        {
            InitializeComponent();

            PatientList.ItemsSource = db.Patients.ToList();
            DoctorList.ItemsSource = db.Doctors.ToList();

            List<string> q = new List<string>();
            q.Add("Отменена");
            q.Add("Запланирована");

            StatusList.ItemsSource = q;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
