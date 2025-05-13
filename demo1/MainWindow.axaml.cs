using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using demo1.Models;
using Microsoft.EntityFrameworkCore;

namespace demo1;

public partial class MainWindow : Window
{
    public List<Worker> workers = HelperDB.context.Workers.Include(w=> w.WorkerPostNavigation).ToList();
    public List<WorkerEnterDate> workerEnterDates= HelperDB.context.WorkerEnterDates.ToList();
    
    public MainWindow()
    {
        InitializeComponent();
    }

    private void SignButton_OnClick(object? sender, RoutedEventArgs e)
    {
        var findornot = workers.FirstOrDefault(x => x.WorkerLogin == LoginTextBox.Text);
        var password = PasswordTextBox.Text;
        if (findornot != null)
        {
        
                if (findornot.WorkerPassword == password &&
                    findornot.WorkerPostNavigation?.PostName == "Продавец")
                { // окно продавца
                    SellerWindow window = new SellerWindow(findornot);
                    window.Show();
                    Close();
                    HelperDB.context.WorkerEnterDates.Add(new WorkerEnterDate
                    {WorkerId = findornot.WorkerId, 
                        WorkerEnterType = "Успешно", 
                        WorkerLastEnter = DateTime.Now,
                        EnterId = workerEnterDates.Count + 1,
                            
                    });
                    HelperDB.context.SaveChanges();
                }
                else if (findornot.WorkerPassword.Equals(PasswordTextBox.Text) &&
                    findornot.WorkerPostNavigation?.PostName == "Администратор")
                { //окно администратора
                    AdminWindow window = new AdminWindow();
                    window.Show();
                    Close();
                    HelperDB.context.WorkerEnterDates.Add(new WorkerEnterDate
                    {WorkerId = findornot.WorkerId, 
                        WorkerEnterType = "Успешно", 
                        WorkerLastEnter = DateTime.Now,
                        EnterId = workerEnterDates.Count + 1
                            
                    });
                    HelperDB.context.SaveChanges();
                }
                else if (findornot.WorkerPassword.Equals(PasswordTextBox.Text) &&
                         findornot.WorkerPostNavigation?.PostName == "Старший смены")
                { // окно старшей смены
                    
                    throw new System.NotImplementedException();
                    HelperDB.context.WorkerEnterDates.Add(new WorkerEnterDate
                    {WorkerId = findornot.WorkerId, 
                        WorkerEnterType = "Успешно", 
                        WorkerLastEnter = DateTime.Now,
                        EnterId = workerEnterDates.Count + 1,
                            
                    });
                    HelperDB.context.SaveChanges();
                }
                else
                {
                    Error_TextBlock.Text = "Неправильный пароль";
                    HelperDB.context.WorkerEnterDates.Add(new WorkerEnterDate
                        {WorkerId = findornot.WorkerId, 
                            WorkerEnterType = "Неуспешно", 
                            WorkerLastEnter = DateTime.Now,
                            EnterId = workerEnterDates.Count + 1,
                            
                        });
                    HelperDB.context.SaveChanges();
            
                }
        }
        else
        {
            Error_TextBlock.Text = "Неправильный логин";
        }
        // SellerWindow window = new SellerWindow();
        // window.Show();
        // Close();



        
    }

    private bool _isPasswordVisible = false;
    
    private void ShowPassword_OnClick(object? sender, RoutedEventArgs e)
    {
        _isPasswordVisible = !_isPasswordVisible;
        if (_isPasswordVisible)
            PasswordTextBox.PasswordChar = '\0';
        else 
            PasswordTextBox.PasswordChar = '*';
    }
}