namespace CA1
{
    /// <summary>
    /// Class for objects that are meant to hold both players. Has a name, list and a file path as attributes.
    /// </summary>
    public class Player
    {
        // Attributes; hold player name, their current hand, and path of the file to log history to
        private readonly string _name;
        private readonly List<Card> _hand = new List<Card>();
        private readonly string _path;

        // Getters for all attributes and no setters; there is only one list for card hand and is updated exclusively
        // through a class method, whereas everything else cannot be changed once the object is created
        public string Name
        {
            get { return _name; }
        }
        public List<Card> Hand
        {
            get { return _hand; }
        }

        public string Path
        {
            get { return _path; }
        }

        /// <summary>
        /// Constructor for Player objects. Requires a name for the player and a path to a text file.
        /// </summary>
        /// <param name="name">Name of the player, used in logging.</param>
        /// <param name="path">Path of text file to record history to.</param>
        public Player(string name, string path)
        {
            _name = name;
            _path = path;
        }

        /// <summary>
        /// Adds a card to a player's hand. Draws from a Deck object, which holds a collection of Card objects.
        /// Additionally, it calculates the card's value according to rules of Blackjack and prints it out both to the
        /// console and the history.
        /// </summary>
        /// <param name="deck">A Deck of Cards to draw from.</param>
        public void DrawCard(Deck deck)
        {
            // Selects a card from the Deck and adds it to the player
            Card card = deck.Draw();

            _hand.Add(card);
            
            // Gets the int value from the value enum assigned to a card, which, to be as generic as possible and not specific to Blackjack,
            // internally uses the enum's index as the default int value

            // The "true value" calculation for an individual card is done here
            int internalValue = (int)card.CardValue;
            int blackjackValue = 0;
            if(internalValue >= 10)
            {
                blackjackValue = 10;
            } else if(internalValue == 0)
            {
                blackjackValue = 11;
            } else
            {
                blackjackValue = internalValue + 1;
            }
            
            // Prints a log of results to both the console and the text file
            string log = $"To {_name}, card dealt is " + card.ToString() + $", value {blackjackValue}\n";
            Console.WriteLine(log);
            File.AppendAllText(_path, log);
        }

        /// <summary>
        /// Empties a player's hand (the list of cards they own).
        /// </summary>
        public void ClearHand()
        {
            _hand.Clear();
        }

        /// <summary>
        /// Calculates the total value of cards in a player's hand. Prints out the result to the console and the log file.
        /// Throws ArgumentException when it encounters an unexpected card rank.
        /// </summary>
        /// <returns>Calculated total value of all cards</returns>
        /// <exception cref="ArgumentException"></exception>
        public int BlackjackValueCalc()
        {
            int currentTotal = 0;

            // Returns zero if the hand is empty
            if (!_hand.Any())
            {
                return currentTotal;
            }

            // Keeps count of aces encountered for later calculations
            int numAces = 0;

            foreach(Card card in _hand) {
                
                // Increments the ace count when it passes over an ace card
                if(card.CardValue == CardValue.Ace)
                {
                    numAces++;
                }

                // Assigns value depending on card rank and associated Blackjack rule
                // Throws an exception if it encounters an unexpected rank (doesn't fall into any of the cases)
                switch(card.CardValue)
                {
                    case CardValue.Ace:
                        currentTotal += 11;
                        break;
                    case CardValue.Two:
                    case CardValue.Three:
                    case CardValue.Four :
                    case CardValue.Five:
                    case CardValue.Six:
                    case CardValue.Seven:
                    case CardValue.Eight:
                    case CardValue.Nine:
                    case CardValue.Ten:
                        currentTotal += (int)card.CardValue + 1;
                        break;
                    case CardValue.Jack:
                    case CardValue.Queen:
                    case CardValue.King:
                        currentTotal += 10;
                        break;
                    default:
                        throw new ArgumentException("Invalid card value found!");
                }
            }

            // If the total value is over 21 and there are aces in the hand, subtracts 10 per ace to simulate aces
            // being devalued from 11 to 1 until it reaches a value <21
            while(currentTotal > 21 && numAces > 0)
            {
                currentTotal -= 10;
                numAces--;
            }

            // Prints a log of results to both the console and the text file
            string log = $"{_name}'s score is " + currentTotal + "\n";
            Console.WriteLine(log);
            File.AppendAllText(_path, log);

            // Returns final value of the hand
            return currentTotal;
        }
    }
}