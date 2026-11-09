using RegHospital;
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
using RegHospital.Objects;
using Microsoft.EntityFrameworkCore;
using RegHospitalDesctop.Windows;
using RegHospitalDesctop.Views;

namespace RegHospitalDesctop.Pages
{
    /// <summary>
    /// Логика взаимодействия для DoctorPage.xaml
    /// </summary>
    public partial class DoctorPage : Page
    {
        RegHospitalContext db = new RegHospitalContext();
        ObservableCollection<Doctor> docs = new ObservableCollection<Doctor>();

        public DoctorPage()
        {
            InitializeComponent();

            db.Doctors.Include(s => s.Specialtie).Include(e => e.Room).Load();
            docs = db.Doctors.Local.ToObservableCollection();
            tableDoctor.ItemsSource = docs;

            SpecList.ItemsSource = db.Specialties.ToList();
            RoomsList.ItemsSource = db.Rooms.ToList();

            //  FilterSpec.ItemsSource = db.Specialties.ToList();
            var temp_spec = db.Specialties.Select(x => new SpecFilterItem { Id = x.SpecialtieId, Name = x.Name }).ToList();
            temp_spec.Insert(0, new SpecFilterItem { Id = 0, Name = "Все специальности" });
            FilterSpec.ItemsSource = temp_spec;
        }

        private void btnclick(object sender, RoutedEventArgs e)
        {
            db.SaveChanges();
            tableDoctor.ItemsSource = null;
            tableDoctor.ItemsSource = docs;
        }

        private void tableDoctor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(tableDoctor.SelectedItem is Doctor d)
            {
                this.DataContext = d;
                
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Doctor d = new Doctor();
            AddDoctorWindow w = new AddDoctorWindow();
            w.DataContext = d;


            if (w.ShowDialog() == true) // стоит код пока не закроем окно
            {
                docs.Add(d);

                db.SaveChanges();
            }
            
        }

        private void ButtonDel_Click(object sender, RoutedEventArgs e)
        {
            if (tableDoctor.SelectedItem is Doctor d)
            {
                var r = MessageBox.Show($"Точно удалить {d.FullName}?",
                    "Удаление",
                    MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (r == MessageBoxResult.Yes)
                {
                    docs.Remove(d);

                     db.SaveChanges();
                }

                
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            // var d = db.Doctors.OrderBy(x => x.FullName).ToList();
            // docs = new ObservableCollection<Doctor>(d);

            docs = new ObservableCollection<Doctor>(docs.OrderBy(x => x.FullName));

            tableDoctor.ItemsSource = docs;
        }

        private void Button_CLick_2(object sender, RoutedEventArgs e)
        {
            docs = new ObservableCollection<Doctor>(docs.OrderByDescending(x => x.FullName));
            tableDoctor.ItemsSource = docs;
        }

        private void FilterSpec_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Convert.ToInt32(FilterSpec.SelectedValue) == 0)
            {
                docs = db.Doctors.Local.ToObservableCollection();
                tableDoctor.ItemsSource = docs;
                return;
            }


            var temp = db.Doctors.Where(x => x.SpecialtieId == Convert.ToInt32(FilterSpec.SelectedValue)).ToList();

            docs = new ObservableCollection<Doctor>(temp);

            tableDoctor.ItemsSource = docs;
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {

            string s = txtSearch.Text;
            var temp = db.Doctors.Include(x => x.Specialtie).Where(x => x.FullName.Contains(s)
            || x.WorkShedule.Contains(s) || x.Specialtie.Name.Contains(s));

            docs = new ObservableCollection<Doctor>(temp);
            //d

            tableDoctor.ItemsSource = docs;
        }
    }
}
