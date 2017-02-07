using System;

namespace ConsoleCards
{
    public class Card
    {
        public string Tag = "";

        private Suit _suit;
        private Value _value;
        private int _tier;
        private string _name;
        //private string _shortHand;

        public int tier
        {
            get { return _tier; }
        }

        public string name
        {
            get { return _name; }
        }

        public string symbol
        {
            get { return _symbol; }
        }

        public Suit suit
        {
            get { return _suit; }
        }

        public Value value
        {
            get { return _value; }
        }

        public Card()
        {
            //empty card object for storing temp cards and easy assignment
            _suit = Suit.none;
            _value = Value.none;
            _name = "none";
        }

        public Card(Suit suit, Value value)
        {
            _suit = suit;
            _value = value;

            if (value == Value.joker)
            {
                _name = "Joker";
            }
            else
            {
                _name = string.Format("{0} of {1}", value, suit);
            }

            
            //give jokers different names, OR gives all cards unique id and use that to search rather than name
            //will need to change at least at thte moment NPC - select/play

            int tierCalc = ((int)value + 1) *10;
            if (suit == Suit.clubs &&  value == Value.three)
            {
                tierCalc--;
            }
            _tier = tierCalc;
        }
        public char Symbol()
        {
            switch (_suit)
            {
                case Suit.clubs:
                    return '\u2663';//Clubs 
                case Suit.spades:
                    return '\u2660';//Spades
                case Suit.diamonds:
                    return '\u2666';//Diamonds
                case Suit.hearts:
                    return '\u2665';//Hearts
                //case Suit.wild:
                //    return '\u22C6';//Star (joker and none)
                //    break;
                //case Suit.none:
                //    return '\u22C6';//Star (joker and none)
                //    break;
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

