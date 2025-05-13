using System;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using demo1.Models;

namespace demo1;

public partial class SellerWindow : Window
{
    
    
    private readonly TimeSpan sessionDuration = TimeSpan.FromMinutes(10);
    private readonly TimeSpan warningTime = TimeSpan.FromMinutes(5);
    private DateTime sessionStartTime;
    private bool warningShow = false;
    
    public SellerWindow(Worker worker)
    {
        InitializeComponent();

        EmployeeFIOBlock.Text = $"{worker.WorkerFirstName} {worker.WorkerLastName}";
        
        sessionStartTime = DateTime.Now;
        StartSessionTimer();
    }

    private void CreateOrder_OnClick(object? sender, RoutedEventArgs e)
    {
        CreateOrderWindow window = new CreateOrderWindow();
        window.Show();
    }
    
    private async void StartSessionTimer()
    {
        while (true)
        {
            TimeSpan elapsedTime = DateTime.Now - sessionStartTime;
            TimeSpan remainingTime = sessionDuration - elapsedTime;
            
            this.FindControl<TextBlock>("TimerBlock").Text = $"Осталось: {remainingTime.Minutes}:{remainingTime.Seconds}";

            if (!warningShow && remainingTime <= warningTime)
            {
                warningShow = true;
                WarningBlock.Text = "Внимание! Ваш сеанс закончится через 5 минут!";
            }

            if (remainingTime <= TimeSpan.Zero)
            {
                EndSession();
                break;
            }

            await Task.Delay(1000);
        }
    }

    private void EndSession()
    {
        this.Close();
    }
}