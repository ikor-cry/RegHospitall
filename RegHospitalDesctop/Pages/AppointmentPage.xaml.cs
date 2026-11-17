using Microsoft.EntityFrameworkCore;
using RegHospital;
using RegHospital.Objects;
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
    /// Логика взаимодействия для AppointmentPage.xaml
    /// </summary>
    public partial class AppointmentPage : Page
    {
        RegHospitalContext db = new RegHospitalContext();
        ObservableCollection<Appointment> ap = new ObservableCollection<Appointment>();

        public AppointmentPage()
        {
            InitializeComponent();

            db.Appointments.Include(s => s.Patient).Include(s => s.Doctor).Load();

            ap = db.Appointments.Local.ToObservableCollection();

            tableAppointment.ItemsSource = ap;



            PatientList.ItemsSource = db.Patients.ToList();
            DoctorList.ItemsSource= db.Doctors.ToList();
        }

        private void ButtonDel_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AddAppWindow w = new AddAppWindow();
            Appointment z = new Appointment();

            w.DataContext = z;

            if(w.ShowDialog() == true)
            {
                if (db.Appointments.Any(s => s.DoctorId == z.DoctorId && s.AppointmentDate == z.AppointmentDate && s.AppointmentTime == z.AppointmentTime && s.Status == "Запланирован") == false)
                {
                    ap.Add(z);

                    db.SaveChanges();

                }
                else
                {
                    MessageBox.Show("Врач уже имеет запись");
                }
            }
        }
        private void btnclick ( object sender, RoutedEventArgs e)
        {

        }
        private void Button_Click_1 (object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_2 (object sender, RoutedEventArgs e)
        {

        }
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void SpecList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void RoomsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void TextBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {

        }
        public void txtCerarh_TextChanged (object sender, TextChangedEventArgs e)
        {

        }
        private void Appointmet_SelectionChanged(object sender, TextChangedEventArgs e)
        {
        }

        private void SpecList_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {

        }

        private void FilterSpec_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void SortDownButton_Click(object sender, RoutedEventArgs e)
        {
            ap = new ObservableCollection<Appointment>(db.Appointments.OrderByDescending(s => s.AppointmentDate).ThenBy(s => s.AppointmentTime));

            tableAppointment.ItemsSource = ap;
        }
        private void SortUpButton_Click(object sender, RoutedEventArgs e)
        {
            ap = new ObservableCollection<Appointment>(db.Appointments.OrderBy(s => s.AppointmentDate).ThenBy(s => s.AppointmentTime));

            tableAppointment.ItemsSource = ap;

        }

        private void tableAppointment_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(tableAppointment.SelectedItem is Appointment a)
            {
                DataContext = a;
            }
        }
    }
}
