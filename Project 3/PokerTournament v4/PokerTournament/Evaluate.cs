using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerTournament
{
    // contains static methods for hand evaluation
    class Evaluate
    {
        // rate poker hands from 1 to 10, 10 being a Royal Flush and 
        // 1 being a High Card. Also returns the high card to break ties
        public static int RateAHand(Card[] hand, out Card highCard)
        {
            highCard = null;

            // sort the hand
            SortHand(hand);

            // Royal Flush 
            if (SameSuit(hand) == true && hand[0].Value == 10 &&
                hand[1].Value == 11 && hand[2].Value == 12 &&
                hand[3].Value == 13 && hand[4].Value == 14)
            {
                highCard = hand[4];
                return 10;
            }

            // straight flush
            if (SameSuit(hand) == true &&
                hand[0].Value == hand[1].Value - 1 &&
                hand[0].Value == hand[2].Value - 2 &&
                hand[0].Value == hand[3].Value - 3 &&
                hand[0].Value == hand[4].Value - 4)
            {
                highCard = hand[4];
                return 9;
            }

            // four of a kind
            for (int i = 2; i < 15; i++)
            {
                if (ValueCount(i, hand) == 4)
                {
                    highCard = hand[4];
                    return 8;
                }
            }

            // full house
            // Get the three cards first
            int threeValue = 0;
            for (int i = 2; i < 15; i++)
            {
                if (ValueCount(i, hand) == 3)
                {
                    threeValue = i;
                }
            }

            if (threeValue != 0)
            {
                // now get the two cards
                for (int i = 2; i < 15; i++)
                {
                    if (i != threeValue && ValueCount(i, hand) == 2)
                    {
                        highCard = hand[4];
                        return 7;
                    }
                }
            }

            // flush - got this far so not a Royal or Straight Flush
            if (SameSuit(hand) == true)
            {
                highCard = hand[4];
                return 6;
            }

            // straight
            if (hand[0].Value == hand[1].Value - 1 &&
               hand[0].Value == hand[2].Value - 2 &&
               hand[0].Value == hand[3].Value - 3 &&
               hand[0].Value == hand[4].Value - 4)
            {
                highCard = hand[4];
                return 5;
            }

            // three of a kind
            for (int i = 2; i < 15; i++)
            {
                if (ValueCount(i, hand) == 3)
                {
                    highCard = hand[4];
                    return 4;
                }
            }

            // two pair
            // Get the first pair
            int firstPair = 0;
            for (int i = 2; i < 15; i++)
            {
                if (ValueCount(i, hand) == 2)
                {
                    firstPair = i;
                }
            }

            // now get the second pair
            for (int i = 2; i < 15; i++)
            {
                if (i == firstPair) continue; // skip this value
                if (ValueCount(i, hand) == 2)
                {
                    highCard = hand[4];
                    return 3;
                }
            }

            // one pair
            for (int i = 2; i < 15; i++)
            {
                if (ValueCount(i, hand) == 2)
                {
                    highCard = hand[4];
                    return 2;
                }
            }

            // must be a high card
            highCard = hand[4];
            return 1;
        }

        // helper method - sort a hand by value
        // Pseudocode:
        /*procedure bubbleSort(A : list of sortable items)
            n = length(A)
            repeat
            swapped = false
            for i = 1 to n-1 inclusive do
                if A[i - 1] > A[i] then
                    swap(A[i - 1], A[i])
                    swapped = true
                end if
            end for
            n = n - 1
            until not swapped
        end procedure
        */
        public static void SortHand(Card[] hand)
        {
            // simple bubble sort - with 5 cards almost as fast as other
            // types of sorts
            int n = hand.Length;
            Boolean swapped = false;
            do
            {
                swapped = false; // reset flag for next iteration
                for (int i = 1; i < n; i++)
                {
                    if (hand[i - 1].Value > hand[i].Value) // do we swap?
                    {
                        Card temp = hand[i - 1];
                        hand[i - 1] = hand[i];
                        hand[i] = temp;
                        swapped = true;
                    }
                }
                n--; // largest value is at the end of the array
            } while (swapped == true);
        }

        // helper method - see if hand is all the same suit
        // could mean a Flush, Straight Flush, or Royal Flush
        private static Boolean SameSuit(Card[] hand)
        {
            // are all cards from the same suit
            for (int i = 1; i < hand.Length; i++)
            {
                if (hand[i].Suit != hand[0].Suit)
                {
                    return false;
                }
            }

            // finished loop - all cards are the same suit
            return true;
        }

        // helper method - do cards match values given
        private static Boolean MatchValues(int[] values, Card[] hand)
        {
            // outer loop - value to test
            for (int i = 0; i < values.Length; i++)
            {
                // inner loop tests for a match
                Boolean match = false;
                for (int j = 0; j < hand.Length; j++)
                {
                    if (hand[j].Value == values[i])
                    {
                        match = true;
                        break;
                    }
                }
                // if there is no match for a value - we're done
                return false;
            }
            // if we match all of the values, return true
            return true;
        }

        // count the number of occurences of a value
        public static int ValueCount(int value, Card[] hand)
        {
            // count the occurences of a value
            int count = 0;
            for (int i = 0; i < hand.Length; i++)
            {
                if (hand[i].Value == value)
                    count++;
            }
            return count;
        }

        // list a hand of cards
        public static string ListHand(Card[] hand)
        {
            // string to hold output
            string text = "";

            // evaluate the hand
            Card highCard = null;
            int rank = Evaluate.RateAHand(hand, out highCard);

            // list your hand
            text += "Hand Rank: " + rank + " \n";
            for (int i = 0; i < hand.Length; i++)
            {
                text += hand[i].ToString() + " ";
            }
            text += "\n";
            return text;
        }
    }
}
