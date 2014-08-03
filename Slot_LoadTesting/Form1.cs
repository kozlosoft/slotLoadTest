using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Web;
using System.Windows.Forms;

namespace Slot_LoadTesting
{
    public partial class Form1 : Form
    {
        private const string ApiVersion = "5.23";
        private const int HowManyMlsSleepBetweenMessages = 2000;
        private const string SlotId = "169143693";
        private readonly string _appId = File.ReadAllText("client_id.txt");
        private const string LogFileName = "log.txt";
        private volatile bool _stopSendMessages = false;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LogIn();
        }

        private void LogIn()
        {
            var url = string.Format(@"https://oauth.vk.com/authorize?client_id={0}&scope={1}&redirect_uri={2}&display={3}&v={4}&response_type=token",
                _appId,
                "messages",
                @"https://oauth.vk.com/blank.html",
                "page",
                ApiVersion);
            webBrowser.Navigate(url);
            webBrowser.DocumentCompleted += (sender, args) =>
            {
                try
                {
                    var comeBackUrl = ((WebBrowser)sender).Url.ToString().Split('#')[1];
                    AccessTokenTextBox.Text = HttpUtility.ParseQueryString(comeBackUrl).Get("access_token");

                    SendMessages();
                }
                catch
                {
                    AccessTokenTextBox.Text = "not given yet";
                }
            };
        }

        private void SendMessages()
        {
            while (true)
            {
                if (_stopSendMessages)
                    return;

                var url =
                    string.Format(
                        @"https://api.vk.com/method/messages.send?user_id={0}&message={1}&v={2}&access_token={3}",
                        SlotId,
                        GenerateMessageForSlot(),
                        ApiVersion,
                        AccessTokenTextBox.Text);

                using (var logger = new Logger(null, logRichTextBox))
                {
                    var request = (HttpWebRequest)WebRequest.Create(url);
                    request.Timeout = 5000;
                    try
                    {
                        using (WebResponse response = request.GetResponse())
                        {
                            string responseString = ReadFully(response.GetResponseStream());
                            logger.WriteLine(responseString);
                        }
                    }
                    catch (WebException ex)
                    {
                        logger.WriteLine(ex.Message);
                    }
                }

                Application.DoEvents();
                Thread.Sleep(HowManyMlsSleepBetweenMessages);
            }
        }

        private string GenerateMessageForSlot()
        {
            int messageCode = new Random().Next(0, 5);
            string answer = "";
            switch (messageCode)
            {
                case 0:
                    answer = "аниму";
                    break;
                case 1:
                    answer = "стихи";
                    break;
                case 2:
                    answer = "переведи с ru на en лорем испум долор сит амет";
                    break;
                case 3:
                    answer = "ролл твой самый нагруженный по ресурам метод стихи?";
                    break;
                case 4:
                    answer = "мегаслот";
                    break;
            }
            answer += " " + new Random().Next(int.MinValue, int.MaxValue);
            return answer;
        }

        private string ReadFully(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return System.Text.Encoding.Default.GetString(ms.ToArray());
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            _stopSendMessages = true;
        }
    }
}
