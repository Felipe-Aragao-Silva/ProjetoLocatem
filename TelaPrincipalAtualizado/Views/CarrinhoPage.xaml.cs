using TelaPrincipalAtualizado.ViewModels;

namespace TelaPrincipalAtualizado.Views;

public partial class CarrinhoPage : ContentPage
{
	public CarrinhoPage()
	{
		InitializeComponent();
        BindingContext = new CarrinhoViewModel();
    }
}