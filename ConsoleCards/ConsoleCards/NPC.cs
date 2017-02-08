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
        //public bool pass = false;

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
            var cardRef = new Card();

            //FIRST CARD OF GAME:
            if (TopDiscard.name == "none" && TopRoundCard.name == "none") //if first card of the game
            {
                if (Hand.Contains(Hand.Find(x => x.tag == "StartingCard"))) //three of clubs is the starting card
                {
                    cardRef = Hand.Find(x => x.tag == "StartingCard");
                    return cardRef;
                }
                else
                {
                    cardRef.tag = "NoStartingCard";
                    return cardRef;
                }
            }

            //FIRST CARD OF ROUND:
            if (TopRoundCard.tag == "empty")
            {
                //play first card in hand
                cardRef = Hand[0];
                return cardRef;
            }

            //CAN BEAT LAST CARD PLAYED:
            if (Hand.Contains(Hand.Find(x => x.tier > TopRoundCard.tier)))
            {
                //need to cahnge this so that NPC does not just play the next highest card they have..
                //add next lot of decsion making here:;
                cardRef = Hand.Find(x => x.tier > TopRoundCard.tier);
                return cardRef;
            }
            else
            {
                cardRef.tag = "Pass";
                return cardRef;
            }
        }

        public void Pass()
        {

        }

        public void RevealHand()
        {
            Debug.ShowCards("NPC " + Id.ToString(), Hand);
        }
    }

}

