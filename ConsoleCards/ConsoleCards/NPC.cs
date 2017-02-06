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

        public Card SelectCardFromHand(Card TopDiscard, Card TopRoundCard)
        {
            Card card;
            //int index;

            //turn this into delegate so that classes dont talk to eachother directly
            //Card TopDiscard = DiscardPile.GetTopCard();
            //Card TopRoundCard = RoundDiscardPile.GetTopCard();

            if (TopDiscard == null && TopRoundCard == null)
            {
                if (Hand.Contains(Hand.Find(x => x.Tier == 9))) //three of clubs is the starting card
                {
                    //index = Hand.FindIndex(x => x.Tier == 9);
                    card = Hand.Find(x => x.Tier == 9);
                }
                else
                {
                    //NPC does not pass before the round has started.
                    //index = -9;
                    card = new Card();
                    card.SpecialID = "PassBeforeRoundStart";
                }
            }
            else if (TopRoundCard != null && Hand.Contains(Hand.Find(x => x.Tier > TopRoundCard.Tier)))
            {
                //add next lot of decsion making here:
                //index = Hand.FindIndex(x => x.Tier > TopRoundCard.Tier);
                card = Hand.Find(x => x.Tier > TopRoundCard.Tier);
            }
            else if (TopRoundCard == null)
            {
                //play first card in hand
                //index = 0;
                card = Hand[0];
            }
            else
            {
                //index = -1;
                card = new Card();
                card.SpecialID = "Pass";
                
            }

            return card;
        }

        public Card TakeCardFromHand(Card _SelectedCard)
        {
            if (Hand.Count != 0)
            {
                //int index = SelectCardFromHand();

                if (_SelectedCard.SpecialID == "PassBeforeRoundStart")
                {
                    //THESE CANT RETURN NULL, THEY HAVE TO RETURN BLANK CARDS
                    //return null;
                    //First round, no starting card in hand (SKIP)
                }
                else if (_SelectedCard.SpecialID == "Pass")
                {
                    //THESE CANT RETURN NULL, THEY HAVE TO RETURN BLANK CARDS
                    Pass();
                    //return null;
                }
                else
                {
                    //SELECT INPUT CARD REFERNCE IN HAND AND REMOVE AND RETURN
                    //Hand.Find(x => x.Name == _SelectedCard.Name);
                    Hand.Remove(Hand.Find(x => x.Name == _SelectedCard.Name));
                    //var card = new Card();
                    //card = Hand[_selectedCardIndex];
                    //Hand.Find(x => x.;

                    //return _SelectedCard;
                }
            }
            //DONT RETURN NULL
            return _SelectedCard;

            //PREVIOUS VERSION:
            //if (Hand.Count != 0)
            //{
            //    //int index = SelectCardFromHand();

            //    if (_selectedCardIndex == -9)
            //    {
            //        return null;
            //        //First round, no starting card in hand (SKIP)
            //    }
            //    else if (_selectedCardIndex == -1)
            //    {
            //        Pass();
            //        return null;
            //    }
            //    else
            //    {


            //        var card = new Card();
            //        card = Hand[_selectedCardIndex];
            //        Hand.RemoveAt(_selectedCardIndex);

            //        return card;
            //    }
            //}
            //return null;
        }

        public void Pass()
        {
            Console.WriteLine("NPC {0} PASSES", Id);
            GameLists.PlayersInRound.Remove(this);
        }

        public void RevealHand()
        {
            Debug.ShowCards("NPC " + Id.ToString(), Hand);
        }
    }

}

