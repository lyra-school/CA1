/* Name: Lyra Karsaj
 * Date: 28/10/2023
 * Desc: Continuous Assessment 1 - Blackjack implementation
 */
namespace CA1
{
    // Possible choices during each turn
    // NOTE: I use 'Hit' and 'Stand' instead of Stick/Twist, though they mean the same thing, as that terminology is more familiar to me
    public enum TurnChoice {
        Hit,
        Stand
    }

    // Possible choices at the end of each round
    public enum ReplayChoice {
        Replay,
        Quit
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            // Hardcoded Windows file path to save the history log to
            string path = "C:\\test\\blackjack-history.txt";

            // Creates file in the above path if it doesn't already exist
            if(!File.Exists(path))
            {
                File.Create(path).Dispose(); 
            }

            // Records a timestamp at each execution of the program
            string session = "-----------------\nSESSION STARTED ON " + DateTime.Now + "\n-----------------\n\n";
            File.AppendAllText(path, session);

            // Creates both players + the deck
            Player player = new Player("Player", path);
            Player dealer = new Player("Dealer", path);

            Deck deck = new Deck();

            // Main game loop
            while(true)
            {
                // Shuffles the deck and reset draw count to 0
                deck.Shuffle();

                // Announces player + deal the initial 2 cards
                Console.WriteLine(player.Name + " is playing...\n");
                player.DrawCard(deck);
                player.DrawCard(deck);

                // Calculates the value of player's hand after the initial draw
                // No need to check for a bust as it's mathematically impossible to bust from two cards
                int currentValue = player.BlackjackValueCalc();

                // Loop Hit/Stand selection until the player either stands or goes bust
                while (true)
                {
                    // Calls the input function, breaks the loop if player stands
                    TurnChoice turnChoice = CheckTurn();
                    if (turnChoice == TurnChoice.Stand)
                    {
                        break;
                    }

                    // Draws a new card, calculates and checks for a bust
                    player.DrawCard(deck);
                    currentValue = player.BlackjackValueCalc();
                    if (currentValue > 21)
                    {
                        break;
                    }
                }

                // If the player ended the previous loop to a bust, the dealer doesn't need to play
                if (currentValue > 21)
                {
                    // Records the loss both in console and in the log file
                    Console.WriteLine("\nYou lost!\n");
                    string lossVariant1 = $"You lost with {currentValue} points; you went bust.\n\n";
                    File.AppendAllText(path, lossVariant1);
                } else
                {
                    // If the player is still in game, announces the dealer playing and draws two initial cards
                    Console.WriteLine(dealer.Name + " is playing...\n");
                    dealer.DrawCard(deck);
                    dealer.DrawCard(deck);

                    // Calculates the value of dealer's hand, draws more cards for them until they reach or
                    // go over the total value of 17
                    int dealerValue = dealer.BlackjackValueCalc();
                    while (dealerValue < 17)
                    {
                        dealer.DrawCard(deck);
                        dealerValue = dealer.BlackjackValueCalc();
                    }

                    // If player has more points than dealer OR the dealer went bust, player wins
                    if (currentValue > dealerValue || dealerValue > 21)
                    {
                        // Records the win both in console and in the log file
                        Console.WriteLine("\nYou won!\n");
                        string win = $"You won with {currentValue} points; dealer lost with {dealerValue}.\n\n";
                        File.AppendAllText(path, win);
                    }
                    // If the two are tied and no one went bust, no one wins
                    else if (currentValue == dealerValue)
                    {
                        // Records the tie both in console and in the log file
                        Console.WriteLine("\nYou tied!\n");
                        string tie = $"You tied with the dealer, with {currentValue} points.\n\n";
                        File.AppendAllText(path, tie);
                    }
                    // Only way to reach this default is if the player has less points than the dealer
                    else
                    {
                        // Records the loss both in console and in the log file
                        Console.WriteLine("\nYou lost!\n");
                        string lossVariant2 = $"You lost with {currentValue} points; dealer won with {dealerValue}.\n\n";
                        File.AppendAllText(path, lossVariant2);
                    }
                }

                // Calls the input method, breaks loop if player wants to quit
                if(CheckReplay() == ReplayChoice.Quit)
                {
                    break;
                }

                // Clears both players' hands
                player.ClearHand();
                dealer.ClearHand();
            }
        }

        /// <summary>
        /// Prompts the user to hit or stand. Method loops until the input provided is valid.
        /// </summary>
        /// <returns>A valid enum value depending on what the user chose</returns>
        public static TurnChoice CheckTurn()
        {
            while (true)
            {
                Console.WriteLine("Do you want to hit or stand - h/s?\n");
                string? input = Console.ReadLine()?.ToLower();
                if (input != "h" && input != "s")
                {
                    Console.WriteLine("\nInput not recognized. Try again.\n");
                    continue;
                }

                Console.Write("\n");

                // If the input is invalid, the method wouldn't have reached this point; the only choices are 'h' or 's'
                // If the evaluation is true, return enum value for hit; if false, return stand
                return input == "h" ? TurnChoice.Hit : TurnChoice.Stand;
            }
        }

        /// <summary>
        /// Asks the user whether they want to quit. Method loops until the input provided is valid.
        /// </summary>
        /// <returns>A valid enum value depending on what the user chose</returns>
        public static ReplayChoice CheckReplay()
        {
            while (true)
            {
                Console.WriteLine("Do you want to play again - y/n?\n");
                string? input = Console.ReadLine()?.ToLower();
                if (input != "y" && input != "n")
                {
                    Console.WriteLine("\nInput not recognized. Try again.\n");
                    continue;
                }

                Console.Write("\n");

                // If the input is invalid, the method wouldn't have reached this point; the only choices are 'y' or 'n'
                // If the evaluation is true, return enum value for replay; if false, return quit
                return input == "y" ? ReplayChoice.Replay : ReplayChoice.Quit;
            }
        }
    }
}