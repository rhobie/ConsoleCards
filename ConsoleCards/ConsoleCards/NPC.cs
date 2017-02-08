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
        public void GroupCardsInHand()
        {
            foreach (var card in Hand)
            {
                card.cardDupCount = Hand.FindAll(x => x.value == card.value).Count;
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
                    //Hand.Find(x => x.tag == "StartingCard")



                    //cardRef.Add(new Card());
                    //cardRef[0] = Hand.Find(x => x.tag == "StartingCard");
                    //if (Hand.Contains(Hand.FindAll(x => x.value == cardRef[0].value)))
                    //{

                    //}
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
            //create two new methods, one that counts the duplicates in each players hand and tags them appropriatly on card creation - call everytime a card is played.
            // AND one that is called after every card is played but only for the card that was played.

            //  (Hand.Contains(Hand.Find(x => x.tier > TopRoundCard.tier && () <= TopRoundCard.cardsPlayed)))
            //                                                    x.cardsPlayed
            //CAN BEAT LAST CARD PLAYED:                                !! \/ HERE cardsPlayed is not set before calling this, need a way of counting avaible cards
            if (Hand.Contains(Hand.Find(x => x.tier > TopRoundCard.tier && x.cardDupCount <= TopRoundCard.cardDupCount)))
            {
                int cardsToMatch = TopRoundCard.cardDupCount;
                //cardRef.Add(new Card());

                cardRef.AddRange(Hand.FindAll(x => x.tier > TopRoundCard.tier && x.cardDupCount <= (TopRoundCard.cardDupCount)));

                //int extraCards = cardRef.Count - cardsToMatch;
                //cardRef.RemoveRange(0, extraCards);



                //for (int i = 0; i < cardsToMatch; i++)
                //{
                //    cardRef[i] = Hand.FindAll(x => x.tier > TopRoundCard.tier && x.cardDupCount <= (TopRoundCard.cardDupCount))[i-1];
                //    Hand.Remove(cardRef[i]);
                //    cardRef[i].cardDupCount = TopRoundCard.cardDupCount;
                //}
                //Hand.InsertRange(0, cardRef);


                return cardRef;

            }
            //    Hand.FindAll()
            //{
            //        TopRoundCard.cardsPlayed
            //    //need to cahnge this so that NPC does not just play the next highest card they have..
            //    //add next lot of decsion making here:;
            //        cardRef.Add(Hand.Find(x => x.tier > TopRoundCard.tier));
            //        return cardRef;
            //    }
            //}

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

