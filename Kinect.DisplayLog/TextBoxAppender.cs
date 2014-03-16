using System;
using System.Windows.Controls;
using log4net.Appender;

namespace Kinect.DisplayLog
{
    public class TextBoxAppender : AppenderSkeleton
    {
        private const int TextLength = 10000;
        private TextBox _textBox;
        public TextBox AppenderTextBox
        {
            get
            {
                return _textBox;
            }
            set
            {
                _textBox = value;
            }
        }

        protected override void Append(log4net.Core.LoggingEvent loggingEvent)
        {
            if (_textBox == null) return;
            var text = string.Format("{0}{1}{2}",loggingEvent.RenderedMessage, Environment.NewLine, _textBox.Text);
            if (text.Length > TextLength) text = text.Substring(0, TextLength);
            _textBox.Text = text;
        }
    }
}
