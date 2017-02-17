using System.Collections.Generic;
using System.Linq;

namespace ConsoleCards
{
    public class NPC
    {
        public int Id;
        public List<Card> Hand { get; set; }
        //private List<Card> _highlightedCards;
        public bool hasCards = false;
        public int[] Score { get; set; }
        public string Rank { get; set; }

        public NPC(int _activePlayers)
        {
            Rank = "Neutral";
            Id = _activePlayers + 1;
            Hand = new List<Card>();
            Score = new int[4] { 0, 0, 0, 0 };
        }

        public void SortCards()
        {
            Hand = Hand.OrderBy(x => x.Tier).ToList();
        }
        public void GroupCards(List<Card> hand)
        {
            if (hand.Count != 0)
            {
                hand.RemoveAll(x => x == null);
                foreach (var card in hand)
                {
                    if (card.Tag != "empty")
                    {

                        card.cardDupCount = hand.FindAll(x => x.Value == card.Value).Count;
                    }
                }
            }
        }


        public List<Card> SelectCardFromHand(Card TopDiscard, Card TopRoundCard)
        {
            var _highlightedCards = new List<Card>();

            //FIRST CARD OF GAME:
            if (TopDiscard.Name == "none" && TopRoundCard.Name == "none") //if first card of the game
            {
                if (Hand.Contains(Hand.Find(x => x.Tag == "StartingCard"))) //three of clubs is the starting card
                {
                    _highlightedCards = Hand.FindAll(x => x.Value == Hand[0].Value);
                }
                else
                {
                    _highlightedCards.Add(new Card());
                    _highlightedCards[0].Tag = "NoStartingCard";
                }
                return _highlightedCards;
            }

            //FIRST CARD OF ROUND:
            if (TopRoundCard.Tag == "empty")
            {
                //play all of the lowest card in hand:
                _highlightedCards = Hand.FindAll(x => x.Tier == Hand[0].Tier);
                //tag card(s) played with how many:
                _highlightedCards[0].cardDupCount = _highlightedCards.Count;
                return _highlightedCards;
            }

            //CAN JOKER A DOUBLE/TRIPLE/QUAD:
            if (true)
            {

            }

            //CAN BEAT CARD:
            if (Hand.Contains(Hand.Find(x => x.Tier > TopRoundCard.Tier && x.cardDupCount >= TopRoundCard.cardDupCount)) && TopRoundCard.Value != Value.Joker)
            {
                //will need to add to this section being able to beat 
                if (WillPlayJokerEarly(TopRoundCard))
                {
                    _highlightedCards.Add(Hand.Find(x => x.Value == Value.Joker));
                    return _highlightedCards;
                }

                int cardsToMatch = TopRoundCard.cardDupCount;
                _highlightedCards.Add(Hand.Find(x => x.Tier > TopRoundCard.Tier && x.cardDupCount >= TopRoundCard.cardDupCount));

                for (int i = 0; i < cardsToMatch - 1; i++)
                {
                    _highlightedCards.Add(Hand.Find(x => x.Value == _highlightedCards[0].Value && x.Suit != _highlightedCards[0].Suit));
                }

                if (Hand.Count == 2 && Hand.FindAll(x => x.Value == Value.Joker).Count == 2)
                {
                    //currently this will never happen as the jokers will be played long before the npc has two cards
                    _highlightedCards.Clear();
                    _highlightedCards.AddRange(Hand);
                }

                GroupCards(_highlightedCards);
                System.Console.Write("HAND VALUE = " + CalculateHandValue());

                return _highlightedCards;
            }
            else
            {
                _highlightedCards.Add(new Card());
                _highlightedCards[0].Tag = "Pass";
                return _highlightedCards;
            }
        }



        public bool WillPlayJokerEarly(Card TopRoundCard)
        {
            if (Hand.Contains(Hand.Find(x => x.Value == Value.Joker)))
            {
                int jokerCount = Hand.FindAll(x => x.Value == Value.Joker).Count;
                if (jokerCount == 1)
                {
                    //if (Hand.Contains(Hand.Find(x => x.Tier > TopRoundCard.Tier && x.Value != Value.Joker && //DOES NOT WORK YET, dup logic is backwards-ish
                    //!(x.Tier > TopRoundCard.Tier && x.cardDupCount >= TopRoundCard.cardDupCount)))) //if npc has a card that is higher than the toproundcard which is NOT a joker
                    //{                                                                                //and is NOT a higher card than the toproundcard, that you have the same amount of cards as //maybe add and toproundcard.carddupcount != 1??
                    //    return true;

                    //}
                    //if (CalculateHandValue() >) //need to refine this..
                    //{
                    //    //this is where a lot of the decision making will be..
                    //}

                    if (Hand.Count < 3) //will joker on second to last card
                    {
                        return true;
                    }
                }
                else //jokerCount is 2
                {
                    if (true)
                    {
                        //this is where a lot of the decision making will be..
                    }

                    if (Hand.Count < 5) //will joker on fourth to last card
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public int CalculateHandValue()
        {
            int handValue = 0;
            foreach (var card in Hand)
            {
                handValue += card.GetTeir();
            }
            handValue /= Hand.Count;

            return handValue;
        }

        public void RevealHand()
        {
            Commentary.ShowCards("NPC " + Id.ToString(), Hand);
        }

    }

}

//DECSION MAKING TO ADD:

// - if there is only one other player in the round and they passed the last round (when is was also just 2 players in round)
//   then play cards in reverse order (not including jokers, twos or aces)

// - 
