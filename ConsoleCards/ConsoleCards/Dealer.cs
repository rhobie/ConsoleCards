using System;
using System.Collections.Generic;

namespace ConsoleCards
{
    class Dealer
    {
        public int position;
        public Deck dealerDeck;

        public Dealer(int _position)
        {
            position = _position;
        }

        public void CreateDeck()
        {
            dealerDeck = new Deck();
        }
        
        public void ShuffleDeck()
        {
            Console.WriteLine("\nSHUFFLING..");
            dealerDeck.Shuffle();
        }
        
        public void Deal(List<NPC> allPlayers, int numOfCards)
        {
            Console.WriteLine("\n DEALER IS DEALING..");

            while (dealerDeck.Cards.Count != 0)
            {
                foreach (var player in allPlayers)
                {
                    if (dealerDeck.Cards.Count != 0)
                    {
                        Card card = dealerDeck.Cards[0];
                        dealerDeck.Cards.RemoveAt(0);
                        player.Hand.Add(card);
                    }
                }
            }
            for (int i = 0; i < allPlayers.Count; i++)
            {
                allPlayers[i].hasCards = true;
            }
        }
    }
}

