namespace TelaCadastroLocatem.Views;

public partial class LocatarioPage : ContentPage

{
    // Campo privado (variável) para controlar o estado da visibilidade da senha.
    // 'false' significa que a senha começa escondida (com asteriscos).
    private bool _showPassword = false;

    // Construtor da página. É executado quando a página é criada.
    public LocatarioPage()
    {
        // Método essencial: carrega o layout visual definido no arquivo XAML (MainPage.xaml)
        // e conecta os elementos (como 'SenhaEntry', 'EmailEntry', etc.) a esta classe.
        InitializeComponent();

        // NOTA: Se esta View estivesse usando o padrão MVVM,
        // a conexão com o ViewModel (visto no código anterior) seria feita aqui,
        // geralmente com a linha:
        // this.BindingContext = new LocatemCadastroLocatario.ViewsModel.CadastroViewModel();
    }


    // Método (event handler) chamado quando o usuário clica no botão/ícone de "mostrar/esconder senha".
    // 'async void' é comum para event handlers, mas 'async' não é estritamente necessário aqui,
    // já que não há operações 'await'. Poderia ser 'private void'.
    private async void OnTogglePassword(object sender, EventArgs e)
    {
        // Inverte o valor booleano. Se era 'false', vira 'true'; se era 'true', vira 'false'.
        _showPassword = !_showPassword;

        // 'SenhaEntry' é o nome (x:Name) do campo de senha no XAML.
        // A propriedade 'IsPassword' define se o texto é mascarado (true) ou mostrado (false).
        // Aqui, ele define a propriedade para o *oposto* do estado atual.
        // (Se _showPassword é true, IsPassword vira false -> mostra a senha).
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

    // Método (event handler) chamado quando o botão de "Login" (ou "Cadastrar") é clicado.
    // 'async' é usado porque ele chama 'DisplayAlert', que é uma operação assíncrona.
    private async void OnLoginClicked(object sender, EventArgs e)
    {
        // Pega o texto de cada campo de entrada (Entry) da tela.
        // 'NomeEntry', 'EmailEntry', etc., são os nomes (x:Name) dos elementos no XAML.
        // O '?' (null-conditional operator) evita erro se o 'Text' for nulo.
        // '.Trim()' remove espaços em branco do início e do fim do texto.
        var nome = NomeEntry.Text?.Trim();
        var email = EmailEntry.Text?.Trim();
        var senha = SenhaEntry.Text?.Trim();
        var cpf = CpfEntry.Text?.Trim();
        var endereco = EnderecoEntry.Text?.Trim();

        // Verifica se os campos obrigatórios estão vazios
        // se (email = nulo/vazio OU senha = nula/vazia OU cnpj = nula/vazia)
        if (string.IsNullOrEmpty(nome) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(senha) || string.IsNullOrEmpty(cpf) || string.IsNullOrEmpty(endereco))
        {
            // Exibe uma mensagem de erro e interrompe o processo de login
            await DisplayAlert("Erro", "Por favor, preencha todos os campos.", "OK");
            return;
        }

        // =========================================================
        // 🚨 ADIÇÃO DO USUÁRIO TESTE 🚨
        // =========================================================
        const string TEST_NOME = "Teste";
        const string TEST_EMAIL = "teste@locatem.com";
        const string TEST_CPF = "12345678901";
        const string TEST_ENDERECO = "Rua teste 34";
        const string TEST_SENHA = "Senha@123";

        // Verifica se os dados inseridos correspondem ao usuário de teste
        if (nome.Equals(TEST_NOME, StringComparison.OrdinalIgnoreCase) &&
            endereco.Equals(TEST_ENDERECO, StringComparison.OrdinalIgnoreCase) && // Incluído
            email.Equals(TEST_EMAIL, StringComparison.OrdinalIgnoreCase) &&
            senha.Equals(TEST_SENHA) &&
            cpf.Equals(TEST_CPF))
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

    // Método (event handler) chamado quando o usuário clica para ver os "Termos de Uso".
    // 'async' é necessário por causa das operações de arquivo (await FileSystem e ReadToEndAsync).
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

    private async void OnIrParaLocadorClicked(object sender, EventArgs e)
    {
        // Navega para a página LocadorPage usando o Shell do MAUI.
        await Shell.Current.GoToAsync("LocadorPage");
    }

    private async void VoltarParaLoginClicked(object sender, EventArgs e)
    {
        // Navega diretamente para a rota de login
        await Shell.Current.GoToAsync("//LoginPage");
    }

    //private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    //{

    //}
}
