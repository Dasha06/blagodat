using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using demo1.Models;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using ZXing;
using ZXing.Common;
using SkiaSharp;
using ZXing.SkiaSharp.Rendering;
using Document = iTextSharp.text.Document;
using Rectangle = iTextSharp.text.Rectangle;

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
        // Данные для штрих-кода (например, номер заказа)
        string orderId = OrderNumber_TextBox.Text;
        string creationTime = DateTime.Now.ToString("ddMMyyyyHHmm");
        var rentalHours = orders.Max(x => x.OrderRentalTime);
        var uniqueCode = new Random();
        string barcodeContent = orderId + creationTime + Convert.ToString(rentalHours) + uniqueCode.Next(100000, 999999);
       
    
        // Параметры штрих-кода (в миллиметрах)
        double heightMm = 25.93;
        double barHeightMm = 22.85;
        double leftQuietZoneMm = 3.63;
        double rightQuietZoneMm = 2.31;
        double digitHeightMm = 2.75;
        double marginBottomMm = 0.165;
        double interDigitGapMm = 0.2;
    
        // Конвертация мм в пиксели (1 мм = 3.78 пикселей при 96 DPI)
        const double mmToPx = 3.78;
        int heightPx = (int)(heightMm * mmToPx);
        int barHeightPx = (int)(barHeightMm * mmToPx);
        int marginsPx = (int)(marginBottomMm * mmToPx);
        int digitHeightPx = (int)(digitHeightMm * mmToPx);
    
        // Создаем штрих-код CODE_128 (можно использовать другие форматы)
        var barcodeWriter = new BarcodeWriter<SKBitmap>
        {
            Format = BarcodeFormat.CODE_128,
            Options = new EncodingOptions
            {
                Height = heightPx,
                Width = 300, // Ширина будет автоматически подобрана
                Margin = (int)(leftQuietZoneMm * mmToPx) // Левая свободная зона
            },
            Renderer = new SKBitmapRenderer()
            
        };
        
    
        // Генерируем изображение штрих-кода
        SKBitmap barcodeBitmap = barcodeWriter.Write(barcodeContent);
    
        byte[] pngBytes;
        using (var image = SKImage.FromBitmap(barcodeBitmap))
        using (var data = image.Encode(SKEncodedImageFormat.Png, 100))
        using (var ms = new MemoryStream())
        {
            data.SaveTo(ms);
            pngBytes = ms.ToArray();
        }

        // 3. Создаем PDF с PDFsharp
        PdfDocument document = new PdfDocument();
        PdfPage page = document.AddPage();
        page.Width = XUnit.FromMillimeter(100);  // Ширина страницы (мм)
        page.Height = XUnit.FromMillimeter(40);  // Высота страницы (мм)

        // Загружаем изображение в XImage
        using (var imgStream = new MemoryStream(pngBytes))
        {
            XImage xImage = XImage.FromStream(imgStream);

            // Рисуем штрих-код на странице PDF
            XGraphics gfx = XGraphics.FromPdfPage(page);
            gfx.DrawImage(xImage, 5, 5, xImage.PointWidth, xImage.PointHeight);
            
            
        }

        // 4. Сохраняем PDF
        string pdfPath = "OrderBarcode.pdf";
        document.Save(pdfPath);
        document.Close();
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