using System.Collections.Generic;

namespace ConsoleCards
{
    class PresidentsAndAssholes
    {
        public static int npcTotal { get => _npcTotal; }
        private static int _npcTotal;

        public static List<NPC> AllPlayers = new List<NPC>();
        public static List<NPC> PlayerRanking = new List<NPC>();

        public PresidentsAndAssholes(int newRounds, int newNpcTotal)
        {
            _npcTotal = newNpcTotal;

            //Add Players
            for (int i = 0; i < newNpcTotal; i++)
            {
                AllPlayers.Add(new NPC(i));
            }
            //Play Rounds
            for (int i = 1; i <= newRounds; i++)
            {
                var game = new Game();
            }
        }
    }

}

