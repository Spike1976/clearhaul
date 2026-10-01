using Avalonia.Controls;
using Avalonia.Interactivity;
using ClearHaul.Client.Services;

namespace ClearHaul.Client;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        var productName = ProductBranding.LoadName();
        Title = productName;
        HeadingText.Text = productName;
    }

    private async void OnCheckHealth(object? sender, RoutedEventArgs e)
    {
        using var client = new ServerHealthClient(new HttpClientHandler());
        var result = await client.CheckAsync(AddressBox.Text, CancellationToken.None);
        ResultText.Text = result.Message;
    }
}
