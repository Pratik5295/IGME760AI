using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerTournament
{
    class Player7: Player
    {
        public bool isFirstOne = false;
        
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
            ListTheHand(hand);
            PlayerAction pa = null;
            int actionSelection = 0;

            int amount = 0;      //amount to bet
            double tempAmount = 0;
            Card highCard = null;
            int rank = Evaluate.RateAHand(hand, out highCard);
            //  10: Royal Flush
            //  9: straight flush
            //    8: four of a kind
            //   7:  full house
            //     6:  flush
            //    5:  straight
            //    4: three of a kind
            //   3: two pair
            //   2: one pair
            //   1: other

            //isFirstOne from round 1 states which player goes first
            //betting round 2 only has options: (5) fold (1) bet (2) raise (3) call
            //Code if goes first
            if (isFirstOne)
            {

                switch (rank)
                {
                    case 1:
                        actionSelection = 5;        //hand value other then fold, dont lose more money.
                        break;
                    case 2:
                        actionSelection = 5;        //when hand value has one pair, the probability of losing is higher, dont bet more as you are first player.
                        break;
                    case 3:
                        actionSelection = 1;                    //when hand value has two pairs, the probability of losing is higher, but still can bet a little.
                        tempAmount = Convert.ToDouble(Money);
                        tempAmount *= 0.02f;
                        tempAmount = Math.Floor(tempAmount);
                        amount = Convert.ToInt32(tempAmount);
                        break;

                    case 4:
                        actionSelection = 1;
                        tempAmount = Convert.ToDouble(Money);   // three of a kind, now we can bet a little more
                        tempAmount *= 0.07f;
                        tempAmount = Math.Floor(tempAmount);
                        amount = Convert.ToInt32(tempAmount);
                        break;


                    case 5:
                        actionSelection = 1;
                        tempAmount = Convert.ToDouble(Money);   //distinct hands with straight -10200, the chances of winning increase, so increase amount
                        tempAmount *= 0.4f;
                        tempAmount = Math.Floor(tempAmount);
                        amount = Convert.ToInt32(tempAmount);
                        break;

                    case 6:
                        actionSelection = 1;
                        tempAmount = Convert.ToDouble(Money);   //flush: distinct hands to draw- 5108, a good hand to win against opponents with a higher chance of getting such hand
                        tempAmount *= 0.6f;
                        tempAmount = Math.Floor(tempAmount);
                        amount = Convert.ToInt32(tempAmount);
                        break;

                    case 7:
                        actionSelection = 1;
                        tempAmount = Convert.ToDouble(Money);   //full house- proabability drops, but still a very formidable hand
                        tempAmount *= 0.3f;
                        tempAmount = Math.Floor(tempAmount);
                        amount = Convert.ToInt32(tempAmount);
                        break;

                    case 8:
                        actionSelection = 1;
                        tempAmount = Convert.ToDouble(Money);       //four of a kind, bet decent
                        tempAmount *= 0.4f;
                        tempAmount = Math.Floor(tempAmount);
                        amount = Convert.ToInt32(tempAmount);
                        break;

                    case 9:
                        actionSelection = 1;
                        tempAmount = Convert.ToDouble(Money);       //hand straight flush, bet high
                        tempAmount *= 0.5f;
                        tempAmount = Math.Floor(tempAmount);
                        amount = Convert.ToInt32(tempAmount);
                        break;

                    case 10:                                        //hand has royal flush, our chances of winning this round are very high.  
                        actionSelection = 1;
                        tempAmount = Convert.ToDouble(Money);
                        tempAmount *= 0.7f;
                        tempAmount = Math.Floor(tempAmount);
                        amount = Convert.ToInt32(tempAmount);
                        break;


                }
            }

            //Code if goes second
            else if (!isFirstOne)
            {
                string preAN = actions[actions.Count - 1].ActionName; // read the action name from the previous one player
                int preAMT = actions[actions.Count - 1].Amount; // read the amount bet from the previous one player

                double preAMTD = Convert.ToDouble(preAMT);
                double valueAMT = 0;
                tempAmount = Convert.ToDouble(Money);

                //if the opponent folds, you win
                if(preAN == "fold")
                {
                    return pa;
                }

                else if(preAN == "check")
                {
                    // no action as checking is not allowed in betting round 2
                    return pa;
                }

                else if (preAN == "call")
                { // actionSelection can be 3 (call), 5 (fold)
                   if(preAMT< tempAmount)       //Checks whether we have money to call,if not we will fold
                    {
                        actionSelection = 3;
                    }
                    else
                    {
                        actionSelection = 5;
                    }
                    
                }
                else if (preAN == "raise")
                { // actionSelection can be 2 (raise), 3 (call), 5 (fold)

                    if (rank >= 8)
                    {
                        tempAmount *= 0.8f;
                        tempAmount = Math.Floor(tempAmount);
                        amount = Convert.ToInt32(tempAmount);
                        actionSelection = 2;
                    }
                    else if (rank > 5)
                    {
                        valueAMT = tempAmount * 0.05f;
                        if (preAMTD > valueAMT)
                        {
                            actionSelection = 3;
                        }
                        else
                        {
                            actionSelection = 2;
                            tempAmount *= 0.05f;
                            amount = Convert.ToInt32(tempAmount);
                        }
                    }

                    else if (rank <= 2)
                    {
                        actionSelection = 5;        // enough bluffing, dont increase the bet we have a bad hand,also dont bet more money to lose it
                    }
                    else
                    {
                        actionSelection = 3;
                    }
                }
                else if (preAN == "bet")
                { // actionSelection can be 2 (raise), 3 (call), 5 (fold)
                    if (rank >= 8)
                    {
                        valueAMT = tempAmount * 0.8f;
                        if (preAMTD > valueAMT)
                        {
                            actionSelection = 3;
                        }
                        else
                        {
                            tempAmount = valueAMT;
                            amount = Convert.ToInt32(tempAmount);
                            actionSelection = 2;
                        }
                    }
                    else if (rank > 5)
                    {
                        valueAMT = tempAmount * 0.05f;
                        if (preAMTD > valueAMT)
                        {
                            actionSelection = 3;
                        }
                        else
                        {
                            tempAmount = valueAMT;
                            amount = Convert.ToInt32(tempAmount);
                            actionSelection = 2;
                        }
                    }
                  
                    else
                    {
                        actionSelection = 3;
                    }
                }


            }
            switch (actionSelection)
            {
                case 1: pa = new PlayerAction(Name, "Bet2", "bet", amount); break;
                case 2: pa = new PlayerAction(Name, "Bet2", "raise", amount); break;
                case 3: pa = new PlayerAction(Name, "Bet2", "call", amount); break;
            
                case 5: pa = new PlayerAction(Name, "Bet2", "fold", amount); break;
                default: pa = null; break;
            }

             isFirstOne = false;
            // return the player action
            return pa;
         
        }

        //override BettingRound1
        public override PlayerAction BettingRound1(List<PlayerAction> actions, Card[] hand)
        {
            
            ListTheHand(hand);
            
            PlayerAction pa = null;

            int actionSelection = 0;
            /*
            actionSelection 1: bet
            actionSelection 2: raise
            actionSelection 3: call
            actionSelection 4: check 
            actionSelection 5: fold
             */

            int amount = 0; // set amount for bet
            double tempAmount = 0;

            // check if the first player of the round 
            if (actions.Count == 0)
            {
                isFirstOne = true;
            } else if (actions.Count != 0) {
                isFirstOne = false;
            }

            // check the rank of the hand
            Card highCard = null;
            int rank = Evaluate.RateAHand(hand, out highCard);

            // if isFirstOne, actionSelection can be 1 (bet), 4 (check), 5(fold)
            if (isFirstOne){
                // select action by rank of the hand ...
                switch (rank)
                {
                    case 10:
                        actionSelection = 1;
                        amount = Money;
                        break; 
                    case 9:
                        actionSelection = 1;
                        tempAmount = Convert.ToDouble(Money);
                        tempAmount *= 0.5f;
                        tempAmount = Math.Floor(tempAmount);
                        amount = Convert.ToInt32(tempAmount);
                        break;
                    case 8:
                        actionSelection = 1;
                        tempAmount = Convert.ToDouble(Money);
                        tempAmount *= 0.2f;
                        tempAmount = Math.Floor(tempAmount);
                        amount = Convert.ToInt32(tempAmount);
                        break;
                    case 7:
                        actionSelection = 1;
                        tempAmount = Convert.ToDouble(Money);
                        tempAmount *= 0.1f;
                        tempAmount = Math.Floor(tempAmount);
                        amount = Convert.ToInt32(tempAmount);
                        break;
                    case 6:
                        actionSelection = 1;
                        tempAmount = Convert.ToDouble(Money);
                        tempAmount *= 0.05f;
                        tempAmount = Math.Floor(tempAmount);
                        amount = Convert.ToInt32(tempAmount);
                        break;
                    case 5:
                        actionSelection = 1;
                        tempAmount = Convert.ToDouble(Money);
                        tempAmount *= 0.04f;
                        tempAmount = Math.Floor(tempAmount);
                        amount = Convert.ToInt32(tempAmount);
                        break;
                    case 4:
                        actionSelection = 1;
                        tempAmount = Convert.ToDouble(Money);
                        tempAmount *= 0.03f;
                        tempAmount = Math.Floor(tempAmount);
                        amount = Convert.ToInt32(tempAmount);
                        break;
                    case 3:
                        actionSelection = 1;
                        tempAmount = Convert.ToDouble(Money);
                        tempAmount *= 0.02f;
                        tempAmount = Math.Floor(tempAmount);
                        amount = Convert.ToInt32(tempAmount);
                        break;
                    case 2: actionSelection = 1;
                        tempAmount = Convert.ToDouble(Money);
                        tempAmount *= 0.01f;
                        tempAmount = Math.Floor(tempAmount);
                        amount = Convert.ToInt32(tempAmount);
                        break;
                    case 1: actionSelection = 4; break;
                    default: break;
                }
            }

            // if NOT isFirstOne
            else if (!isFirstOne){
                string preAN = actions[actions.Count-1].ActionName; // read the action name from the previous one player
                int preAMT = actions[actions.Count - 1].Amount; // read the amount bet from the previous one player

                double preAMTD = Convert.ToDouble(preAMT);
                double valueAMT = 0;
                tempAmount = Convert.ToDouble(Money);

                if (preAN == "fold")
                { // no action and you win
                    // actionSelection = 0;
                    return pa;
                }
                else if (preAN == "check")
                { // actionSelection can be 1 (bet), 4 (check), 5 (fold)
                    actionSelection = 4;
                }
                else if (preAN == "call")
                { // actionSelection can be 3 (call), 5 (fold)
                    actionSelection = 3;
                }
                else if (preAN == "raise")
                { // actionSelection can be 2 (raise), 3 (call), 5 (fold)

                    if (rank > 8) {
                        tempAmount *= 0.5f;
                        tempAmount = Math.Floor(tempAmount);
                        amount = Convert.ToInt32(tempAmount);
                        actionSelection = 2;
                    }
                    else if (rank > 5) {
                        valueAMT = tempAmount * 0.05f;
                        if (preAMTD > valueAMT)
                        {
                            actionSelection = 3;
                        }
                        else {
                            actionSelection = 2;
                            tempAmount *= 0.05f;
                            amount = Convert.ToInt32(tempAmount);
                        }
                    }
                    else
                    {
                        actionSelection = 3;
                    }
                }
                else if (preAN == "bet")
                { // actionSelection can be 2 (raise), 3 (call), 5 (fold)
                    if (rank > 8)
                    {
                        valueAMT = tempAmount * 0.5f;
                        if (preAMTD > valueAMT)
                        {
                            actionSelection = 3;
                        }
                        else {
                            tempAmount = valueAMT;
                            amount = Convert.ToInt32(tempAmount);
                            actionSelection = 2;
                        }
                    }
                    else if (rank > 5)
                    {
                        valueAMT = tempAmount * 0.05f;
                        if (preAMTD > valueAMT)
                        {
                            actionSelection = 3;
                        }
                        else
                        {
                            tempAmount = valueAMT;
                            amount = Convert.ToInt32(tempAmount);
                            actionSelection = 2;
                        }
                    }
                    else
                    {
                        actionSelection = 3;
                    }
                }
                else {
                    return pa;
                }
            }

            
            /*
            switch(actions[actions.Count-1].actionName)
            {
                case "bet":
                    if (true) { }
                    break;
            }
            */

            /*
            switch (rank)
            {
                case 10: actionSelection = 1; break; // Royal Flush
                case 9: break; // straight flush
                case 8: break; // four of a kind
                case 7: break; // full house
                case 6: break; // flush
                case 5: break; // straight
                case 4: break; // three of a kind
                case 3: break; // two pair
                case 2: break; // one pair
                case 1: break; // other
                default: break;
            }
            */


            // convert actionSelection numbers into certain action
            switch (actionSelection)
            {
                case 1: pa = new PlayerAction(Name, "Bet1", "bet", amount); break;
                case 2: pa = new PlayerAction(Name, "Bet1", "raise", amount); break;
                case 3: pa = new PlayerAction(Name, "Bet1", "call", amount); break;
                case 4: pa = new PlayerAction(Name, "Bet1", "check", amount); break;
                case 5: pa = new PlayerAction(Name, "Bet1", "fold", amount); break;
                default: pa = null; break;
            }

          //  isFirstOne = false;       //same for round 2
            // return the player action
            return pa;
        }

        //override Draw
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
    }
}
