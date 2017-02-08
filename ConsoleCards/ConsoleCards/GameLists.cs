using System.Collections.Generic;

namespace ConsoleCards
{
    //refactor this out, can initalise these lists in the game loop.
    public static class GameLists
    {
        readonly public static List<NPC> AllPlayers = new List<NPC>();
        readonly public static List<NPC> PlayersInRound = new List<NPC>();
        public static List<NPC> SeatingPlan = new List<NPC>();
        readonly public static List<NPC> InactivePlayers = new List<NPC>();
        readonly public static List<NPC> PlayerRanking = new List<NPC>();
        public static List<NPC> PassedPlayers = new List<NPC>();
    }

}

