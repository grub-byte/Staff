using System.Windows;

namespace ClientStaff.Views
{
    /// <summary>
    /// Логика взаимодействия для DialogView.xaml
    /// </summary>
    public partial class DialogView : Window
    {
        public DialogView()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}
