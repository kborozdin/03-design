using System;
using System.IO;

namespace battleships
{
    public interface IUserUi
    {
        void WriteLine(string line);
        char ReadKey();
    }

    class UserUi : IUserUi
    {
        private readonly bool isInteractiove;

        public UserUi(bool isInteractiove)
        {
            this.isInteractiove = isInteractiove;
        }

        public void WriteLine(string line)
        {
            if (isInteractiove)
                Console.WriteLine(line);
        }

        public char ReadKey()
        {
            return Console.ReadKey().KeyChar;
        }
    }
}