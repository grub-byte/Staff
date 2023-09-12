using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using ClientStaff.Service;
using ClientStaff.Views;
using Common.Models.Client;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NLog;

namespace ClientStaff.ViewModels
{
    internal partial class MainWindowViewModel : ObservableObject
    {
        private const string STR_READY = "Готово";
        #region Private Fields
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IDataService<ClientEmployee> _dataService;
        private ClientEmployee? selectedEmployee;
        private string statusText;
        #endregion

        #region Ctor
        public MainWindowViewModel(IDataService<ClientEmployee> dataService)
        {
            _dataService = dataService;
            CreateCommand = new AsyncRelayCommand(CreateCommandExecute);
            EditCommand = new AsyncRelayCommand(EditCommandExecute, EditCommandCanExecute);
            LoadCommand = new AsyncRelayCommand(LoadCommandExecute);
            DeleteCommand = new AsyncRelayCommand(DeleteCommandExecute, DeleteCommandCanExecute);
        }
        #endregion

        #region Public Properties
        public ObservableCollection<ClientEmployee> Employees { get; } = new ObservableCollection<ClientEmployee>();

        public ClientEmployee? SelectedEmployee
        {
            get => selectedEmployee;
            set => SetProperty(
                selectedEmployee,
                value,
                callback: (emp) =>
                {
                    selectedEmployee = emp;
                    EditCommand.NotifyCanExecuteChanged();
                    DeleteCommand.NotifyCanExecuteChanged();
                });
        }

        public string StatusText { get => statusText; set => SetProperty(ref statusText, value); }
        #endregion

        #region CreateCommand
        public AsyncRelayCommand CreateCommand { get; }

        private async Task CreateCommandExecute()
        {
            var dlg = new DialogView();
            dlg.Title = "Создание";
            if (dlg.ShowDialog() ?? false)
            {
                try
                {
                    StatusText = "Создание";
                    var employee = GetEmployee(dlg);
                    var createdEmployee = await _dataService.Create(employee);
                    if (createdEmployee != null)
                    {
                        Employees.Add(createdEmployee);
                    }
                    StatusText = STR_READY;
                }
                catch (Exception ex)
                {
                    Logger.Error(ex);
                    StatusText = SetErrorStatus(ex);
                }
            }
        }
        #endregion

        #region EditCommand
        public AsyncRelayCommand EditCommand { get; }

        private async Task EditCommandExecute()
        {
            var dlg = new DialogView();
            dlg.Title = "Редактирование";
            dlg.FirstName.Text = SelectedEmployee?.FirstName;
            dlg.LastName.Text = SelectedEmployee?.LastName;
            dlg.MiddleName.Text = SelectedEmployee?.MiddleName;
            dlg.Birthday.Text = SelectedEmployee?.Birthday?.ToShortDateString();
            if (dlg.ShowDialog() ?? false)
            {
                try
                {
                    StatusText = "Редактирование";
                    var employee = GetEmployee(dlg);
                    var updatedEmployee = await _dataService.Update(employee);
                    var found = Employees.FirstOrDefault(x => x.Id == updatedEmployee?.Id);
                    if (found != null)
                    {
                        int i = Employees.IndexOf(found);
                        Employees[i] = updatedEmployee ?? Employees[i];
                    }
                    StatusText = STR_READY;
                }
                catch (Exception ex)
                {
                    Logger.Error(ex);
                    StatusText = SetErrorStatus(ex);
                }
            }
        }

        private bool EditCommandCanExecute() { return SelectedEmployee != null; }
        #endregion

        #region DeleteCommand
        public AsyncRelayCommand DeleteCommand { get; }

        private async Task DeleteCommandExecute()
        {
            if (SelectedEmployee != null)
            {
                try
                {
                    StatusText = "Удаление";
                    var employee = await _dataService.Delete(SelectedEmployee);
                    if (employee != null)
                    {
                        Employees.Remove(SelectedEmployee);
                    }
                    StatusText = STR_READY;
                }
                catch (Exception ex)
                {
                    Logger.Error(ex);
                    StatusText = SetErrorStatus(ex);
                }
            }
        }

        private bool DeleteCommandCanExecute() { return SelectedEmployee != null; }
        #endregion

        #region LoadCommand
        public AsyncRelayCommand LoadCommand { get; }

        private async Task LoadCommandExecute()
        {
            for (var i = 0; i < 3; i++)
            {
                try
                {
                    StatusText = "Загрузка";
                    Employees.Clear();
                    await foreach (var employee in _dataService.Get())
                    {
                        Employees.Add(employee);
                    }
                    StatusText = STR_READY;
                    break;
                }
                catch (Exception ex)
                {
                    Logger.Error(ex);
                    StatusText = SetErrorStatus(ex);
                }
            }
        }
        #endregion

        #region Private Methods
        private ClientEmployee GetEmployee(DialogView dlg)
        {
            return new ClientEmployee()
            {
                FirstName = dlg.FirstName.Text,
                LastName = dlg.LastName.Text,
                MiddleName = dlg.MiddleName.Text,
                Sex =
                    string.IsNullOrEmpty(dlg.Sex.SelectedItem?.ToString())
                        ? null
                        : (Common.Enums.Sex)(dlg.Sex.SelectedIndex + 1),
                HaveChildren = dlg.HaveChildren.IsChecked,
                Birthday = DateTime.TryParse(dlg.Birthday.Text, out var date) ? date : null,
                Id = SelectedEmployee?.Id ?? 0
            };
        }

        private string SetErrorStatus(Exception ex)
        {
            return $"Error: {ex?.Message}";
        }
        #endregion
    }
}
