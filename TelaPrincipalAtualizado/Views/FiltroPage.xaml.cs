using TelaPrincipalAtualizado.ViewModels;
using Microsoft.Maui.Controls;

namespace TelaPrincipalAtualizado.Views;

public partial class FiltroPage : ContentPage
{
    private readonly HistoricoViewModel _viewModel;

    public FiltroPage(HistoricoViewModel viewModel)
	{
		InitializeComponent();
        _viewModel = viewModel;

        TipoPicker.SelectedItem = _viewModel.TipoSelecionado;
        PrecoSlider.Value = _viewModel.PrecoMaximo;
        PrecoLabel.Text = $"R$ {PrecoSlider.Value:0.00}";

        PrecoSlider.ValueChanged += PrecoSlider_ValueChanged;

    }
    private void PrecoSlider_ValueChanged(object sender, ValueChangedEventArgs e)
    {
        PrecoLabel.Text = $"R$ {e.NewValue:0.00}";
    }

    private void AplicarFiltroButton_Clicked(object sender, EventArgs e)
    {
        _viewModel.TipoSelecionado = TipoPicker.SelectedItem?.ToString() ?? "Todos";
        _viewModel.PrecoMaximo = PrecoSlider.Value;

        _viewModel.AplicarFiltroCommand.Execute(null);

        Navigation.PopModalAsync();
    }

    private void CancelarButton_Clicked(object sender, EventArgs e)
    {
        Navigation.PopModalAsync();
    }

}