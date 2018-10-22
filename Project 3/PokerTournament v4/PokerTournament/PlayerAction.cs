using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerTournament
{
    // a record of an action a player takes in the game
    class PlayerAction
    {
        // attributes
        private string name; // player name - can idenify who did the action
        private string actionPhase; // allowed values - Bet1, Bet2, Draw
        private string actionName;  // Depends on the phase
                                    // Bet1/Bet2: bet, check, raise, fold, call
                                    // Draw: stand pat, draw
        private int amount; // amount bet or raised if Bet1 or Bet2, number cards in Draw

        // constructor
        public PlayerAction(string nm,string ap, string an, int amt)
        {
            name = nm;
            actionPhase = ap;
            actionName = an;
            amount = amt;
        }

        // properties - get only
        public string Name { get { return name; } }
        public string ActionPhase { get { return actionPhase; } }
        public string ActionName { get { return actionName; } }
        public int Amount { get { return amount; } }

        // override ToString
        public override string ToString()
        {
            return "Player: " + name +
                " Action Phase: " + actionPhase +
                " Action Name: " + actionName +
                " Amount: " + amount;
        }
    }
}
