using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleCards
{
    public class NPC
    {
        public int Id;
        public List<Card> Hand { get; set; }
        //public List<Card> Duplicates { get; set; } //change name to card selection later?
        public bool hasCards = false;
        //public bool pass = false;

        public NPC(int _activePlayers)
        {
            Id = _activePlayers + 1;
            Hand = new List<Card>();
            //Duplicates = new List<Card>();
        }

        public void SortCards()
        {
            Hand = Hand.OrderBy(x => x.tier).ToList();
        }
        public void GroupCardsInList(List<Card> list)
        {
            if (Hand.Count != 0)
            {
                foreach (var card in list)
                {
                    card.cardDupCount = list.FindAll(x => x.value == card.value).Count;
                }
            }
        }

        public List<Card> SelectCardFromHand(Card TopDiscard, Card TopRoundCard)
        {
            var cardRef = new List<Card>();

            //FIRST CARD OF GAME:
            if (TopDiscard.name == "none" && TopRoundCard.name == "none") //if first card of the game
            {
                if (Hand.Contains(Hand.Find(x => x.tag == "StartingCard"))) //three of clubs is the starting card
                {
                    cardRef = Hand.FindAll(x => x.value == Hand[0].value);
                    return cardRef;
                }
                else
                {
                    cardRef.Add(new Card());
                    cardRef[0].tag = "NoStartingCard";
                    return cardRef;
                }
            }

            //FIRST CARD OF ROUND:
            if (TopRoundCard.tag == "empty")
            {
                //play all of the lowest card in hand:
                cardRef = Hand.FindAll(x => x.tier == Hand[0].tier);
                //tag card(s) played with how many:
                cardRef[0].cardDupCount = cardRef.Count;
                return cardRef;
            }
            //CAN BEAT CARD:
            if (Hand.Contains(Hand.Find(x => x.tier > TopRoundCard.tier && x.cardDupCount >= TopRoundCard.cardDupCount)))
            {
                int cardsToMatch = TopRoundCard.cardDupCount;
                cardRef.Add(Hand.Find(x => x.tier > TopRoundCard.tier && x.cardDupCount >= TopRoundCard.cardDupCount));

                for (int i = 0; i < cardsToMatch - 1; i++)
                {
                    cardRef.Add(Hand.Find(x => x.value == cardRef[0].value && x.suit != cardRef[0].suit));
                }
                GroupCardsInList(cardRef);
                return cardRef;

            }

            //need to add to this so that NPC can make decisions rather than playing the lowest possible card(s) each hand

            else
            {
                cardRef.Add(new Card());
                cardRef[0].tag = "Pass";
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

