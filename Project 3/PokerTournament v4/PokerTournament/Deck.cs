using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerTournament
{
    /*
     * A deck of 52 cards (no jokers).
     */
    class Deck
    {
        // list to hold the cards
        private List<Card> cards = null;

        private List<Card> cardsDealt = null;

        // array of suits
        private string[] suits = { "Hearts", "Clubs", "Diamonds", "Spades" };

        // random number generator
        Random rgen = new Random();

        // constructor
        public Deck()
        {
        }

        // start of a new round - shuffle the deck and clear
        // the list of dealt cards
        public void NewRound()
        {
            MakeDeck(); // create a new deck
            cardsDealt = new List<Card>();  // new list of dealt cards
        }

        // Create a deck of Card objects
        private void MakeDeck()
        {
            Console.WriteLine("Making a new deck");
            // create the List
            cards = new List<Card>();

            // go through all four suits
            for (int suitNum = 0; suitNum < suits.Length;suitNum++)
            {
                // set each value
                for(int i = 2; i <= 14; i++)
                {
                    cards.Add(new Card(suits[suitNum], i));
                }
            }
        }

        // deal out N cards, making certain there are no duplicates
        public Card[] Deal(int num)
        {
            // create the hand array
            Card[] hand = new Card[num];

            // loop until we get all the cards needed
            for(int i = 0; i < num; i++)
            {
                // if the deck is empty, make a new deck
                if(cards.Count == 0)
                {
                    MakeDeck(); // create a new deck
                    DeleteDealtCards(); // clear out the cards already out
                }

                // get the card to add to the hand
                int cardPos = rgen.Next(cards.Count);
                hand[i] = cards[cardPos];

                // remove the card from the deck
                cards.RemoveAt(cardPos);
            }

            // return the full hand
            return hand;
        }

        // delete any dealt cards
        private void DeleteDealtCards()
        {
            // loop through the cards already dealt
            foreach(Card dealtCard in cardsDealt)
            {
                cards.Remove(dealtCard);
            }
        }
    }
}
