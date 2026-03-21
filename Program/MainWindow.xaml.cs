using System;
using System.Runtime.InteropServices; // Потрібно для роботи з Windows API
using System.Windows;
using System.Windows.Interop; // Потрібно для отримання Handle вікна
using Program.ViewModels;

namespace Program
{
    public partial class MainWindow : Window
    {
        public MainWindow(MainViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;

            // Підписуємося на подію завантаження вікна, щоб змінити колір
            this.Loaded += MainWindow_Loaded;
        }

        // --- МАГІЯ ДЛЯ ТЕМНОГО ЗАГОЛОВКА ---
        // Імпортуємо функцію з Windows API
        [DllImport("dwmapi.dll", PreserveSig = true)]
        private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);

        // Код атрибута для темного режиму (працює на Windows 10 версії 1903+ та Windows 11)
        private const int DWMWA_USE_IMMERSIVE_DARK_MODE = 20;

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                // Отримуємо системний ідентифікатор нашого вікна (Handle)
                IntPtr handle = new WindowInteropHelper(this).Handle;

                // Вмикаємо темний режим (1 = True)
                int darkMode = 1;
                DwmSetWindowAttribute(handle, DWMWA_USE_IMMERSIVE_DARK_MODE, ref darkMode, sizeof(int));
            }
            catch
            {
                
            }
        }
    }
}