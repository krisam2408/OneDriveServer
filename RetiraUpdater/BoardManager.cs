using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RetiraUpdater
{
    internal class BoardManager
    {
        private List<string> m_messages;
        private bool m_isLoading;
        private Timer m_timer;

        private int LastMessageIndex { get { return m_messages.Count-1; } }
        private string[] MessagesArray { get { return m_messages.ToArray(); } }

        internal BoardManager()
        {
            m_isLoading = false;
            m_messages = new();
        }

        internal void UpdateBoard(string[] messages)
        {
            Console.Clear();
            foreach(string msg in messages)
                Console.WriteLine(msg);
        }

        internal void AddMessage(string msg)
        {
            m_messages.Add(msg);
            UpdateBoard(MessagesArray);
        }

        internal void UpdateLastMessage(string msg)
        {
            SetLastMessage(msg);
            UpdateBoard(MessagesArray);
        }

        private void SetLastMessage(string msg)
        {
            if(m_isLoading)
            {
                m_messages[^2] = msg;
                return;
            }

            m_messages[^1] = msg;
        }

        private void DeleteLastMessage()
        {
            m_messages.RemoveAt(LastMessageIndex);
            UpdateBoard(MessagesArray);
        }

        internal void StartLoading()
        {
            m_isLoading = true;
            m_messages.Add("-");
            m_timer = new(new TimerCallback(Loading));
            m_timer.Change(0, 500);
        }

        internal void StopLoading()
        {
            m_timer.Dispose();
            m_isLoading = false;
            CleanBoard();
        }

        private void CleanBoard()
        {
            List<string> freshBoard = new();
            string[] trashMessages = new string[] { "-", "\\", "|", "/", string.Empty };

            foreach(string msg in m_messages)
                if(!trashMessages.Contains(msg))
                    freshBoard.Add(msg);

            m_messages = freshBoard;
            UpdateBoard(MessagesArray);
        }

        private string GetLastLoadingState()
        {
            string[] loadingMessages = new string[] { "-", "\\", "|", "/" };
            string output = string.Empty;

            foreach (string msg in m_messages)
                if (loadingMessages.Contains(msg))
                    output = msg;

            return output;
        }

        private void Loading(object info)
        {
            string lastChar = GetLastLoadingState();
            CleanBoard();
            string nextChar = lastChar switch
            {
                "-" => "\\",
                "\\" => "|",
                "|" => "/",
                _ => "-"
            };
            AddMessage(nextChar);
        }

    }
}
