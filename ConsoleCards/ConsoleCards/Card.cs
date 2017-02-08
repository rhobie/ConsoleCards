using System;

namespace ConsoleCards
{
    public class Card
    {
        public string tag;// { get { return _tag; } }
        //private string _tag;

        public string uniqueId { get { return _uniqueId; } }
        private string _uniqueId;

        public Suit suit { get { return _suit; } }
        private Suit _suit;

        public Value value { get { return _value; } }
        private Value _value;

        public string name { get { return _name; } }
        private string _name;

        public int tier { get { return _tier; } }
        private int _tier;

        public string shorthand { get { return _shorthand; } }
        private string _shorthand;

        public int cardDupCount;// { get => cardDupCount; set => cardDupCount = value; }
        //private int _cardDupCount;

        readonly private string[] ShorthandValue = new string[] { " 3", " 4", " 5", " 6", " 7", " 8", " 9", "10", " J", " Q", " K", " A", " 2", "Jo", " n" };

        public Card()
        {
            //empty card object for storing temp cards and easy assignment
            _uniqueId = DateTime.Now.Ticks.ToString();
            _suit = Suit.none;
            _value = Value.none;
            _name = "none";
            tag = "empty";
        }

        public Card(string newUniqueId, Suit newSuit, Value newValue)
        {
            _uniqueId = newUniqueId;
            _suit = newSuit;
            _value = newValue;
            _name = (int)newValue == 13 ? "Joker" : string.Format("{0} of {1}", value, suit);
            _shorthand = ShorthandValue[(int)newValue] + GetSymbol();
            _tier = ((int)value + 1) * 10;
            //_cardDupCount = 0;

            if (newValue == Value.Three && newSuit == Suit.Clubs)
            {
                tag = "StartingCard";
            }
            else
            {
                tag = "default";
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
                case Suit.Wild:
                    return '\u22C6';//Star (joker and none)
                case Suit.none:
                    return '\u22C6';//Star (joker and none)
                default:
                    return '\u22C6'; //Star (joker and none)
            }
        }

        internal int GetTeir()
        {
            return tier;
        }
    }

}

