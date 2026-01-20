using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using TelaPrincipalAtualizado.Models;

// Alias para evitar conflito com Microsoft.Maui.ApplicationModel.Communication.Contact
using Contact = TelaPrincipalAtualizado.Models.Contact;

namespace TelaPrincipalAtualizado.ViewModels
{
    public class ChatViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        void OnPropertyChanged([CallerMemberName] string? name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        // ID do usuário atual (simulado)
        private const int CurrentUserId = 0;
        public int CurrentUserIdValue => CurrentUserId;

        // Todas as mensagens do sistema (repositório)
        private ObservableCollection<ChatMessage> _allMessages;

        // Mensagens filtradas para a conversa atual
        private ObservableCollection<ChatMessage> _filteredMessages;
        public ObservableCollection<ChatMessage> Messages
        {
            get => _filteredMessages;
            set
            {
                _filteredMessages = value;
                OnPropertyChanged();
            }
        }

        // Lista de contatos
        public ObservableCollection<Contact> Contacts { get; set; }

        // Contato selecionado
        private Contact? _selectedContact;
        public Contact? SelectedContact
        {
            get => _selectedContact;
            set
            {
                if (_selectedContact != value)
                {
                    // Desmarca o anterior
                    if (_selectedContact != null)
                        _selectedContact.IsSelected = false;

                    _selectedContact = value;

                    // Marca o novo
                    if (_selectedContact != null)
                        _selectedContact.IsSelected = true;

                    OnPropertyChanged();
                    OnPropertyChanged(nameof(SelectedContactName));
                    OnPropertyChanged(nameof(IsContactOnline));
                    OnPropertyChanged(nameof(OnlineStatusText));
                    OnPropertyChanged(nameof(SelectedContactAvatar));
                    OnPropertyChanged(nameof(SelectedContactRole));

                    // Filtra mensagens para o contato selecionado
                    FilterMessagesForSelectedContact();
                }
            }
        }

        public string SelectedContactName => SelectedContact?.Name ?? "Selecione um contato";
        public bool IsContactOnline => SelectedContact?.IsOnline ?? false;
        public string OnlineStatusText => IsContactOnline ? "Online" : "Offline";
        public string SelectedContactAvatar => SelectedContact?.DisplayAvatar ?? "usuario.png";
        public string SelectedContactRole => SelectedContact?.RoleDescription ?? "";

        // Texto da mensagem
        private string _messageText = string.Empty;
        public string MessageText
        {
            get => _messageText;
            set
            {
                _messageText = value;
                OnPropertyChanged();
            }
        }

        // Comandos
        public ICommand SendMessageCommand { get; }
        public ICommand SelectContactCommand { get; }

        private int _messageIdCounter = 1;

        public ChatViewModel()
        {
            // Inicializa contatos com dados de exemplo
            Contacts = new ObservableCollection<Contact>
            {
                new Contact(1, "Marcos", true, UserRole.Locador) 
                { 
                    LastMessageTime = DateTime.Now.Date.AddHours(11).AddMinutes(29),
                    AvatarSource = "usuario.png"
                },
                new Contact(2, "Junior", false, UserRole.Locatario) 
                { 
                    LastMessageTime = DateTime.Now.Date.AddHours(11).AddMinutes(29),
                    AvatarSource = "usuario.png"
                },
                new Contact(3, "Léo", true, UserRole.Locador) 
                { 
                    LastMessageTime = DateTime.Now.Date.AddHours(11).AddMinutes(29),
                    AvatarSource = "usuario.png"
                }
            };

            // Repositório central de todas as mensagens
            _allMessages = new ObservableCollection<ChatMessage>
            {
                // Conversa com Marcos (ID: 1)
                new ChatMessage
                {
                    Id = _messageIdCounter++,
                    Content = "Olá! Tudo bem?",
                    IsFromCurrentUser = false,
                    SenderName = "Marcos",
                    SenderId = 1,
                    RecipientId = CurrentUserId,
                    Timestamp = DateTime.Now.AddMinutes(-10)
                },
                new ChatMessage
                {
                    Id = _messageIdCounter++,
                    Content = "Preciso alugar uma furadeira para amanhã.",
                    IsFromCurrentUser = false,
                    SenderName = "Marcos",
                    SenderId = 1,
                    RecipientId = CurrentUserId,
                    Timestamp = DateTime.Now.AddMinutes(-8)
                },
                new ChatMessage
                {
                    Id = _messageIdCounter++,
                    Content = "Olá Marcos! Tudo ótimo e você?",
                    IsFromCurrentUser = true,
                    SenderName = "Eu",
                    SenderId = CurrentUserId,
                    RecipientId = 1,
                    Timestamp = DateTime.Now.AddMinutes(-5)
                },
                new ChatMessage
                {
                    Id = _messageIdCounter++,
                    Content = "Tenho disponível sim!",
                    IsFromCurrentUser = true,
                    SenderName = "Eu",
                    SenderId = CurrentUserId,
                    RecipientId = 1,
                    Timestamp = DateTime.Now.AddMinutes(-4)
                },
                new ChatMessage
                {
                    Id = _messageIdCounter++,
                    Content = "Pode retirar amanhã às 9h.",
                    IsFromCurrentUser = true,
                    SenderName = "Eu",
                    SenderId = CurrentUserId,
                    RecipientId = 1,
                    Timestamp = DateTime.Now.AddMinutes(-3)
                },

                // Conversa com Junior (ID: 2)
                new ChatMessage
                {
                    Id = _messageIdCounter++,
                    Content = "Oi! Vi que você tem uma serra circular disponível.",
                    IsFromCurrentUser = false,
                    SenderName = "Junior",
                    SenderId = 2,
                    RecipientId = CurrentUserId,
                    Timestamp = DateTime.Now.AddMinutes(-30)
                },
                new ChatMessage
                {
                    Id = _messageIdCounter++,
                    Content = "Sim, está disponível! Quer alugar?",
                    IsFromCurrentUser = true,
                    SenderName = "Eu",
                    SenderId = CurrentUserId,
                    RecipientId = 2,
                    Timestamp = DateTime.Now.AddMinutes(-25)
                },

                // Conversa com Léo (ID: 3)
                new ChatMessage
                {
                    Id = _messageIdCounter++,
                    Content = "Bom dia! A lixadeira ainda está disponível?",
                    IsFromCurrentUser = false,
                    SenderName = "Léo",
                    SenderId = 3,
                    RecipientId = CurrentUserId,
                    Timestamp = DateTime.Now.AddHours(-2)
                }
            };

            // Inicializa coleção filtrada vazia
            _filteredMessages = new ObservableCollection<ChatMessage>();

            // Seleciona o primeiro contato por padrão
            SelectedContact = Contacts[0];

            // Configura comandos
            SendMessageCommand = new Command(ExecuteSendMessage, CanSendMessage);
            SelectContactCommand = new Command<Contact>(ExecuteSelectContact);
        }

        /// <summary>
        /// Filtra as mensagens para exibir apenas a conversa com o contato selecionado
        /// </summary>
        private void FilterMessagesForSelectedContact()
        {
            if (SelectedContact == null)
            {
                Messages = new ObservableCollection<ChatMessage>();
                return;
            }

            int contactId = SelectedContact.Id;

            // Filtra mensagens onde o usuário atual é remetente OU destinatário
            // E o contato selecionado é o outro participante
            var filtered = _allMessages
                .Where(m => 
                    (m.SenderId == CurrentUserId && m.RecipientId == contactId) ||
                    (m.SenderId == contactId && m.RecipientId == CurrentUserId))
                .OrderBy(m => m.Timestamp)
                .ToList();

            Messages = new ObservableCollection<ChatMessage>(filtered);
        }

        private bool CanSendMessage()
        {
            return !string.IsNullOrWhiteSpace(MessageText) && SelectedContact != null;
        }

        private void ExecuteSendMessage()
        {
            if (string.IsNullOrWhiteSpace(MessageText) || SelectedContact == null)
                return;

            var newMessage = new ChatMessage
            {
                Id = _messageIdCounter++,
                Content = MessageText.Trim(),
                IsFromCurrentUser = true,
                SenderName = "Eu",
                SenderId = CurrentUserId,
                RecipientId = SelectedContact.Id,
                Timestamp = DateTime.Now
            };

            // Adiciona ao repositório central
            _allMessages.Add(newMessage);

            // Adiciona à lista filtrada (já que é para o contato atual)
            Messages.Add(newMessage);

            // Limpa o campo de texto
            MessageText = string.Empty;

            // Atualiza o horário da última mensagem do contato
            SelectedContact.LastMessageTime = DateTime.Now;
        }

        private void ExecuteSelectContact(Contact? contact)
        {
            if (contact != null)
            {
                SelectedContact = contact;
            }
        }
    }
}
