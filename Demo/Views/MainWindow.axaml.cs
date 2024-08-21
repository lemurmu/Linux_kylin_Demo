using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Styling;
using QLingScope.ViewModels;

namespace QLingScope;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        _viewModel = new MainWindowsViewModel(this);
        this.DataContext = _viewModel;
    }

    private MainWindowsViewModel _viewModel;

    private void SatrtBtn_OnClick(object? sender, RoutedEventArgs e)
    {
        _viewModel.Start();
    }

    private void StopBtn_OnClick(object? sender, RoutedEventArgs e)
    {
        _viewModel.Stop();
    }
}