using System.Collections.Generic;

namespace ConsoleCards
{
    class PresidentsAndAssholes
    {
        public static int npcTotal { get => _npcTotal; }
        private static int _npcTotal;

        public static List<NPC> AllPlayers = new List<NPC>();
        public static List<NPC> PlayerRanking = new List<NPC>();

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

