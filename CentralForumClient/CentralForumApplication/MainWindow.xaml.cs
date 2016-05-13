using System.Windows;
using CentralForumApplication.ViewModel;

namespace CentralForumApplication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public bool IsVisible = true;

        /// <summary>
        /// Initializes a new instance of the MainWindow class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            Closing += (s, e) => ViewModelLocator.Cleanup();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            CentralForum.Visibility = IsVisible ? Visibility.Hidden : Visibility.Visible;

            IsVisible = !IsVisible;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            CentralForum.Visibility = Visibility.Hidden;
        }
    }
}