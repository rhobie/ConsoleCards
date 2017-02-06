namespace ConsoleCards
{
    public class Round
    {
        public void ResetPlayersInRound()
        {
            //need to make the player who won the round start the next round but still keep the same play order
            //GameLists.PlayersInRound.Clear();
            foreach (var player in GameLists.SeatingPlan)
            {
                if (!GameLists.PlayersInRound.Contains(player) && player.Hand.Count != 0)
                {
                    GameLists.PlayersInRound.Add(player);
                }
                if (player.Hand.Count == 0)
                {
                    GameLists.InactivePlayers.Add(player);
                }
            }
            //remove inactive players from active player list
            foreach (var player in GameLists.InactivePlayers)
            {
                if (GameLists.SeatingPlan.Contains(player))
                {
                    GameLists.SeatingPlan.Remove(player);
                }
            }
        }

    }

}

