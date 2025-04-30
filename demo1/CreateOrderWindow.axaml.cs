using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using demo1.Models;

namespace demo1;

public partial class CreateOrderWindow : Window
{
    public List<string> TextBoxValues { get; } = new List<string>();
    
    public List<Order> orders = HelperDB.context.Orders.ToList();
    public List<Client> Clients = HelperDB.context.Clients.ToList();
    public List<Service> Services = HelperDB.context.Services.ToList();
    
    public CreateOrderWindow()
    {
        InitializeComponent();
        SetupAutoCompleteForClients();
        SetupAutoCompleteForOrders();
    }

    private void CreateOrder_OnClick(object? sender, RoutedEventArgs e)
    {
        throw new System.NotImplementedException();
    }


    private void CreateClient_OnClick(object? sender, RoutedEventArgs e)
    {
        throw new System.NotImplementedException();
    }

    private void AddTextBox(object? sender, RoutedEventArgs e)
    {
        var newTextBox = new AutoCompleteBox
        {
            Margin = new Thickness(0, 5, 0, 5),
            Watermark = "Введите услугу"
        };
        
        // Подписываемся на изменение текста
        newTextBox.TextChanged += (s, args) =>
        {
            var index = ServicesStackPanel.Children.IndexOf((Control)s);
            if (index >= 0)
            {
                if (index >= TextBoxValues.Count)
                    TextBoxValues.Add(newTextBox.Text);
                else
                    TextBoxValues[index] = newTextBox.Text;
            }
        };
        
        TextBoxValues.Add(""); // Добавляем пустую строку
        ServicesStackPanel.Children.Add(newTextBox);
    }
    
    private void SetupAutoCompleteForClients()
    {
        // Устанавливаем ItemsSource
        Client_CompleteBox.ItemsSource = Clients;

        // Настраиваем фильтрацию (StartsWith)
        Client_CompleteBox.FilterMode = AutoCompleteFilterMode.Custom;
        Client_CompleteBox.ItemFilter = (searchText, item) =>
        {
            if (item is Client client)
            {
                return
                    client.ClientFirstName?.StartsWith(searchText, StringComparison.OrdinalIgnoreCase) == true ||
                    client.ClientMidName?.StartsWith(searchText, StringComparison.OrdinalIgnoreCase) == true ||
                    client.ClientLastName?.StartsWith(searchText, StringComparison.OrdinalIgnoreCase) == true ||
                    client.ClentPassport.ToString()?.StartsWith(searchText, StringComparison.OrdinalIgnoreCase) == true ||
                    client.ClientAddress?.StartsWith(searchText, StringComparison.OrdinalIgnoreCase) == true ||
                    client.ClientBirthday.ToString()?.StartsWith(searchText, StringComparison.OrdinalIgnoreCase) == true ||
                    client.ClientEmail?.StartsWith(searchText, StringComparison.OrdinalIgnoreCase) == true ||
                    client.ClientId.ToString()?.StartsWith(searchText, StringComparison.OrdinalIgnoreCase) == true ||
                    client.ClientPassword?.StartsWith(searchText, StringComparison.OrdinalIgnoreCase) == true;
                    
            }
            return false;
        };
    }

    private void SetupAutoCompleteForOrders()
    {
        
        OrderNumber_TextBox.GotFocus += (sender, args) => 
        {
            if (string.IsNullOrEmpty(OrderNumber_TextBox.Text))
            {
                OrderNumber_TextBox.Watermark = GetLastOrderNumber().ToString();
            }
        };
        OrderNumber_TextBox.KeyUp += (sender, e) =>
        {
            if (e.Key == Key.Enter && string.IsNullOrEmpty(OrderNumber_TextBox.Text))
            {
                OrderNumber_TextBox.Text = GetLastOrderNumber().ToString();
                e.Handled = true; // Предотвращаем дальнейшую обработку
            }
        };
    }
    
    private int GetLastOrderNumber()
    {
        if (orders.Count == 0) return 1;
        return orders.Max(o => o.OrderId) + 1;
    }
}