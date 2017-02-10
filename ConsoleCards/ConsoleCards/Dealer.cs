using System;
using System.Collections.Generic;

namespace ConsoleCards
{
    class Dealer
    {
        public int Position;
        public Deck DealerDeck;

        public Dealer(int _position)
        {
            Position = _position;
        }

        public void CreateDeck()
        {
            DealerDeck = new Deck();
        }
        
        public void ShuffleDeck()
        {
            Commentary.Shuffling();
            DealerDeck.Shuffle();
        }
        
        public void Deal(List<NPC> allPlayers, int numOfCards)
        {
            Commentary.DealerDealing();

            while (DealerDeck.Cards.Count != 0)
            {
                foreach (var player in allPlayers)
                {
                    if (DealerDeck.Cards.Count != 0)
                    {
                        Card card = DealerDeck.Cards[0];
                        DealerDeck.Cards.RemoveAt(0);
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

