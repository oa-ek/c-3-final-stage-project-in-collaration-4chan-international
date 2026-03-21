using Microsoft.Extensions.DependencyInjection;
using Models_Context.Models;
using Program.ViewModels;
using Services.Interfaces;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Program
{
    public partial class MenuWindow : Window
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ICharacterBuildService _buildService;

        public MenuWindow(IServiceProvider serviceProvider, ICharacterBuildService buildService)
        {
            InitializeComponent();
            _serviceProvider = serviceProvider;
            _buildService = buildService;

            LoadBuilds();
        }
        private void OnDeleteClick(object sender, RoutedEventArgs e)
        {
            
            if (sender is Button button && button.DataContext is CharacterBuild buildToDelete)
            {
               
                var result = MessageBox.Show(
                    $"Are you sure you want to delete '{buildToDelete.Name}'?",
                    "Delete Character",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        
                        _buildService.DeleteBuild(buildToDelete.Id);

                        
                        LoadBuilds();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error deleting: " + ex.Message);
                    }
                }
            }

            
            e.Handled = true;
        }

        private void LoadBuilds()
        {
            try
            {
               
                var builds = _buildService.GetAllBuilds();
                BuildsList.ItemsSource = builds;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading: " + ex.Message);
            }
        }

        private void OnBuildDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (BuildsList.SelectedItem is CharacterBuild selectedBuild)
            {
                OpenEditor(selectedBuild);
            }
        }

        private void NewCharacter_Click(object sender, RoutedEventArgs e)
        {
            OpenEditor(null);
        }

        private void OpenEditor(CharacterBuild build)
        {
            
            var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
            var viewModel = (MainViewModel)mainWindow.DataContext;

            viewModel.CloseAction = () => mainWindow.Close();

            if (build != null)
            {
                viewModel.LoadBuild(build);
            }

            
            this.Hide();

            
            mainWindow.ShowDialog();

            
            this.Show(); // показати меню знову після закриття редактора білда
            LoadBuilds(); // Оновлюємо квадратики, щоб побачити зміни
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}