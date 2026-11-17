using Microsoft.EntityFrameworkCore;
using RegHospital;
using RegHospital.Objects;
using RegHospitalDesctop.Views;
using RegHospitalDesctop.Windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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


namespace RegHospitalDesctop.Pages
{
    /// <summary>
    /// Логика взаимодействия для PatientPage.xaml
    /// </summary>
    public partial class PatientPage : Page
    {
        RegHospitalContext db = new RegHospitalContext();

        ObservableCollection<Patient> patients = new ObservableCollection<Patient>(); 

        public PatientPage()
        {
            InitializeComponent();

            db.Patients.Load(); // подгрузи в локальное хранилище 

            patients = db.Patients.Local.ToObservableCollection();

            tablePatient.ItemsSource = patients;



            var P_Item = db.Patients.Select(x => x.Gender).Distinct().ToList();
            P_Item.Insert(0, "Все" );
            FilterPatient.ItemsSource = P_Item;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            db.SaveChanges();
        }

        private void Del_Click(object sender, RoutedEventArgs e)
        {
            if (tablePatient.SelectedItem is Patient p)
            {
                patients.Remove(p);
                db.SaveChanges();

            }
        }

        private void Dob_Click(object sender, RoutedEventArgs e)
        {
            Patient p = new Patient();
            AddPatientWindow pp = new AddPatientWindow();
            pp.DataContext = p;


            if (pp.ShowDialog() == true) // стоит код пока не закроем окно
            {
                patients.Add(p);

                db.SaveChanges();
            }

        }
        private void Patientlist(object sender, SelectionChangedEventArgs e)
        {
            if (tablePatient.SelectedItem is Patient p)
            {
                DataContext = p;

            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            patients = new ObservableCollection<Patient>(patients.OrderBy(c => c.FullName));
            tablePatient.ItemsSource = patients;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            patients = new ObservableCollection<Patient>(patients.OrderByDescending(c =>  c.FullName));
            tablePatient.ItemsSource= patients;
        }
        private void FilterPatient_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(FilterPatient.SelectedValue == "Все")
            {
                patients = db.Patients.Local.ToObservableCollection();
                tablePatient.ItemsSource = patients;
                return;
            }


            var p = db.Patients.Where(x => x.Gender == FilterPatient.SelectedValue).ToList();

            patients = new ObservableCollection<Patient>(p);

            tablePatient.ItemsSource = patients;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var s = txtSearch.Text;

            var pt = db.Patients.Local.Where(x => x.FullName.Contains(s));

            patients = new ObservableCollection<Patient>(pt);

            tablePatient.ItemsSource = patients;
        }
      
    }
}
