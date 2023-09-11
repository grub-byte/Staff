using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using ClientStaff.Service;
using ClientStaff.Views;
using Common.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NLog;

namespace ClientStaff.ViewModels
{
    internal partial class MainWindowViewModel : ObservableObject
    {
        #region Private Fields
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IDataService<Employee> _dataService;
        #endregion

        #region Ctor
        public MainWindowViewModel(IDataService<Employee> dataService)
        {
            _dataService = dataService;
        }
        #endregion

        #region Public Properties
        [ObservableProperty]
        public ObservableCollection<Employee> employees;

        [ObservableProperty]
        public Employee selectedEmployee;
        #endregion

        #region CreateCommand
        [RelayCommand]
        private async Task CreateCommandExecute()
        {
            var dlg = new DialogView();
            dlg.Title = "Создание";
            if (dlg.ShowDialog() ?? false)
            {
                var employee = GetEmployee(dlg);
                await _dataService.Create(employee);
                OnPropertyChanged(nameof(Employees));
            }
        }
        #endregion

        #region EditCommand
        [RelayCommand(CanExecute = nameof(EditCommandCanExecute))]
        private void EditCommandExecute()
        {
            var dlg = new DialogView();
            dlg.Title = "Редактирование";
            dlg.FirstName.Text = SelectedEmployee.FirstName;
            dlg.LastName.Text = SelectedEmployee.LastName;
            dlg.MiddleName.Text = SelectedEmployee.MiddleName;
            dlg.Birthday.Text = new DateTime(SelectedEmployee.Birthday).ToShortDateString();
            if (dlg.ShowDialog() ?? false)
            {
                var employee = GetEmployee(dlg);
                _dataService.Update(employee);
                OnPropertyChanged(nameof(Employees));
            }
        }
        private bool EditCommandCanExecute()
        {
            return SelectedEmployee != null;
        }
        #endregion

        #region LoadCommand
        [RelayCommand]
        private async Task LoadCommandExecute()
        {
            try
            {
                var items = await _dataService.Get();
                Employees = new ObservableCollection<Employee>(items);
            }
            catch (Exception ex)
            {
                Logger.Warn(ex);
            }

        }
        #endregion

        #region Private Methods
        private long GetDate(string timeString)
        {
            return DateTime.TryParse(timeString, out var date) ? date.Ticks : 0;
        }
        private Employee GetEmployee(DialogView dlg)
        {
            return new Employee()
            {
                FirstName = dlg.FirstName.Text,
                LastName = dlg.LastName.Text,
                MiddleName = dlg.MiddleName.Text,
                Sex = string.IsNullOrEmpty(dlg.Sex.SelectedItem?.ToString()) ? null : (Common.Enums.Sex)dlg.Sex.SelectedIndex,
                HaveChildren = dlg.HaveChildren.IsChecked,
                Birthday = GetDate(dlg.Birthday.Text)
            };
        }
        #endregion
    }
}
