using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
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
        private Employee? selectedEmployee;
        #endregion

        #region Ctor
        public MainWindowViewModel(IDataService<Employee> dataService)
        {
            _dataService = dataService;
            CreateCommand = new AsyncRelayCommand(CreateCommandExecute);
            EditCommand = new AsyncRelayCommand(EditCommandExecute, EditCommandCanExecute);
            LoadCommand = new AsyncRelayCommand(LoadCommandExecute);
            DeleteCommand = new AsyncRelayCommand(DeleteCommandExecute, DeleteCommandCanExecute);
        }
        #endregion

        #region Public Properties
        public ObservableCollection<Employee> Employees { get; } = new ObservableCollection<Employee>();

        public Employee? SelectedEmployee { get => selectedEmployee; set => SetProperty(ref selectedEmployee, value); }
        #endregion

        #region CreateCommand
        public ICommand CreateCommand { get; }

        private async Task CreateCommandExecute()
        {
            var dlg = new DialogView();
            dlg.Title = "Создание";
            if (dlg.ShowDialog() ?? false)
            {
                try
                {
                    var employee = GetEmployee(dlg);
                    await _dataService.Create(employee);
                    OnPropertyChanged(nameof(Employees));
                }
                catch (Exception)
                {
                }
            }
        }
        #endregion

        #region EditCommand
        public ICommand EditCommand { get; }

        private async Task EditCommandExecute()
        {
            var dlg = new DialogView();
            dlg.Title = "Редактирование";
            dlg.FirstName.Text = SelectedEmployee?.FirstName;
            dlg.LastName.Text = SelectedEmployee?.LastName;
            dlg.MiddleName.Text = SelectedEmployee?.MiddleName;
            dlg.Birthday.Text = new DateTime(SelectedEmployee?.Birthday ?? 0).ToShortDateString();
            if (dlg.ShowDialog() ?? false)
            {
                try
                {
                    var employee = GetEmployee(dlg);
                    var updatedEmployee = await _dataService.Update(employee);
                    var found = Employees.FirstOrDefault(x => x.Id == updatedEmployee?.Id);
                    if (found != null)
                    {
                        int i = Employees.IndexOf(found);
                        Employees[i] = updatedEmployee ?? Employees[i];
                    }
                }
                catch (Exception)
                {
                }
            }
        }

        private bool EditCommandCanExecute() { return SelectedEmployee != null; }
        #endregion

        #region DeleteCommand
        public ICommand DeleteCommand { get; }

        private async Task DeleteCommandExecute()
        {
            if (SelectedEmployee != null)
            {
                try
                {
                    await _dataService.Delete(SelectedEmployee);
                }
                catch
                {
                }
            }
        }

        private bool DeleteCommandCanExecute() { return SelectedEmployee != null; }
        #endregion

        #region LoadCommand
        public ICommand LoadCommand { get; }

        private async Task LoadCommandExecute()
        {
            try
            {
                await foreach (var employee in _dataService.Get())
                {
                    Employees.Add(employee);
                }
            }
            catch (Exception ex)
            {
                Logger.Warn(ex);
            }
        }
        #endregion

        #region Private Methods
        private long GetDate(string timeString) { return DateTime.TryParse(timeString, out var date) ? date.Ticks : 0; }
        private Employee GetEmployee(DialogView dlg)
        {
            return new Employee()
            {
                FirstName = dlg.FirstName.Text,
                LastName = dlg.LastName.Text,
                MiddleName = dlg.MiddleName.Text,
                Sex =
                    string.IsNullOrEmpty(dlg.Sex.SelectedItem?.ToString())
                        ? null
                        : (Common.Enums.Sex)dlg.Sex.SelectedIndex,
                HaveChildren = dlg.HaveChildren.IsChecked,
                Birthday = GetDate(dlg.Birthday.Text)
            };
        }
        #endregion
    }
}
