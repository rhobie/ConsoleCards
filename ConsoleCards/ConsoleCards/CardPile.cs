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
            Card topCard;
            if (Cards.Count == 0)
            {
                //return null card
                topCard = new Card("");
                return topCard;
            }
            else
            {
                topCard = Cards[Cards.Count - 1];
                return topCard;
            }
        }

        public void MoveCardsToPile(CardPile newPile)
        {
            foreach (var card in Cards)
            {
                newPile.Cards.Add(card);
            }
            Cards.Clear();
        }
    }

}

