using System.Collections.Generic;

namespace ConsoleCards
{
    public class CardPile
    {
        public List<Card> Cards { get; set; }

        public CardPile()
        {
            Cards = new List<Card>();
        }

        public Card GetTopCard()
        {
            if (Cards.Count == 0)
            {
                //return no card
                return new Card();
            }
            else
            {
                //return top card
                return Cards[Cards.Count - 1]; ;
            }
        }

        public void MoveCardsToPile(CardPile otherPile)
        {
            foreach (var card in Cards)
            {
                otherPile.Cards.Add(card);
            }
            Cards.Clear();
        }
    }

}

