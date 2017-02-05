﻿using System;
using System.Collections.Generic;

namespace ConsoleCards
{
    class Dealer
    {
        public int Position;
        public Deck dealerDeck;

        public Dealer(int _position)
        {
            Position = _position;
            //CreateDeck();
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

            GameLoop.PlayersHaveCards = true;

            for (int i = 0; i < numOfCards; i++)
            {
                for (int playerId = 0; playerId < allPlayers.Count; playerId++)
                {
                    Card card = dealerDeck.Cards[0];
                    dealerDeck.Cards.RemoveAt(0);
                    allPlayers[playerId].Hand.Add(card);
                    allPlayers[playerId].hasCards = true;
                }
            }
        }

    }

}

