namespace CA1
{
    /// <summary>
    /// Class type that holds a collection of Card objects, means to generate it, and a Random object for associated shuffling methods.
    /// </summary>
    public class Deck {

        // Attributes to hold a collection of cards, Random object, and an object-specific count of how many cards have been drawn from it
        private readonly Card[] _cards;
        private readonly Random _random;

        // Changed by in-class functions; resets to zero with a Shuffle() call in assumption that the deck is only shuffled
        // after the cards have been all returned
        private int _drawIndex;
        
        /// <summary>
        /// Constructs a Deck object with a Random object of choice.
        /// </summary>
        /// <param name="random">The Random object to use in any shuffling.</param>
        public Deck(Random random) {
            _cards = GenerateUnshuffledDeck();
            _random = random;
        }

        /// <summary>
        /// Constructs a Deck object with a newly made Random object.
        /// </summary>
        public Deck() : this(new Random()) {}

        /// <summary>
        /// Shuffles the card collection using the Knuth shuffle algorithm. Resets the count of cards drawn to 0.
        /// </summary>
        public void Shuffle() {
            // Gets size of the collection
            // For element retrieving operations, it will not result in IndexOutOfBounds as it's decremented in the following loop
            int len = _cards.Length;

            // Swaps cards in order from the highest index with a random index 
            while (len > 1) {
                int randomIndex = _random.Next(len--);
                Card old = _cards[len];
                _cards[len] = _cards[randomIndex];
                _cards[randomIndex] = old;
            }

            // The count is reset to zero because this function is assumed to be called after all cards are returned to the deck
            _drawIndex = 0;
        }

        /// <summary>
        /// Selects a card from the beginning of the deck, with an offset depending on how many cards have been drawn already.
        /// Throws an exception if the amount of drawn cards exceeds the size of the deck, as it's mathematically impossible to
        /// use up the entire deck in regular Blackjack.
        /// </summary>
        /// <returns>Next Card in order</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public Card Draw() {
            // If count of drawn cards exceeds length of collection, throws InvalidOperationException
            if (_drawIndex >= _cards.Length) {
                throw new InvalidOperationException("No cards left");
            }

            // Increments the count of drawn cards when it returns a card in order
            return _cards[_drawIndex++];
        }

        /// <summary>
        /// Returns a Boolean value depending on whether there are cards left in the deck.
        /// Empty decks always return false.
        /// </summary>
        /// <returns>'True' if there are cards available, 'false' if otherwise</returns>
        public bool HasCard() {
            return _drawIndex < _cards.Length;
        }

        /// <summary>
        /// Returns a count of cards that are left.
        /// </summary>
        /// <returns>Integer value for cards remaining to be drawn</returns>
        public int CardsLeft() {
            return _cards.Length - _drawIndex;
        }

        /// <summary>
        /// Generates a standard deck (array) of Card objects every time an object is instantiated
        /// based on enums that hold all possible card ranks and suits.
        /// Accessible only by members of Deck class.
        /// </summary>
        /// <returns>A newly generated array of cards</returns>
        private static Card[] GenerateUnshuffledDeck() {
            // A standard deck is 52 cards large, so is the array that holds it
            Card[] cardArray = new Card[52];

            // Generates all cards based on rank + suit combinations
            int i = 0;
            foreach (CardValue value in Enum.GetValues(typeof(CardValue))) {
                foreach (CardSuit suit in Enum.GetValues(typeof(CardSuit))) {
                    cardArray[i++] = new Card(value, suit);
                }
            }

            return cardArray;
        }
    }
}