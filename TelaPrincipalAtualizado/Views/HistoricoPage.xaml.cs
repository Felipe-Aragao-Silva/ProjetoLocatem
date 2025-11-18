using TelaPrincipalAtualizado.Models;
using TelaPrincipalAtualizado.ViewModels;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Shapes;
using System;
using System.Collections.Generic;

namespace TelaPrincipalAtualizado.Views;

public partial class HistoricoPage : ContentPage
{
    private bool painelAberto = true;
    private HistoricoViewModel ViewModel => BindingContext as HistoricoViewModel;

    public HistoricoPage()
	{
		InitializeComponent();

        // Inicializa os cards
        CarregarCards();

        // Atualiza os cards automaticamente quando as coleções mudarem
        ViewModel.PropertyChanged += (s, e) =>
        {
            if (e.PropertyName == nameof(ViewModel.HistoricoCompra) || e.PropertyName == nameof(ViewModel.Ofertas))
            {
                AtualizarCards();
            }
        };
    }


    private void CarregarCards()
    {
        AtualizarCards();
    }

    private void AtualizarCards()
    {
        // Limpa os layouts
        HistoricoCompraLayout.Children.Clear();
        OfertasLayout.Children.Clear();

        // Recria os cards
        foreach (var item in ViewModel.HistoricoCompra)
            HistoricoCompraLayout.Add(CriarCard(item, ViewModel.HistoricoCompra));

        foreach (var item in ViewModel.Ofertas)
            OfertasLayout.Add(CriarCard(item, ViewModel.Ofertas));
    }

    // Abrir página de filtro
    private async void AbrirFiltro_Clicked(object sender, EventArgs e)
    {
        var filtroPage = new FiltroPage(ViewModel);
        await Navigation.PushModalAsync(filtroPage);
    }

    // Criar card dinamicamente
    private Microsoft.Maui.Controls.Border CriarCard(ItemModel item, IList<ItemModel> lista)
    {
        var border = new Microsoft.Maui.Controls.Border
        {
            BackgroundColor = Colors.White,
            StrokeShape = new RoundRectangle { CornerRadius = 20 },
            WidthRequest = 200,
            Padding = 10,
            Margin = new Thickness(5)
        };

        var stack = new VerticalStackLayout { Spacing = 5 };

        // Imagem do item
        var img = new Microsoft.Maui.Controls.Image
        {
            Source = item.ImageSource,
            HeightRequest = 120,   // define altura fixa para todas as imagens
            WidthRequest = 180,    // largura compatível com o card
            Aspect = Aspect.AspectFill, // preenche o espaço, cortando se necessário
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Start
        };
        stack.Add(img);

        // Dados do item
        stack.Add(new Microsoft.Maui.Controls.Label
        {
            Text = item.Nome,
            FontSize = 16,
            FontAttributes = FontAttributes.Bold,
            TextColor = Colors.Black
        });
        stack.Add(new Microsoft.Maui.Controls.Label
        {
            Text = item.PrecoPorDia,
            FontSize = 14,
            TextColor = Colors.Gray
        });
        stack.Add(new Microsoft.Maui.Controls.Label
        {
            Text = item.Localizacao,
            FontSize = 12,
            TextColor = Colors.DarkGray
        });

        // Botão de excluir
        var btnExcluir = new Microsoft.Maui.Controls.Button
        {
            Text = "Excluir",
            BackgroundColor = Colors.Transparent,
            TextColor = Colors.Red,
            FontAttributes = FontAttributes.Bold,
            Padding = 0,
            HeightRequest = 20
        };
        btnExcluir.Clicked += async (s, e) =>
        {
            bool confirm = await DisplayAlert("Confirmação", $"Deseja excluir {item.Nome}?", "Sim", "Não");
            if (confirm)
            {
                lista.Remove(item);
                if (lista == ViewModel.HistoricoCompra)
                    HistoricoCompraLayout.Children.Remove(border);
                else
                    OfertasLayout.Children.Remove(border);
            }
        };
        stack.Add(btnExcluir);

        border.Content = stack;

        // Adiciona animação de hover
        AdicionarAnimacaoCard(border);

        return border;
    }

    // Animação hover do card
    private void AdicionarAnimacaoCard(Microsoft.Maui.Controls.Border border)
    {
        var corOriginal = Colors.White;
        var corHover = Color.FromArgb("#F2AF0D");

        Color Interpolar(Color from, Color to, double t)
        {
            return new Color(
                (float)(from.Red + (to.Red - from.Red) * t),
                (float)(from.Green + (to.Green - from.Green) * t),
                (float)(from.Blue + (to.Blue - from.Blue) * t),
                (float)(from.Alpha + (to.Alpha - from.Alpha) * t)
            );
        }

        var pointerEnter = new PointerGestureRecognizer();
        pointerEnter.PointerEntered += (s, e) =>
        {
            uint duracao = 300;
            border.Animate("hoverEnter", t => border.BackgroundColor = Interpolar(corOriginal, corHover, t), 16, duracao, Easing.Linear);
        };

        var pointerExit = new PointerGestureRecognizer();
        pointerExit.PointerExited += (s, e) =>
        {
            uint duracao = 300;
            border.Animate("hoverExit", t => border.BackgroundColor = Interpolar(corHover, corOriginal, t), 16, duracao, Easing.Linear);
        };

        border.GestureRecognizers.Add(pointerEnter);
        border.GestureRecognizers.Add(pointerExit);
    }

}