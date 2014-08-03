using System;
using System.IO;
using System.Windows.Forms;

namespace Slot_LoadTesting
{
    public class Logger : IDisposable
    {
        private readonly StreamWriter _streamWriter;
        private readonly RichTextBox _richTextBox;
        private bool _doNotTouchDisk = false;

        public Logger(string logFileName = null, RichTextBox richTextBox = null)
        {
            if (logFileName != null)
                _streamWriter = new StreamWriter(logFileName, true);
            else
                _doNotTouchDisk = true;

            if (richTextBox != null)
                _richTextBox = richTextBox;
            else
                _richTextBox = new RichTextBox();
        }


        public void Dispose()
        {
            if (!_doNotTouchDisk)
            {
                _streamWriter.Close();
                _streamWriter.Dispose();
            }
        }

        public void WriteLine(string message)
        {
            if (!_doNotTouchDisk)
                _streamWriter.WriteLine(message);

            _richTextBox.Text += message + Environment.NewLine;
            if (_richTextBox.Text.Split('\n').Length > 10)
                _richTextBox.Text = _richTextBox.Text.Remove(0, _richTextBox.Text.IndexOf('\n') + 1);
        }
    }
}