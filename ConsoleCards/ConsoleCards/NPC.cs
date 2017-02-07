using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleCards
{
    public class NPC
    {
        public int Id;
        public List<Card> Hand { get; set; }
        public bool hasCards = false;

        public NPC(int _activePlayers)
        {
            Id = _activePlayers + 1;
            Hand = new List<Card>();
        }

        public void SortCards()
        {
            Hand = Hand.OrderBy(x => x.tier).ToList();
        }

        public Card SelectCardFromHand(Card TopDiscard, Card TopRoundCard)
        {
            var cardRef = new Card("");

            if (TopDiscard == null && TopRoundCard == null) //if first card of the game
            {
                if (Hand.Contains(Hand.Find(x => x.tier == 9))) //three of clubs is the starting card
                {
                    cardRef = Hand.Find(x => x.tier == 9);
                }
                else
                {
                    //NPC does not pass before the round has started.
                    cardRef.tag = "PassBeforeRoundStart";
                }
            }
            else if (TopRoundCard != null && Hand.Contains(Hand.Find(x => x.tier > TopRoundCard.tier)))
            {
                //need to cahnge this so that NPC does not just play the next highest card they have..
                //add next lot of decsion making here:;
                cardRef = Hand.Find(x => x.tier > TopRoundCard.tier);
            }
            else if (TopRoundCard == null)
            {
                //play first card in hand
                cardRef = Hand[0];
            }
            else
            {
                //card = new Card();
                cardRef.tag = "Pass";
                Pass();
            }
            return cardRef;
        }

        public void Pass()
        {
            Console.WriteLine("NPC {0} PASSES\n", Id);
            GameLists.PlayersInRound.Remove(this);
        }

        public void RevealHand()
        {
            Debug.ShowCards("NPC " + Id.ToString(), Hand);
        }
    }

}

