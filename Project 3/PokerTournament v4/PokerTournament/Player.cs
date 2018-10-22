using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace PokerTournament
{
    // identify a player in the game
    abstract class Player
    {
        private int id;
        private string name;
        private Boolean dealer;
        private Boolean inGame;
        private int money;
        private Card[] hand;

        // constructor
        public Player(int idNum, string nm, int mny)
        {
            id = idNum;
            name = nm;
            dealer = false;
            inGame = true;
            money = mny;
        }

        // properties
        public int Id { get { return id; } }
        public string Name { get { return name; } }
        public Boolean Dealer
        {
            get { return dealer; }
            set { dealer = value; }
        }

        public Boolean InGame
        {
            get { return inGame; }
            set { inGame = value; }
        }

        public Card[] Hand
        {
            get { return hand; }
            set { hand = value; }
        }

        public int Money { get { return money; } }

        // change money value
        public void ChangeMoney(int amt)
        {
            money += amt; // positive to increase funds, negative to reduce them
        }

        // override ToString
        public override string ToString()
        {
            return " Name: " + name + 
                " Dealer: " + dealer + " Money: " + money;
        }

        // add the cards back into your hand after the Draw step
        public void AddCards(Card[] hand, Card[] newCards)
        {
            // fill in the empty spots
            for (int i = 0; i < newCards.Length; i++)
            {
                for (int j = 0; j < hand.Length; j++)
                {
                    if (hand[j] == null) // empty spot
                    {
                        hand[j] = newCards[i]; // fill the spot
                        break;
                    }
                }
            }

            // sort the cards
            Evaluate.SortHand(hand);
        }

        // abstract methods

        // first round of betting - see project instructions
        public abstract PlayerAction BettingRound1(List<PlayerAction> actions,Card[] hand);
        
        // draw 0 - 5 cards - see project instructions
        public abstract PlayerAction Draw(Card[] hand);
        

        // second round of betting - see project instructions
        public abstract PlayerAction BettingRound2(List<PlayerAction> actions, Card[] hand); 
    }
}