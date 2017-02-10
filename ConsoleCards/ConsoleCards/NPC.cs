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
            Hand = Hand.OrderBy(x => x.Tier).ToList();
        }
        public void GroupCards(List<Card> hand)
        {
            if (hand.Count != 0)
            {
                foreach (var card in hand)
                {
                    if (card != null)
                    {
                        card.cardDupCount = hand.FindAll(x => x.Value == card.Value).Count;
                    }
                }
            }
        }

        public List<Card> SelectCardFromHand(Card TopDiscard, Card TopRoundCard)
        {
            var cardRef = new List<Card>();

            //FIRST CARD OF GAME:
            if (TopDiscard.Name == "none" && TopRoundCard.Name == "none") //if first card of the game
            {
                if (Hand.Contains(Hand.Find(x => x.Tag == "StartingCard"))) //three of clubs is the starting card
                {
                    cardRef = Hand.FindAll(x => x.Value == Hand[0].Value);
                    return cardRef;
                }
                else
                {
                    cardRef.Add(new Card());
                    cardRef[0].Tag = "NoStartingCard";
                    return cardRef;
                }
            }

            //FIRST CARD OF ROUND:
            if (TopRoundCard.Tag == "empty")
            {
                //play all of the lowest card in hand:
                cardRef = Hand.FindAll(x => x.Tier == Hand[0].Tier);
                //tag card(s) played with how many:
                cardRef[0].cardDupCount = cardRef.Count;
                return cardRef;
            }
            //CAN BEAT CARD:
            if (Hand.Contains(Hand.Find(x => x.Tier > TopRoundCard.Tier && x.cardDupCount >= TopRoundCard.cardDupCount)))
            {
                int cardsToMatch = TopRoundCard.cardDupCount;
                cardRef.Add(Hand.Find(x => x.Tier > TopRoundCard.Tier && x.cardDupCount >= TopRoundCard.cardDupCount));

                for (int i = 0; i < cardsToMatch - 1; i++)
                {
                    cardRef.Add(Hand.Find(x => x.Value == cardRef[0].Value && x.Suit != cardRef[0].Suit));
                }
                GroupCards(cardRef);
                return cardRef;

            }

            //need to add to this so that NPC can make decisions rather than playing the lowest possible card(s) each hand

            else
            {
                cardRef.Add(new Card());
                cardRef[0].Tag = "Pass";
                return cardRef;
            }
        }

        public void Pass()
        {

        }

        public void RevealHand()
        {
            Commentary.ShowCards("NPC " + Id.ToString(), Hand);
        }
    }

}

