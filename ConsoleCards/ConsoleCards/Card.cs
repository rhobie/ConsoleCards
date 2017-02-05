using System;

namespace ConsoleCards
{
    public class Card
    {
        
        private Suit suit;
        private Value value;
        private int tier;

        public int Tier
        {
            get { return this.tier; }
        }

        public Suit Suit
        {
            get { return this.suit; }
        }

        public Value Value
        {
            get { return this.value; }
        }

        public Card(Suit _suit, Value _value)
        {
            this.suit = _suit;
            this.value = _value;

            int tierCalc = ((int)Value + 1) *10;
            if (_suit == Suit.clubs &&  _value == Value.three)
            {
                tierCalc--;
            }
            this.tier = tierCalc;
        }

        internal int GetTeir()
        {
            return Tier;
        }
    }

}

