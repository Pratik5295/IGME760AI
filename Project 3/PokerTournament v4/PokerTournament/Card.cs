using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerTournament
{
    /*
     * Represents a single playing card
     */
    class Card
    {
        // attributes
        private int value;
        private string suit;

        public string Suit { get { return suit; } }
        public int Value { get { return value; } }

        // constructor
        public Card(string st, int val)
        {
            value = val;
            suit = st;
        }

        // override ToString
        public override string ToString()
        {
            string cardStr = "";

            // create the text
            switch(value)
            {
                case 2: cardStr = "Two of "; break;
                case 3: cardStr = "Three of "; break;
                case 4: cardStr = "Four of "; break;
                case 5: cardStr = "Five of "; break;
                case 6: cardStr = "Six of "; break;
                case 7: cardStr = "Seven of "; break;
                case 8: cardStr = "Eight of "; break;
                case 9: cardStr = "Nine of "; break;
                case 10: cardStr = "Ten of "; break;
                case 11: cardStr = "Jack of "; break;
                case 12: cardStr = "Queen of "; break;
                case 13: cardStr = "King of "; break;
                case 14: cardStr = "Ace of "; break;
            }

            // add the suit
            cardStr += suit;

            return cardStr;
        }
    }
}
