namespace ConsoleCards
{
    class PresidentsAndAssholes
    {
        public static int NpcTotal { get => npcTotal; }
        private static int npcTotal;

        public PresidentsAndAssholes(int rounds, int _npcTotal)
        {
            npcTotal = _npcTotal;

            for (int i = 1; i <= rounds; i++)
            {
                var game = new Game();
            }
        }
    }

}

