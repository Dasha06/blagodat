using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace demo1;

public partial class SellerWindow : Window
{
    public SellerWindow()
    {
        InitializeComponent();
    }

    private void CreateOrder_OnClick(object? sender, RoutedEventArgs e)
    {
        CreateOrderWindow window = new CreateOrderWindow();
        window.Show();
        Close();
    }
}