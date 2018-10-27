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
                        if (tempAmt > this.Money) //
                        {
                            Console.WriteLine("Amount bet is more than the amount you have available.");
                            amount = 0;
                        }
                        else if (tempAmt < 0)
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
                    default: Console.WriteLine("Invalid menu selection - try again"); continue;
                }
            } while (actionSelection != "1" && actionSelection != "2" &&
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
            PlayerAction pa = null;
            int cardstoDelete = 0;
            bool samePoints = false;
            bool suitsPair3 = false;
            bool suitsPair2 = false;
            bool straight = true;
            int straightPos = -1;
            string save = null;

            Dictionary<int, int> valueDic = new Dictionary<int, int>();
            Dictionary<String, int> suitDic = new Dictionary<string, int>();

            //initial lists
            for (int i = 2; i < 15; i++)
            {
                valueDic.Add(i, 0);
            }
            suitDic.Add("Hearts", 0);
            suitDic.Add("Spades", 0);
            suitDic.Add("Clubs", 0);
            suitDic.Add("Diamonds", 0);

            //check cards with same points
            for (int i = 0; i < hand.Length; i++)
                valueDic[hand[i].Value]++;

            //check same points in hand
            foreach (int value in valueDic.Values)
            {
                if (value >= 2)
                {
                    samePoints = true;
                }
            }

            //check cards with same suits
            for (int i = 0; i < hand.Length; i++)
                suitDic[hand[i].Suit]++;

            //check if has two or three of the same suit
            foreach (KeyValuePair<string, int> kvp in suitDic)
            {
                if (kvp.Value >= 3)
                    suitsPair3 = true;
                else if (kvp.Value == 2)
                {
                    save = kvp.Key;
                    suitsPair2 = true;
                }
            }

            //foreach (KeyValuePair<string, int> kvp in suitDic)
            //    Console.WriteLine(kvp.Key + " " + kvp.Value);

            //check cards with straight


            int[] d = new int[4];
            int[] a = new int[3];

            //distence between cards
            for (int i = 0; i < 4; i++)
                d[i] = System.Math.Abs(hand[i].Value - hand[i + 1].Value) - 1;

            //check if it is straight
            for (int i = 0; i < 4; i++)
                if (d[i] != 0)
                    straight = false;

            //check if player can replace one card to get straight
            for (int i = 0; i < 2; i++)
            {
                int x = d[i] + d[i + 1] + d[i + 2];
                if (x <= 1)
                {
                    straightPos = i;
                    break;
                }
            }

            //delete cards
            for (int i = 0; i < hand.Length; i++) // loop to delete cards
            {
                //if has straight
                if (straight)
                    break;
                //if has same points
                if (samePoints)
                {
                    if (valueDic[hand[i].Value] == 1)
                    {
                        Console.WriteLine("Deleted is " + hand[i].Value + "     " + hand[i].Suit);
                        hand[i] = null;
                        cardstoDelete++;

                    }
                    continue;
                }
                //if has three of the same suit
                if (suitsPair3)
                {
                    if (suitDic[hand[i].Suit] <= 2)
                    {
                        Console.WriteLine("Deleted is " + hand[i].Value + "     " + hand[i].Suit);
                        hand[i] = null;
                        cardstoDelete++;
                    }
                    continue;
                }
                //if has four cards that can be straight after replacing
                if (straightPos != -1)
                {
                    if (straightPos == 0)
                    {
                        Console.WriteLine("Deleted is " + hand[i].Value + "     " + hand[i].Suit);
                        hand[4] = null;
                    }
                    cardstoDelete++;
                    break;
                }
                //if has two of the same suit
                if (suitsPair2)
                {
                    if (suitDic[hand[i].Suit] == 2)
                    {
                        Console.WriteLine(save);
                        if (hand[i].Suit != save)
                        {
                            Console.WriteLine("Deleted is " + hand[i].Value + "     " + hand[i].Suit);
                            hand[i] = null;
                            cardstoDelete++;
                        }
                        continue;

                    }
                    else if (suitDic[hand[i].Suit] == 1)
                    {
                        Console.WriteLine("Deleted is " + hand[i].Value + "     " + hand[i].Suit);
                        hand[i] = null;
                        cardstoDelete++;
                    }
                    continue;
                }
                Console.WriteLine("Deleted is " + hand[i].Value + "     " + hand[i].Suit);
                hand[i] = null;
                cardstoDelete++;
            }
            Console.WriteLine("Cards to delete is " + cardstoDelete);
            Console.WriteLine("");
            pa = new PlayerAction(Name, "Draw", "draw", cardstoDelete);
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
