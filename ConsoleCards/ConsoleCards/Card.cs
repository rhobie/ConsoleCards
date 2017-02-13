using System;

namespace ConsoleCards
{
    public class Card
    {
        public string Tag;// { get { return _tag; } }
        //private string _tag;

        public string UniqueId { get { return _uniqueId; } }
        private string _uniqueId;

        public Suit Suit { get { return _suit; } }
        private Suit _suit;

        public Value Value { get { return _value; } }
        private Value _value;

        public string Name { get { return _name; } }
        private string _name;

        public int Tier { get { return _tier; } }
        private int _tier;

        public string Shorthand { get { return _shorthand; } }
        private string _shorthand;

        public int cardDupCount { get => _cardDupCount; set => _cardDupCount = value; }
        private int _cardDupCount;

        readonly private string[] ShorthandValue = new string[] { " 3", " 4", " 5", " 6", " 7", " 8", " 9", "10", " J", " Q", " K", " A", " 2", "Jo", " n" };

        public Card()
        {
            //empty card object for storing temp cards and easy assignment
            _uniqueId = DateTime.Now.Ticks.ToString();
            _suit = Suit.none;
            _value = Value.none;
            _name = "none";
            Tag = "empty";
            cardDupCount = 0;
        }

        public Card(string uniqueId, Suit suit, Value value)
        {
            _uniqueId = uniqueId;
            _suit = suit;
            _value = value;
            _name = (int)value == 13 ? "Joker" : string.Format("{0} of {1}", Value, Suit);
            _shorthand = ShorthandValue[(int)value] + GetSymbol();
            _tier = ((int)Value + 1) * 10;
            //_cardDupCount = 0; if this is ever zero something has gone wrong

            if (value == Value.Three && suit == Suit.Clubs)
            {
                Tag = "StartingCard";
            }
            else
            {
                Tag = "default";
            }
        }


        public char GetSymbol()
        {
            switch (_suit)
            {
                case Suit.Clubs:
                    return '\u2663';//Clubs 
                case Suit.Spades:
                    return '\u2660';//Spades
                case Suit.Diamonds:
                    return '\u2666';//Diamonds
                case Suit.Hearts:
                    return '\u2665';//Hearts
                case Suit.Black:
                    return '\u22C6';//Star (joker and none)
                case Suit.none:
                    return '\u22C6';//Star (joker and none)
                default:
                    return '\u22C6'; //Star (joker and none)
            }
        }

        internal int GetTeir()
        {
            return Tier;
        }
    }

}

