using System;
using System.Collections.Generic;

namespace Gluttony.DataTransfer
{
    public class Response
    {
        private readonly ExceptionMessagesOption exceptionMessagesOption;
        public bool Result { get; set; }
        public int ErrorCount { get; set; }
        public List<string> Messages { get; private set; }
        public string MinMessages
        {
            get
            {
                string message = string.Empty;
                foreach(string msg in Messages)
                    message += $"{msg} ";

                return message;
            }
        }
        
        public Response(ExceptionMessagesOption option = ExceptionMessagesOption.ExceptionNameAndMessage)
        {
            exceptionMessagesOption = option;
            Messages = new();
            Result = false;
            ErrorCount = 0;
        }

        public void AddMessage(params string[] msg)
        {
            foreach (string m in msg)
                Messages.Add($"[{Messages.Count}] {m}");
        }

        public void AddMessage(Exception e)
        {
            if (e == null)
                return;

            Exception ne;

            do
            {
                AddMessage(ExceptionMessageBreakDown(e));
                ne = e.InnerException;
                ErrorCount++;
            } while (ne == null);

        }

        private string[] ExceptionMessageBreakDown(Exception e)
        {
            if (e == null)
                return null;

            return exceptionMessagesOption switch
            {
                ExceptionMessagesOption.ExceptionNameOnly => new string[] { $"Exception: {e.GetType().FullName}" },
                ExceptionMessagesOption.MessageOnly => new string[] { $"Message {e.Message}" },
                ExceptionMessagesOption.StackTraceOnly => new string[] { $"StackTrace {e.StackTrace}" },
                ExceptionMessagesOption.ExceptionNameAndMessage => new string[] { $"Exception: {e.GetType().FullName}", $"Message {e.Message}" },
                ExceptionMessagesOption.ExceptionNameAndStackTrace => new string[] { $"Exception: {e.GetType().FullName}", $"StackTrace {e.StackTrace}" },
                ExceptionMessagesOption.MessageAndStackTrace => new string[] { $"Message {e.Message}", $"StackTrace {e.StackTrace}" },
                ExceptionMessagesOption.All => new string[] { $"Exception: {e.GetType().FullName}", $"Message {e.Message}", $"StackTrace {e.StackTrace}" },
                _ => null,
            };
        }

    }
}
