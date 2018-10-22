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

        //override BettingRound1
        public override PlayerAction BettingRound1(List<PlayerAction> actions, Card[] hand)
        {
            return null;
        }

        //override Draw
        public override PlayerAction Draw(Card[] hand)
        {
            return null; 
        }

        //override BettingRound2

         public override PlayerAction BettingRound2(List<PlayerAction> actions, Card[] hand)
        {
            return null;
        }
    }
}
