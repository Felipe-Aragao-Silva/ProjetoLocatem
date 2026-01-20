namespace TelaPrincipalAtualizado.Views;

public partial class ChatPage : ContentPage
{
    public ChatPage()
    {
        InitializeComponent();
    }

    private async void OnMenuTapped(object sender, TappedEventArgs e)
    {
        // Navega de volta ou abre menu
        if (Navigation.NavigationStack.Count > 1)
        {
            await Navigation.PopAsync();
        }
    }
}
