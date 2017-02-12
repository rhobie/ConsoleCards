using System.Collections.Generic;
using System.Linq;

namespace ConsoleCards
{
    public class Ranking
    {
        public static List<NPC> PlayerRanking = new List<NPC>();

        public static void SwapCards()
        {
            foreach (var player in PlayerRanking)
            {
                player.SortCards();
            }

            if (PlayerRanking.Count != 0)
            {
                var lowCards = new List<Card>();
                var highCards = new List<Card>();

                lowCards.Add(GetPresident().Hand[0]);
                lowCards.Add(GetPresident().Hand[1]);

                highCards.Add(GetAsshole().Hand[GetAsshole().Hand.Count - 1]);
                highCards.Add(GetAsshole().Hand[GetAsshole().Hand.Count - 2]);

                GetAsshole().Hand.AddRange(lowCards);
                GetPresident().Hand.AddRange(highCards);

                GetAsshole().Hand = GetAsshole().Hand.Except(highCards).ToList();
                GetPresident().Hand = GetPresident().Hand.Except(lowCards).ToList();               

                Commentary.SwapCards(GetPresident(), lowCards[0], lowCards[1], GetAsshole(), highCards[0], highCards[1]);


                if (GetVicePresident() != null && GetViceAsshole() != null)
                {
                    lowCards.Clear();
                    highCards.Clear();
                    lowCards.Add(GetVicePresident().Hand[0]);
                    highCards.Add(GetViceAsshole().Hand[GetViceAsshole().Hand.Count - 1]);

                    GetViceAsshole().Hand.Add(lowCards[0]);
                    GetVicePresident().Hand.Add(highCards[0]);

                    GetViceAsshole().Hand.Remove(highCards[0]);
                    GetVicePresident().Hand.Remove(lowCards[0]);

                    GetViceAsshole().SortCards();
                    GetVicePresident().SortCards();

                    Commentary.SwapCards(GetVicePresident(), lowCards[0], GetViceAsshole(), highCards[0]);
                }

                foreach (var player in PlayerRanking)
                {
                    player.SortCards();
                    player.GroupCards(player.Hand);
                }
            }
            PlayerRanking.Clear();
        }

        private static NPC GetPresident()
        {
            return PlayerRanking[0];
        }

        private static NPC GetVicePresident()
        {
            if (PlayerRanking.Count >= 4)
            {
                return PlayerRanking[1];
            }
            return null;
        }

        private static NPC GetViceAsshole()
        {
            if (PlayerRanking.Count >= 4)
            {
                return PlayerRanking[PlayerRanking.Count - 2];
            }
            return null;
        }

        private static NPC GetAsshole()
        {
            return PlayerRanking[PlayerRanking.Count - 1];
        }

        public static void RankPlayers()
        {
            foreach (var player in PresidentsAndAssholes.AllPlayers)
            {
                player.Rank = "Neutral";
            }
            GetPresident().Rank = "President";
            GetVicePresident().Rank = "Vice President";
            GetViceAsshole().Rank = "Vice Asshole";
            GetAsshole().Rank = "Asshole";//NPC.Rank can probably be removed entirely..

            GetPresident().Score[0]++;
            GetVicePresident().Score[1]++;
            GetViceAsshole().Score[2]++;
            GetAsshole().Score[3]++;
        }
    }

}

