namespace ConsoleCards
{
    class Card
    {
        private Suit suit;
        private Value value;

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
        }
    }

}

