 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace PokerTournament
{
    // plays a round of poker between two players
    class Game
    {
        // players in the game
        Player p0;
        Player p1;

        // file to track results
        StreamWriter tournamentResults;

        // deck of cards
        Deck deck = new Deck();

        // pot of money - used by Tournament and Round methods
        int pot = 0;  // pot of money (holds bets and ante)
        int anteAmt = 0;

        // constructor
        public Game(Player a, Player b)
        {
            // assign the playersa
            p0 = a;
            p1 = b;

            // set up the results file
            string filename = p0.Name + " vs " + p1.Name + ".txt";
            tournamentResults = new StreamWriter(filename);
        }

        // Tournament game - plays until one player is out of money
        // or 100 rounds have occurred, whichever comes first
        public void Tournament()
        {
            // set up
            p0.Dealer = true; // set p0 as dealer initially
            int rounds = 1; // count the rounds
            int ante = 10; // initial ante amount


            // loop
            while(rounds <= 100 && p0.Money > 0 && p1.Money > 0)
            {
                // announce start of round
                ResultWriter("Starting round " + rounds);

                // set the ante
                if (rounds <= 25) ante = 10;
                else if (rounds <= 50) ante = 20;
                else ante = 30;
                ResultWriter("Ante for this round: " + ante);

                // ante up - if a player can't pay the ante then they lose
                if (p0.Money - ante <= 0)
                {
                    ResultWriter(p0.Name + " cannot ante, so they lose the tournament.");
                    break;
                }

                if (p1.Money - ante <= 0)
                {
                    ResultWriter(p1.Name + " cannot ante, so they lose the tournament.");
                    break;
                }

                // ante up
                anteAmt = ante * 2; // add to the pot in Round
                p0.ChangeMoney(-ante);
                p1.ChangeMoney(-ante);

                // run the round
                string roundResult = Round();
                ResultWriter(roundResult);

                // list player status
                ResultWriter("\nAfter round " + rounds + ", player status: ");
                ResultWriter(p0.ToString());
                ResultWriter(p1.ToString());
                ResultWriter(" ");

                // change dealers
                if(p0.Dealer == true)
                {
                    p0.Dealer = false;
                    p1.Dealer = true;
                }
                else
                {
                    p0.Dealer = true;
                    p1.Dealer = false;
                }
                rounds++; // next round
            }
            // game over - announce results
            if(rounds > 100)
            {
                ResultWriter("After 100 rounds:");
                if(p0.Money > p1.Money)
                {
                    string text = p0.Name + " has " + p0.Money +
                        " credits and " + p1.Name + " has " +
                        p1.Money + "credits. " + p0.Name +
                        " is the winner!";
                    ResultWriter(text);
                }
                else if(p1.Money > p0.Money)
                {
                    string text = p0.Name + " has " + p0.Money +
                        " credits and " + p1.Name + " has " +
                        p1.Money + "credits. " + p1.Name +
                        " is the winner!";
                    ResultWriter(text);
                }
                else // tie
                {
                    string text = p0.Name + " has " + p0.Money +
                        " credits and " + p1.Name + " has " +
                        p1.Money + "credits. " + "Tie - no winner";
                    ResultWriter(text);
                }
            }
            else  // someone is broke
            {
                ResultWriter("After " + (rounds - 1) + " rounds:");
                if(p0.Money == 0)
                {
                    string text = p0.Name + " has " + p0.Money +
                        " credits and " + p1.Name + " has " +
                        p1.Money + "credits. " + p1.Name +
                        " is the winner!";
                    ResultWriter(text);
                }
                else
                {
                    string text = p0.Name + " has " + p0.Money +
                        " credits and " + p1.Name + " has " +
                        p1.Money + "credits. " + p0.Name +
                        " is the winner!";
                    ResultWriter(text);
                }
            }
        }

        // plays 1 round of poker
        private string Round()
        {
            string text = ""; // result text
            List<PlayerAction> actions = new List<PlayerAction>(); // list of actions

            // reset the pot
            if(pot % 2 == 0) // even numbered pot
            {
                pot = anteAmt; // pot with antes only
            }
            else // odd pot
            {
                // in this case, the pot was not an even number
                // so there was 1 credit left over for the starting pot
                // In theory this should never happen.
                pot = anteAmt + 1; 
            }
            // call players in order
            Player[] playerOrder = new Player[2];

            // note that playerOrder[1] always contains the dealer
            if(p0.Dealer == true) // player 0 deals?
            {
                playerOrder[0] = p1; // p1 goes first
                playerOrder[1] = p0;
            }
            else
            {
                playerOrder[0] = p0; // p0 goes first
                playerOrder[1] = p1;
            }
            // setup deck for a new round
            deck.NewRound();

            // dealer deals out 5 cards to each player
            playerOrder[0].Hand = deck.Deal(5);
            playerOrder[1].Hand = deck.Deal(5);

            // round 1 of betting - loop until both players check,
            // one folds, or one calls
            ResultWriter("Betting round 1:");

            Boolean done = false; // flags when finished
            do
            {
                PlayerAction pa0 = playerOrder[0].BettingRound1(actions, playerOrder[0].Hand);
                bool valid = CheckAction("Bet1",actions, pa0);
                if(valid == false)
                {
                    ResultWriter(playerOrder[0].Name + " played a bad action of " + pa0.ActionName + " and forfeits the hand");
                    pa0 = new PlayerAction(pa0.Name, pa0.ActionPhase, "fold", 0);
                }
                actions.Add(pa0);
                ResultWriter(pa0.ToString());
                ResultWriter(" ");

                // handle the case of the first player calling - the
                // second player must also call - do this automatically
                // and break out of the loop
                if(pa0.ActionName == "call")
                {
                    // add the second player's action automatically
                    PlayerAction pa1 = new PlayerAction(playerOrder[1].Name, "Bet1", "call", 0);
                    actions.Add(pa1);
                    break; // done betting
                }

                if (pa0.ActionName != "fold") // first player did not fold
                {
                    PlayerAction pa1 = playerOrder[1].BettingRound1(actions, playerOrder[1].Hand);
                    valid = CheckAction("Bet1", actions, pa1);
                    if (valid == false)
                    {
                        ResultWriter(playerOrder[1].Name + " played a bad action of " + pa1.ActionName + " and forfeits the hand");
                        pa1 = new PlayerAction(pa1.Name, pa1.ActionPhase, "fold", 0);
                    }
                    actions.Add(pa1);
                    ResultWriter(pa1.ToString());
                    ResultWriter(" ");
                }
                done = EvaluateActions(actions,"Bet1");
            } while (done == false);

            // update the pot based on the bets
            int lastBet = 0;
            for(int i = 0; i < actions.Count; i++)
            {
                if(actions[i].ActionPhase == "Bet1")
                {
                    switch(actions[i].ActionName)
                    {
                        case "bet":
                            lastBet = actions[i].Amount; 
                            pot += lastBet; // adjust the pot
                            // deduct from player
                            if(actions[i].Name == playerOrder[0].Name) // player0 bet?
                            {
                                playerOrder[0].ChangeMoney(-lastBet);
                            }
                            else // must be player1
                            {
                                playerOrder[1].ChangeMoney(-lastBet);
                            }
                            break;
                        case "raise":
                            int total = lastBet; // amt from previous player
                            pot += lastBet; // player raising must match last bet
                            lastBet = actions[i].Amount;
                            total += lastBet; // amt being raised
                            pot += lastBet; // plus the amount raised
                            // deduct from player
                            if (actions[i].Name == playerOrder[0].Name) // player0 bet?
                            {
                                playerOrder[0].ChangeMoney(-total);
                            }
                            else // must be player1
                            {
                                playerOrder[1].ChangeMoney(-total);
                            }
                            break;
                        case "call":
                            // skip if this is a call after another call
                            if (i - 1 >= 0)
                            {
                                if (actions[i - 1].ActionName == "call")
                                {
                                    break;
                                }
                            }
                            pot += lastBet; // match the last bet
                            // deduct from player
                            if (actions[i].Name == playerOrder[0].Name) // player0 bet?
                            {
                                playerOrder[0].ChangeMoney(-lastBet);
                            }
                            else // must be player1
                            {
                                playerOrder[1].ChangeMoney(-lastBet);
                            }
                            break;
                    }
                }
            }

            ResultWriter("After Bet1, pot is " + pot);
            ResultWriter(" ");

            // see if someone folded
            if (actions[actions.Count - 1].ActionName == "fold" &&
                actions[actions.Count - 1].Name == playerOrder[1].Name)
            {
                // if the player in playerOrder[1] folded, other
                // player gets the pot
                playerOrder[0].ChangeMoney(pot);
                string result = actions[actions.Count - 1].Name + " folded. Other player gets the pot of " + pot;
                pot = 0; // clear the pot
                return result; // skip rest of loop
            }
            else if(actions[actions.Count - 1].ActionName == "fold" &&
                actions[actions.Count - 1].Name == playerOrder[0].Name)
            {
                // if the player in playerOrder[1] folded, other
                // player gets the pot
                playerOrder[1].ChangeMoney(pot);
                string result = actions[actions.Count - 1].Name + " folded. Other player gets the pot of " + pot;
                pot = 0; // clear the pot
                return result; // skip rest of loop
            }

            // draw
            for (int i = 0; i < playerOrder.Length; i++)
            {
                PlayerAction pa = playerOrder[i].Draw(playerOrder[i].Hand);
                actions.Add(pa);
                if(pa.Amount > 0)
                {
                    Card[] newCards = deck.Deal(pa.Amount); // get cards
                    playerOrder[i].AddCards(playerOrder[i].Hand, newCards);
                }
                ResultWriter("Name: " + playerOrder[i].Name);
                string handList = Evaluate.ListHand(playerOrder[i].Hand);
                ResultWriter(handList);
                ResultWriter(" ");
            }

            // round 2 of betting- loop until both players check,
            // one folds, or one calls
            ResultWriter("Betting round 2:");
            done = false; // flags when finished
            do
            {
                PlayerAction pa0 = playerOrder[0].BettingRound2(actions, playerOrder[0].Hand);
                bool valid = CheckAction("Bet2", actions, pa0);
                if (valid == false)
                {
                    ResultWriter(playerOrder[0].Name + " played a bad action of " + pa0.ActionName + " and forfeits the hand");
                    pa0 = new PlayerAction(pa0.Name, pa0.ActionPhase, "fold", 0);
                }
                actions.Add(pa0);
                ResultWriter(pa0.ToString());
                ResultWriter(" ");

                // handle the case of the first player calling - the
                // second player must also call - do this automatically
                // and break out of the loop
                if (pa0.ActionName == "call")
                {
                    // add the second player's action automatically
                    PlayerAction pa1 = new PlayerAction(playerOrder[1].Name, "Bet2", "call", 0);
                    actions.Add(pa1);
                    break; // done betting
                }

                if (pa0.ActionName != "fold") // first player did not fold
                {
                    PlayerAction pa1 = playerOrder[1].BettingRound2(actions, playerOrder[1].Hand);
                    valid = CheckAction("Bet2", actions, pa1);
                    if (valid == false)
                    {
                        ResultWriter(playerOrder[1].Name + " played a bad action of " + pa1.ActionName + " and forfeits the hand");
                        pa1 = new PlayerAction(pa1.Name, pa1.ActionPhase, "fold", 0);
                    }
                    actions.Add(pa1);
                    ResultWriter(pa1.ToString());
                    ResultWriter(" ");
                }
                done = EvaluateActions(actions, "Bet2");
            } while (done == false);

            // update the pot based on the bets
            lastBet = 0;
            for (int i = 0; i < actions.Count; i++)
            {
                if (actions[i].ActionPhase == "Bet2")
                {
                    switch (actions[i].ActionName)
                    {
                        case "bet":
                            lastBet = actions[i].Amount;
                            pot += lastBet; // adjust the pot
                            // deduct from player
                            if (actions[i].Name == playerOrder[0].Name) // player0 bet?
                            {
                                playerOrder[0].ChangeMoney(-lastBet);
                            }
                            else // must be player1
                            {
                                playerOrder[1].ChangeMoney(-lastBet);
                            }
                            break;
                        case "raise":
                            int total = lastBet; // amt from previous player
                            pot += lastBet; // player raising must match last bet
                            lastBet = actions[i].Amount;
                            total += lastBet; // amt being raised
                            pot += lastBet; // plus the amount raised
                            // deduct from player
                            if (actions[i].Name == playerOrder[0].Name) // player0 bet?
                            {
                                playerOrder[0].ChangeMoney(-total);
                            }
                            else // must be player1
                            {
                                playerOrder[1].ChangeMoney(-total);
                            }
                            break;
                        case "call":
                            // skip if this is a call after another call
                            if(i-1 >= 0)
                            {
                                if(actions[i - 1].ActionName == "call")
                                {
                                    break;
                                }
                            }
                            pot += lastBet; // match the last bet
                            // deduct from player
                            if (actions[i].Name == playerOrder[0].Name) // player0 bet?
                            {
                                playerOrder[0].ChangeMoney(-lastBet);
                            }
                            else // must be player1
                            {
                                playerOrder[1].ChangeMoney(-lastBet);
                            }
                            break;
                    }
                }
            }

            ResultWriter("After Bet2, pot is " + pot);
            ResultWriter(" ");

            // see if someone folded
            if (actions[actions.Count - 1].ActionName == "fold" &&
                actions[actions.Count - 1].Name == playerOrder[1].Name)
            {
                // if the player in playerOrder[1] folded, other
                // player gets the pot
                playerOrder[0].ChangeMoney(pot);
                string result = actions[actions.Count - 1].Name + " folded. Other player gets the pot of " + pot;
                pot = 0; // clear the pot
                return result; // skip rest of loop
            }
            else if (actions[actions.Count - 1].ActionName == "fold" &&
                actions[actions.Count - 1].Name == playerOrder[0].Name)
            {
                // if the player in playerOrder[1] folded, other
                // player gets the pot
                playerOrder[1].ChangeMoney(pot);
                string result = actions[actions.Count - 1].Name + " folded. Other player gets the pot of " + pot;
                pot = 0; // clear the pot
                return result; // skip rest of loop
            }

            // round resolution
            // see if there is a clear winner based on hand strength
            Card highCard = null;
            int p0Rank = Evaluate.RateAHand(playerOrder[0].Hand,out highCard);
            int p1Rank = Evaluate.RateAHand(playerOrder[1].Hand, out highCard);
            if(p0Rank > p1Rank)
            {
                text = playerOrder[0].Name + " has a better hand and wins " + pot;
                playerOrder[0].ChangeMoney(pot);
                pot = 0;
            }
            else if(p1Rank > p0Rank)
            {
                text = playerOrder[1].Name + " has a better hand and wins " + pot;
                playerOrder[1].ChangeMoney(pot);
                pot = 0;
            }
            else // same rank - needs further examination
            {
                // sort both hands
                Evaluate.SortHand(playerOrder[0].Hand);
                Card[] hand0 = playerOrder[0].Hand;
                Evaluate.SortHand(playerOrder[1].Hand);
                Card[] hand1 = playerOrder[1].Hand;

                switch (p0Rank)
                {
                    case 1: // high card
                        for(int i = 4; i >= 0;i--)
                        {
                            if(hand0[i].Value != hand1[i].Value)
                            {
                                if(hand0[i].Value > hand1[i].Value)
                                {
                                    text = playerOrder[0].Name + " has a better hand and wins " + pot;
                                    playerOrder[0].ChangeMoney(pot);
                                    pot = 0;
                                }
                                else if(hand1[i].Value > hand0[i].Value)
                                {
                                    text = playerOrder[1].Name + " has a better hand and wins " + pot;
                                    playerOrder[1].ChangeMoney(pot);
                                    pot = 0;
                                }
                            }
                        }

                        // could be a tie
                        if(pot != 0)
                        {
                            playerOrder[0].ChangeMoney(pot / 2);
                            playerOrder[1].ChangeMoney(pot / 2);
                            text = "Tie, each player gets " + pot / 2;
                            if (pot % 2 != 0)
                            {
                                pot = 1;
                            }
                            else
                            {
                                pot = 0;
                            }
                        }
                        break;
                    case 2: // one pair
                        // get the pair for playerOrder[0]
                        int p0Pair = 0;
                        for(int j = 14; j >= 2; j--)
                        {
                            int count = Evaluate.ValueCount(j, hand0);
                            if(count == 2) // found the pair
                            {
                                p0Pair = j;
                                break;
                            }
                        }

                        // do  the same for the other hand
                        int p1Pair = 0;
                        for (int k = 14; k >= 2; k--)
                        {
                            int count = Evaluate.ValueCount(k, hand1);
                            if (count == 2) // found the pair
                            {
                                p1Pair = k;
                                break;
                            }
                        }

                        // which is higher
                        if(p0Pair > p1Pair) // playerOrder[0] wins
                        {
                            text = playerOrder[0].Name + " has a better hand and wins " + pot;
                            playerOrder[0].ChangeMoney(pot);
                        }
                        else if(p1Pair > p0Pair)
                        {
                            text = playerOrder[1].Name + " has a better hand and wins " + pot;
                            playerOrder[1].ChangeMoney(pot);
                        }
                        else
                        {   // need to see what the high
                            // card is aside from the pair
                            // get the cards that are not part of a pair from hand0
                            Card[] h0NotPair = new Card[3];
                            int pos = 0;
                            for(int i = 0; i < hand0.Length; i++)
                            {
                                if(hand0[i].Value != p0Pair)
                                {
                                    h0NotPair[pos] = hand0[i];
                                    pos++;
                                }
                            }

                            // do the same for the next hand
                            Card[] h1NotPair = new Card[3];
                            pos = 0;
                            for (int i = 0; i < hand1.Length; i++)
                            {
                                if (hand1[i].Value != p1Pair)
                                {
                                    h1NotPair[pos] = hand1[i];
                                    pos++;
                                }
                            }

                            // see if high card breakes the tie
                            for (int i = 2; i >= 0; i--)
                            {
                                if (h0NotPair[i].Value != h1NotPair[i].Value)
                                {
                                    if (h0NotPair[i].Value > h1NotPair[i].Value)
                                    {
                                        text = playerOrder[0].Name + " has a better hand and wins " + pot;
                                        playerOrder[0].ChangeMoney(pot);
                                        pot = 0;
                                    }
                                    else if (h1NotPair[i].Value > h0NotPair[i].Value)
                                    {
                                        text = playerOrder[1].Name + " has a better hand and wins " + pot;
                                        playerOrder[1].ChangeMoney(pot);
                                        pot = 0;
                                    }
                                }
                            }

                            // could be a tie
                            if (pot != 0)
                            {
                                playerOrder[0].ChangeMoney(pot / 2);
                                playerOrder[1].ChangeMoney(pot / 2);
                                text = "Tie, each player gets " + pot / 2;
                                if (pot % 2 != 0)
                                {
                                    pot = 1;
                                }
                                else
                                {
                                    pot = 0;
                                }
                            }
                        }
                        break;
                    case 3: // two pair
                        // get the two pair
                        int[] h0Pair = new int[2];
                        int[] h1Pair = new int[2];

                        // get hand0 pairs
                        int pCount = 0;
                        for(int i = 14; i >= 2; i--)
                        {
                            int count = Evaluate.ValueCount(i, hand0);
                            if (count == 2) // found the pair
                            {
                                h0Pair[pCount] = i;
                                pCount++;
                            }
                        }

                        // get the hand1 pairs
                        pCount = 0;
                        for (int i = 14; i >= 2; i--)
                        {
                            int count = Evaluate.ValueCount(i, hand1);
                            if (count == 2) // found the pair
                            {
                                h1Pair[pCount] = i;
                                pCount++;
                            }
                        }
                        // compare the pairs
                        if(h0Pair[0] > h1Pair[0]) // playerOrder[0] wins
                        {
                            text = playerOrder[0].Name + " has a better hand and wins " + pot;
                            playerOrder[0].ChangeMoney(pot);
                        }
                        else if (h1Pair[0] > h0Pair[0]) // playerOrder[1] wins
                        {
                            text = playerOrder[1].Name + " has a better hand and wins " + pot;
                            playerOrder[1].ChangeMoney(pot);
                            pot = 0;
                        }
                        else // tie on the highest pair
                        {
                            // compare the second pair
                            if (h0Pair[1] > h1Pair[1]) // playerOrder[0] wins
                            {
                                text = playerOrder[0].Name + " has a better hand and wins " + pot;
                                playerOrder[0].ChangeMoney(pot);
                                pot = 0;
                            }
                            else if (h1Pair[0] > h0Pair[0]) // playerOrder[1] wins
                            {
                                text = playerOrder[1].Name + " has a better hand and wins " + pot;
                                playerOrder[1].ChangeMoney(pot);
                                pot = 0;
                            }
                            else // tie on the highest pair
                            {
                                playerOrder[0].ChangeMoney(pot / 2);
                                playerOrder[1].ChangeMoney(pot / 2);
                                text = "Tie, each player gets " + pot / 2;
                                // tie overall
                                if (pot % 2 != 0)
                                {
                                    pot = 1;
                                }
                                else
                                {
                                    pot = 0;
                                }
                            }
                        }
                        break;
                    case 4: // three of a kind
                        // get the pair for playerOrder[0]
                        int p0Three = 0;
                        for (int j = 14; j >= 2; j--)
                        {
                            int count = Evaluate.ValueCount(j, hand0);
                            if (count == 3) // found the pair
                            {
                                p0Three = j;
                                break;
                            }
                        }

                        // do  the same for the other hand
                        int p1Three = 0;
                        for (int k = 14; k >= 2; k--)
                        {
                            int count = Evaluate.ValueCount(k, hand1);
                            if (count == 3) // found the three cards
                            {
                                p1Three = k;
                                break;
                            }
                        }

                        // which is higher - no possibility of a tie
                        if (p0Three > p1Three) // playerOrder[0] wins
                        {
                            text = playerOrder[0].Name + " has a better hand and wins " + pot;
                            playerOrder[0].ChangeMoney(pot);
                        }
                        else
                        {
                            text = playerOrder[1].Name + " has a better hand and wins " + pot;
                            playerOrder[1].ChangeMoney(pot);
                        }
                        pot = 0;
                        break;
                    case 5: // straight
                        // compare the top card - if one is higher than the other, that
                        // player is the winner. Otherwise, there is a tie
                        if (hand0[0].Value > hand1[0].Value)
                        {
                            text = playerOrder[0].Name + " has a better hand and wins " + pot;
                            playerOrder[0].ChangeMoney(pot);
                            pot = 0;
                        }
                        else if (hand1[0].Value > hand0[0].Value)
                        {
                            text = playerOrder[1].Name + " has a better hand and wins " + pot;
                            playerOrder[1].ChangeMoney(pot);
                            pot = 0;
                        }
                        else // tie
                        {
                            playerOrder[0].ChangeMoney(pot / 2);
                            playerOrder[1].ChangeMoney(pot / 2);
                            text = "Tie, each player gets " + pot / 2;
                            if (pot % 2 != 0)
                            {
                                pot = 1;
                            }
                            else
                            {
                                pot = 0;
                            }
                        }
                        break;
                    case 6: // flush
                        // locate the high cards and keep testing until you 
                        // either have a tie or a winner
                        // tie flag
                        Boolean tie = true;
                        for(int i = 4; i >= 0; i--)
                        {
                            if(hand0[i].Value != hand1[i].Value)
                            {
                                // determine the winner 
                                if(hand0[i].Value > hand1[i].Value)
                                {
                                    text = playerOrder[0].Name + " has a better hand and wins " + pot;
                                    playerOrder[0].ChangeMoney(pot);
                                    pot = 0;
                                }
                                else
                                {
                                    text = playerOrder[1].Name + " has a better hand and wins " + pot;
                                    playerOrder[1].ChangeMoney(pot);
                                    pot = 0;
                                }
                                // not a tie
                                tie = false;
                                break; // exit loop
                            }
                        }
                        // handle a tie
                        if(tie == true)
                        {
                            playerOrder[0].ChangeMoney(pot / 2);
                            playerOrder[1].ChangeMoney(pot / 2);
                            text = "Tie, each player gets " + pot / 2;
                            if (pot % 2 != 0)
                            {
                                pot = 1;
                            }
                            else
                            {
                                pot = 0;
                            }
                        }
                        break;
                    case 7: // full house
                        // get the two pair
                        int h0FH = 0;
                        int h1FH = 0;

                        // get hand0 triple
                        for (int i = 14; i >= 2; i--)
                        {
                            int count = Evaluate.ValueCount(i, hand0);

                            if (count == 3) // found the triple
                            {
                                h0FH = i;
                            }
                        }

                        // get the hand1 triple

                        for (int i = 14; i >= 2; i--)
                        {
                            int count = Evaluate.ValueCount(i, hand1);

                            if (count == 3) // found the triple
                            {
                                h1FH = i;
                            }
                        }
                        // compare the triples
                        if (h0FH > h1FH) // playerOrder[0] wins
                        {
                            text = playerOrder[0].Name + " has a better hand and wins " + pot;
                            playerOrder[0].ChangeMoney(pot);
                        }
                        else // playerOrder[1] wins
                        {
                            text = playerOrder[1].Name + " has a better hand and wins " + pot;
                            playerOrder[1].ChangeMoney(pot);
                        }
                        pot = 0;
                        break;
                    case 8: // four of a kind
                        // get the pair for playerOrder[0]
                        int p0Four = 0;
                        for (int j = 14; j >= 2; j--)
                        {
                            int count = Evaluate.ValueCount(j, hand0);
                            if (count == 4) // found the  4 cards
                            {
                                p0Four = j;
                                break;
                            }
                        }

                        // do  the same for the other hand
                        int p1Four = 0;
                        for (int k = 14; k >= 2; k--)
                        {
                            int count = Evaluate.ValueCount(k, hand1);
                            if (count == 4) // found the pair
                            {
                                p1Four = k;
                                break;
                            }
                        }

                        // which is higher - no possible tie
                        if (p0Four > p1Four) // playerOrder[0] wins
                        {
                            text = playerOrder[0].Name + " has a better hand and wins " + pot;
                            playerOrder[0].ChangeMoney(pot);
                        }
                        else
                        {
                            text = playerOrder[1].Name + " has a better hand and wins " + pot;
                            playerOrder[1].ChangeMoney(pot);
                        }
                        pot = 0;
                        break;
                    case 9: // straight flush
                        // compare the top card - if one is higher than the other, that
                        // player is the winner. Otherwise, there is a tie
                        if(hand0[4].Value > hand1[4].Value)
                        {
                            text = playerOrder[0].Name + " has a better hand and wins " + pot;
                            playerOrder[0].ChangeMoney(pot);
                            pot = 0;
                        }
                        else if (hand1[4].Value > hand0[4].Value)
                        {
                            text = playerOrder[1].Name + " has a better hand and wins " + pot;
                            playerOrder[1].ChangeMoney(pot);
                            pot = 0;
                        }
                        else // tie
                        {
                            playerOrder[0].ChangeMoney(pot / 2);
                            playerOrder[1].ChangeMoney(pot / 2);
                            text = "Tie, each player gets " + pot / 2;
                            if (pot % 2 == 0)
                            {
                                pot = 1;
                            }
                            else
                            {
                                pot = 0;
                            }
                        }
                        break;
                    case 10: // royal flush
                        // automatic tie - split the pot
                        playerOrder[0].ChangeMoney(pot / 2);
                        playerOrder[1].ChangeMoney(pot / 2);
                        text = "Tie, each player gets " + pot / 2;
                        if (pot % 2 != 0)
                        {
                            pot = 1;
                        }
                        else
                        {
                            pot = 0;
                        }
                        break;
                }
            }

            // return results
            return text;
        }

        // check if a betting action is valid
        private bool CheckAction(string phase, List<PlayerAction> actions, PlayerAction action)
        {
            if(actions.Count == 0)
            {
                // first action of the round is special
                if(action.ActionName == "check" || action.ActionName == "fold" ||
                    action.ActionName == "bet")
                {
                    return true;
                }
                // otherwise a bad entry
                return false;
            }

            PlayerAction previous = actions[actions.Count - 1];
            // see if we have an invalid action sequence
            if (previous.ActionPhase != phase)
            {
                // first action in the betting phase - must be true
                return true;
            }
            else if(action.ActionName == "check" && 
                (previous.ActionName == "bet" ||
                 previous.ActionName == "raise" ||
                 previous.ActionName == "call"))
            {
                // can't check after a bet/raise/call
                return false;
            }
            else if((previous.ActionName != "bet" && 
                    previous.ActionName != "raise" &&
                    previous.ActionName != "call") &&
                    (action.ActionName == "raise" || 
                     action.ActionName == "call"))
            {
                // need to have a previous bet or raise 
                // to have a raise/call action
                return false;
            }

            // if not a bad case, then the action is OK
            return true;
        }

        // check over the last set of actions to see if we are
        // done betting
        private bool EvaluateActions(List<PlayerAction> actions,string phase)
        {
            // special case - the last player is the dealer and calls
            // then we are done
            PlayerAction lastAction = actions[actions.Count - 1];
            if(lastAction.ActionPhase == phase && 
                (phase == "Bet1" || phase == "Bet2") &&
                lastAction.ActionName == "call")
            {
                // see if the player is the dealer
                if(lastAction.Name == p0.Name && p0.Dealer == true)
                {
                    return true; // dealer called - betting ends
                }
                else if(lastAction.Name == p1.Name && p1.Dealer == true)
                {
                    return true; // dealer called - betting ends
                }
            }

            // look at the last two actions for the given phase
            int count = 0;
            PlayerAction pa0 = null;
            PlayerAction pa1 = null;
            for (int i = actions.Count - 1; i >= 0; i--)
            {
                PlayerAction temp = actions[i];
                if (temp.ActionPhase == phase)
                {
                    if (count == 0)
                    {
                        pa0 = temp;
                        count++;
                    }
                    else
                    {
                        pa1 = temp;
                        break;
                    }
                }
            }

            // check for end conditions
            if(pa1 != null && pa1.ActionName == "fold")
            {
                return true; // somebody folded
            }
            else if (pa0 != null && pa0.ActionName == "fold")
            {
                return true; // somebody folded
            }
            else if(pa1 != null && pa0 != null && pa1.ActionName == "check" && pa0.ActionName == "check")
            {
                return true; // both checked
            }
            else if (pa1 != null && pa0 != null && pa1.ActionName == "call" && pa0.ActionName == "call")
            {
                return true; // both checked
            }

            // if none of the above, still betting
            return false;
        }

        // output to console and file
        private void ResultWriter(string msg)
        {
            Console.WriteLine(msg);
            tournamentResults.WriteLine(msg);
            tournamentResults.Flush();
        }
    }
}
