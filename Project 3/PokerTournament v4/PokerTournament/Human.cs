using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerTournament
{
    // allows a human player to participate
    class Human : Player
    {
        // setup your basic human player
        public Human(int idNum, string nm, int mny) : base(idNum, nm, mny)
        {
        }

        // handle the first round of betting
        public override PlayerAction BettingRound1(List<PlayerAction> actions, Card[] hand)
        {
            // list the hand
            ListTheHand(hand);

            // select an action
            string actionSelection = "";
            PlayerAction pa = null;
            do
            {
                Console.WriteLine("Select an action:\n1 - bet\n2 - raise\n3 - call\n4 - check\n5 - fold");
                actionSelection = Console.ReadLine();

                // get amount if appropriate
                int amount = 0;
                if (actionSelection[0] == '1' || actionSelection[0] == '2')
                {
                    string amtText = "";
                    do
                    {
                        if (actionSelection[0] == '1') // bet
                        {
                            Console.Write("Amount to bet? ");
                            amtText = Console.ReadLine();
                        }
                        else if (actionSelection[0] == '2') // raise
                        {
                            Console.Write("Amount to raise? ");
                            amtText = Console.ReadLine();
                        }
                        // convert the string to an int
                        int tempAmt = 0;
                        int.TryParse(amtText, out tempAmt);

                        // check input
                        if(tempAmt > this.Money) //
                        {
                            Console.WriteLine("Amount bet is more than the amount you have available.");
                            amount = 0;
                        }
                        else if(tempAmt < 0)
                        {
                            Console.WriteLine("Amount bet or raised cannot be less than zero.");
                            amount = 0;
                        }
                        else
                        {
                            amount = tempAmt;
                        }
                    } while (amount <= 0);
                }

                // create the PlayerAction
                switch (actionSelection)
                {
                        case "1": pa = new PlayerAction(Name, "Bet1", "bet", amount); break;
                        case "2": pa = new PlayerAction(Name, "Bet1", "raise", amount); break;
                        case "3": pa = new PlayerAction(Name, "Bet1", "call", amount); break;
                        case "4": pa = new PlayerAction(Name, "Bet1", "check", amount); break;
                        case "5": pa = new PlayerAction(Name, "Bet1", "fold", amount); break;
                        default:Console.WriteLine("Invalid menu selection - try again"); continue;
                }
            }while (actionSelection != "1" && actionSelection != "2" &&
                    actionSelection != "3" && actionSelection != "4" &&
                    actionSelection != "5");
            // return the player action
            return pa;
        }

        // reuse the same logic for second betting round
        public override PlayerAction BettingRound2(List<PlayerAction> actions, Card[] hand)
        {
            PlayerAction pa1 = BettingRound1(actions, hand);

            // create a new PlayerAction object
            return new PlayerAction(pa1.Name, "Bet2", pa1.ActionName, pa1.Amount);
        }

        public override PlayerAction Draw(Card[] hand)
        {
            // list the hand
            ListTheHand(hand);

            // determine how many cards to delete
            int cardsToDelete = 0;
            do
            {
                Console.Write("How many cards to delete? "); // get the count
                string deleteStr = Console.ReadLine();
                int.TryParse(deleteStr, out cardsToDelete);
            } while (cardsToDelete < 0 || cardsToDelete > 5);

            // which cards to delete if any
            PlayerAction pa = null;
            if(cardsToDelete > 0 && cardsToDelete < 5)
            {
                for (int i = 0; i < cardsToDelete; i++) // loop to delete cards
                {
                    Console.WriteLine("\nDelete card " + (i + 1) + ":");
                    for (int j = 0; j < hand.Length; j++)
                    {
                        Console.WriteLine("{0} - {1}", (j + 1), hand[j]);
                    }
                    // selete cards to delete
                    int delete = 0;
                    do
                    {

                        Console.Write("Which card to delete? (1 - 5): ");
                        string delStr = Console.ReadLine();
                        int.TryParse(delStr, out delete);

                        // see if the entry is valid
                        if (delete < 1 || delete > 5)
                        {
                            Console.WriteLine("Invalid entry - enter a value between 1 and 5.");
                            delete = 0;
                        }
                        else if (hand[delete - 1] == null)
                        {
                            Console.WriteLine("Entry was already deleted.");
                            delete = 0;
                        }
                        else
                        {
                            hand[delete - 1] = null; // delete entry
                            delete = 99; // flag to exit loop
                        }
                    } while (delete == 0);
                }
                // set the PlayerAction object
                pa = new PlayerAction(Name, "Draw", "draw", cardsToDelete);
            }
            else if(cardsToDelete == 5)
            {
                // delete them all
                for(int i = 0; i < hand.Length; i++)
                {
                    hand[i] = null;
                }
                pa = new PlayerAction(Name, "Draw", "draw", 5);
            }
            else // no cards deleted
            {
                pa = new PlayerAction(Name, "Draw", "stand pat", 0);
            }

            // return the action
            return pa;
        }

        // helper method - list the hand
        private void ListTheHand(Card[] hand)
        {
            // evaluate the hand
            Card highCard = null;
            int rank = Evaluate.RateAHand(hand, out highCard);

            // list your hand
            Console.WriteLine("\nName: " + Name + " Your hand:   Rank: " + rank);
            for (int i = 0; i < hand.Length; i++)
            {
                Console.Write(hand[i].ToString() + " ");
            }
            Console.WriteLine();
        }
    }
}
