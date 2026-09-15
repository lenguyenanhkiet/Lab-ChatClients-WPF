using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ChatClientWPF
{
    public partial class MainWindow : Window
    {
        private TcpClient _client;
        private NetworkStream _stream;

        public MainWindow()
        {
            InitializeComponent();
            ConnectToServer();
        }

        private async void ConnectToServer()
        {
            try
            {
                _client = new TcpClient("127.0.0.1", 5000);
                _stream = _client.GetStream();
                ChatHistory.Items.Add("[Hệ thống] Đã kết nối tới Server thành công!");
                _ = ReceiveMessagesAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể kết nối đến server: " + ex.Message);
            }
        }

        private async Task ReceiveMessagesAsync()
        {
            byte[] buffer = new byte[1024];
            try
            {
                while (true)
                {
                    int bytesRead = await _stream.ReadAsync(buffer, 0, buffer.Length);
                    if (bytesRead == 0) break;

                    string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    Dispatcher.Invoke(() => ChatHistory.Items.Add("Người khác: " + message));
                }
            }
            catch
            {
                Dispatcher.Invoke(() => ChatHistory.Items.Add("[Hệ thống] Mất kết nối đến server."));
            }
        }

        private async void SendMessage()
        {
            if (string.IsNullOrWhiteSpace(MessageInput.Text) || _stream == null) return;

            string message = MessageInput.Text;
            byte[] buffer = Encoding.UTF8.GetBytes(message);
            await _stream.WriteAsync(buffer, 0, buffer.Length);

            ChatHistory.Items.Add("Bạn: " + message);
            MessageInput.Clear();
        }

        private void Send_Click(object sender, RoutedEventArgs e) => SendMessage();

        private void MessageInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) SendMessage();
        }

        private void Emoji_Click(object sender, RoutedEventArgs e)
        {
            if (sender is System.Windows.Controls.Button btn)
            {
                MessageInput.Text += btn.Content.ToString();
                MessageInput.CaretIndex = MessageInput.Text.Length;
                MessageInput.Focus();
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _stream?.Close();
            _client?.Close();
        }
    }
}