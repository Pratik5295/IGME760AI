using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerTournament
{
    class Player7: Player
    {
        //constructor inherited from Player
        public Player7(int idNum, string nm, int mny): base(idNum,nm,mny)
        {
        }

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

        //override BettingRound2
        public override PlayerAction BettingRound2(List<PlayerAction> actions, Card[] hand)
        {
            return null;
        }

        //override BettingRound1
        public override PlayerAction BettingRound1(List<PlayerAction> actions, Card[] hand)
        {
            return null;
        }

        //override Draw
        public override PlayerAction Draw(Card[] hand)
        {
            PlayerAction pa = null;
            int cardstoDelete = 0;
            bool isSamePoints = false;
            bool isThreeSuitsPair = false;
            bool isTwoSuitsPair = false;
            bool isStraight = true;
            int straightPos = -1;
            string SuitToSave = null;

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
                    isSamePoints = true;
                }
            }

            //check cards with same suits
            for (int i = 0; i < hand.Length; i++)
                suitDic[hand[i].Suit]++;

            //check if has two or three of the same suit
            foreach (KeyValuePair<string, int> kvp in suitDic)
            {
                if (kvp.Value >= 3)
                    isThreeSuitsPair = true;
                else if (kvp.Value == 2)
                {
                    SuitToSave = kvp.Key;
                    isTwoSuitsPair = true;
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
                    isStraight = false;

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
                if (isStraight)
                    break;
                //if has same points
                if (isSamePoints)
                {
                    if (valueDic[hand[i].Value] == 1)
                    {
                        //Console.WriteLine("Deleted is " + hand[i].Value + "     " + hand[i].Suit);
                        hand[i] = null;
                        cardstoDelete++;

                    }
                    continue;
                }
                //if has three of the same suit
                if (isThreeSuitsPair)
                {
                    if (suitDic[hand[i].Suit] <= 2)
                    {
                        //Console.WriteLine("Deleted is " + hand[i].Value + "     " + hand[i].Suit);
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
                        //Console.WriteLine("Deleted is " + hand[i].Value + "     " + hand[i].Suit);
                        hand[4] = null;
                    }
                    cardstoDelete++;
                    break;
                }
                //if has two of the same suit
                if (isTwoSuitsPair)
                {
                    if (suitDic[hand[i].Suit] == 2)
                    {
                        //Console.WriteLine(SuitToSave);
                        if (hand[i].Suit != SuitToSave)
                        {
                            //Console.WriteLine("Deleted is " + hand[i].Value + "     " + hand[i].Suit);
                            hand[i] = null;
                            cardstoDelete++;
                        }
                        continue;

                    }
                    else if (suitDic[hand[i].Suit] == 1)
                    {
                        //Console.WriteLine("Deleted is " + hand[i].Value + "     " + hand[i].Suit);
                        hand[i] = null;
                        cardstoDelete++;
                    }
                    continue;
                }
                //Console.WriteLine("Deleted is " + hand[i].Value + "     " + hand[i].Suit);
                hand[i] = null;
                cardstoDelete++;
            }
            //Console.WriteLine("Cards to delete is " + cardstoDelete);
            //Console.WriteLine("");
            pa = new PlayerAction(Name, "Draw", "draw", cardstoDelete);
            return pa;
        }
    }
}
