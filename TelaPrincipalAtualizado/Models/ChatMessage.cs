using System;

namespace TelaPrincipalAtualizado.Models
{
    public class ChatMessage
    {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
        public bool IsFromCurrentUser { get; set; }
        public string SenderName { get; set; } = string.Empty;
        
        // IDs para filtro de mensagens privadas
        public int SenderId { get; set; }
        public int RecipientId { get; set; }

        // Propriedades computadas para binding no XAML
        public string FormattedTime => Timestamp.ToString("HH:mm");
        
        public LayoutOptions HorizontalAlignment => 
            IsFromCurrentUser ? LayoutOptions.End : LayoutOptions.Start;

        public ChatMessage() 
        {
            Timestamp = DateTime.Now;
        }

        public ChatMessage(int id, string content, bool isFromCurrentUser, string senderName, int senderId, int recipientId)
        {
            Id = id;
            Content = content;
            IsFromCurrentUser = isFromCurrentUser;
            SenderName = senderName;
            SenderId = senderId;
            RecipientId = recipientId;
            Timestamp = DateTime.Now;
        }
    }
}
