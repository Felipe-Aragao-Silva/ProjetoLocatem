using System;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using System.IO;
using TelaPrincipalAtualizado;
using TelaPrincipalAtualizado.Views;

namespace TelaCadastroLocatem.Views;

public partial class LoginPage : ContentPage
{
    private bool _isPasswordVisible = false;
    public LoginPage()
	{
		InitializeComponent();
	}

    // Lógica para mostrar/ocultar a senha
    // A assinatura do método (public/private, tipo de retorno, nome e argumentos) deve estar correta.
    private void AoClicarParaAlternarVisibilidadeDeSenha(object sender, EventArgs e)
    {
        // Inverte o estado atual
        _isPasswordVisible = !_isPasswordVisible;

        // 1. Alterna a propriedade IsPassword do Entry (usando o x:Name="Senha")
        // Se _isPasswordVisible é TRUE (mostrar), IsPassword deve ser FALSE.
        Senha.IsPassword = !_isPasswordVisible;

        // 2. Alterna a imagem do botão (usando o x:Name="BotaoDeAlternarVisibilidad")
        if (_isPasswordVisible)
        {
            BotaoDeAlternarVisibilidade.Source = "olhoaberto.png";
        }
        else
        {
            BotaoDeAlternarVisibilidade.Source = "olhofechado.png";
        }
    }

    // Botão de entrar
    private async void OnLoginClicked(object sender, EventArgs e)
    {
        var email = Email.Text?.Trim();
        var senha = Senha.Text?.Trim();

        // 1. Validação de campos vazios 
        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(senha))
        {
            await DisplayAlert("Erro", "Por favor, preencha todos os campos.", "OK");
            return;
        }

        // 2. Validação LGPD: Verifica se o CheckBox foi marcado
        // Usa o nome do CheckBox que você definiu no XAML (TermosLGPDCheckBox)
        if (!TermosLGPDCheckBox.IsChecked)
        {
            await DisplayAlert("Atenção", "Para continuar, você deve aceitar os Termos de Uso e a Política de Privacidade.", "OK");
            return;
        }

        //3. Usuário Fixo de Teste
        const string TEST_EMAIL = "teste@locatem.com";
        const string TEST_SENHA = "Senha@123";

        //4. Verifica se as credenciais digitadas correspondem ao usuário de teste
        if (email.Equals(TEST_EMAIL, StringComparison.OrdinalIgnoreCase) && senha.Equals(TEST_SENHA))
        {
            await DisplayAlert("Sucesso", "Login realizado com sucesso!", "OK");
            await Shell.Current.GoToAsync("MainPage");


            return;
        }

        //5. Se falhar em TODAS as regras acima.
        else
        {
            await DisplayAlert("Erro", "Email ou senha invalidos.", "OK");
        }
    }

    //List<ContentPage> listPages = [MainPage, LoginPage, NotificacaoPage];

    //Index = 3;

    //listPages[3]

    // Botão de cadastro (Navegação entre telas)
    private async void CadastroClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("LocatarioPage");
    }


    // Botão/link de Termos De Uso no CheckBox
    private async void TermosDeUsoClicked(object sender, EventArgs e)
    {
        using var stream = await FileSystem.OpenAppPackageFileAsync("termos.txt");
        using var reader = new StreamReader(stream);

        var termos = await reader.ReadToEndAsync();
        await DisplayAlert("Termos de Uso", termos, "OK");
    }

    // Botão/link de Políticas de Uso no CheckBox
    private async void PoliticaDePrivacidadeClicked(object sender, EventArgs e)
    {
        using var stream = await FileSystem.OpenAppPackageFileAsync("PoliticasPrivacidade.txt");
        using var reader = new StreamReader(stream);

        var politica = await reader.ReadToEndAsync();
        await DisplayAlert("Política de Privacidade", politica, "OK");
    }
}