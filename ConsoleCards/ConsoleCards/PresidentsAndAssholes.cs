namespace ConsoleCards
{
    class PresidentsAndAssholes
    {
        public static int npcTotal { get => _npcTotal; }
        private static int _npcTotal;

        public PresidentsAndAssholes(int rounds, int npcTotal)
        {
            _npcTotal = npcTotal;

            for (int i = 1; i <= rounds; i++)
            {
                var game = new Game();
            }
        }
    }

}

