using System;
using System.IO;
using System.Windows.Forms;

namespace Slot_LoadTesting
{
    public class Logger : IDisposable
    {
        private readonly StreamWriter _streamWriter;
        private readonly RichTextBox _richTextBox;

        public Logger(string logFileName, RichTextBox richTextBox = null)
        {
            _streamWriter = new StreamWriter(logFileName, true);
            if (richTextBox != null)
                _richTextBox = richTextBox;
            else
                _richTextBox = new RichTextBox();
        }


        public void Dispose()
        {
            _streamWriter.Close();
            _streamWriter.Dispose();
        }

        public void WriteLine(string message)
        {
            _streamWriter.WriteLine(message);
            _richTextBox.Text += message + Environment.NewLine;
        }
    }
}