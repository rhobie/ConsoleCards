using System.Collections.Generic;

namespace ConsoleCards
{
    //refactor this out, can initalise these lists in the game loop.
    public static class GameLists
    {
        public static List<NPC> PlayersInRound = new List<NPC>();
        public static List<NPC> SeatingPlan = new List<NPC>();
        public static List<NPC> InactivePlayers = new List<NPC>(); //not implemented but will need when rounds are in

    }

}

