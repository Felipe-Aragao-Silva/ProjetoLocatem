using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelaCadastroLocatem.Views;

public partial class LocadorPage : ContentPage
{

  // Variável booleana que controla se a senha será mostrada ou escondida
    private bool _showPassword = false;
    public LocadorPage()
    {
        InitializeComponent(); // Inicializa os componentes visuais definidos no arquivo XAML
    }

    // Oculta ou mostra a senha quando o usuário clica no ícone de olho
    private async void OnTogglePassword(object sender, EventArgs e)
    {
        // Inverte o valor da variável _showPassword (de true para false, e vice-versa)
        _showPassword = !_showPassword;

        // Altera a propriedade IsPassword do campo de senha (SenhaEntry)
        // Quando IsPassword = true ? oculta o texto (mostra bolinhas)
        // Quando IsPassword = false ? mostra o texto real digitado
        SenhaEntry.IsPassword = !_showPassword;

        if (_showPassword)
        {
            BotaoDeAlterarVisibilidade.Source = "olhoaberto.png"; // Ícone de olho aberto quando a senha está visível.
        }
        else
        {
            BotaoDeAlterarVisibilidade.Source = "olhofechado.png"; // Ícone de olho fechado quando a senha está escondida.
        }
    }

    // Método executado quando o usuário clica no botão de login
    private async void OnLoginClicked(object sender, EventArgs e)
    {
        // Lê os textos digitados nos campos da tela
        // O ?.Trim() remove espaços extras e evita erro caso o campo esteja nulo
        var nome = NomeEntry.Text?.Trim();
        var email = EmailEntry.Text?.Trim();
        var senha = SenhaEntry.Text?.Trim();
        var cnpj = CnpjEntry.Text?.Trim();
        var endereco = EnderecoEntry.Text?.Trim();

        // Verifica se os campos obrigatórios estão vazios
        // se (email = nulo/vazio OU senha = nula/vazia OU cnpj = nula/vazia)
        if (string.IsNullOrEmpty(nome) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(senha) || string.IsNullOrEmpty(cnpj) || string.IsNullOrEmpty(endereco))
        {
            // Exibe uma mensagem de erro e interrompe o processo de login
            await DisplayAlert("Erro", "Por favor, preencha todos os campos.", "OK");
            return;
        }

        // =========================================================
        // ?? ADIÇÃO DO USUÁRIO TESTE ??
        // =========================================================
        const string TEST_NOME = "Teste";
        const string TEST_EMAIL = "teste@locatem.com";
        const string TEST_CNPJ = "12345678901234";
        const string TEST_ENDERECO = "Rua teste 34";
        const string TEST_SENHA = "Senha@123";

        // Verifica se os dados inseridos correspondem ao usuário de teste
        if (nome.Equals(TEST_NOME, StringComparison.OrdinalIgnoreCase) &&
            endereco.Equals(TEST_ENDERECO, StringComparison.OrdinalIgnoreCase) && // Incluído
            email.Equals(TEST_EMAIL, StringComparison.OrdinalIgnoreCase) &&
            senha.Equals(TEST_SENHA) &&
            cnpj.Equals(TEST_CNPJ))
        {
            // Se a validação for real, aqui você chamaria o serviço para cadastrar o usuário
            await DisplayAlert("Sucesso", $"Usuário de Teste cadastrado com sucesso!", "OK");
            return;
        }
        // =========================================================

        // Simulação de autenticação (Lógica Genérica)
        // Recomendação: No mundo real, todos os campos deveriam ser validados aqui.
        if (email.Contains('@') && senha.Length >= 4)
        {
            // Aqui você usaria as variáveis 'nome' e 'endereco' para salvar os dados
            await DisplayAlert("Sucesso", "Cadastro realizado com sucesso!", "OK");
            return;
        }
        else
        {
            await DisplayAlert("Erro", "Dados inválidos para cadastro. Tente o usuário de teste.", "OK");
        }
    }

    // Método executado quando o usuário clica no botão para ver os Termos de Uso
    private async void TermosDeUsoClicked(object sender, EventArgs e)
    {
        using var stream = await FileSystem.OpenAppPackageFileAsync("termos.txt");
        using var reader = new StreamReader(stream);

        var termos = await reader.ReadToEndAsync();
        await DisplayAlert("Termos de Uso", termos, "OK");
    }

    private async void PoliticaDePrivacidadeClicked(object sender, EventArgs e)
    {
        using var stream = await FileSystem.OpenAppPackageFileAsync("PoliticasPrivacidade.txt");
        using var reader = new StreamReader(stream);

        var politica = await reader.ReadToEndAsync();
        await DisplayAlert("Políticas de Privacidade", politica, "OK");
    }

    private async void OnIrParaLocatarioClicked(object sender, EventArgs e)
    {
        // Navega para a página LocatarioPage usando o Shell do MAUI
        await Shell.Current.GoToAsync(nameof(LocatarioPage));
    }

    private async void VoltarParaLoginClicked(object sender, EventArgs e)
    {
        // Navega diretamente para a rota de login
        await Shell.Current.GoToAsync("//LoginPage");
    }
}
