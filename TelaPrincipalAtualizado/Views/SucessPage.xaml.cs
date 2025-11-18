namespace TelaPrincipalAtualizado.Views;

public partial class SucessPage : ContentPage
{
	public SucessPage()
	{
		InitializeComponent();
	}

    private async void VoltarClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//MainPage");
    }
}