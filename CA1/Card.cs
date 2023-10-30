namespace CA1
{
    /// <summary>
    /// Enum containing all possible card ranks in a standard deck.
    /// </summary>
    public enum CardValue
    {
        Ace,
        Two,
        Three,
        Four,
        Five,
        Six,
        Seven,
        Eight,
        Nine,
        Ten,
        Jack,
        Queen,
        King
    }

    /// <summary>
    /// Enum containing all possible card suits in a standard deck.
    /// </summary>
    public enum CardSuit
    {
        Diamonds,
        Hearts,
        Spades,
        Clubs
    }

    /// <summary>
    /// Card object, containing an associated rank and suit.
    /// </summary>
    public class Card
    {
        // Attributes for a rank and suit
        private readonly CardValue _cardValue;
        private readonly CardSuit _cardType;

        // Getters for all attributes
        // No setters as these attributes can't be changed once assigned
        public CardValue CardValue
        {
            get { return _cardValue; }
        }

        public CardSuit CardType
        {
            get { return _cardType; }
        }

        /// <summary>
        /// Constructs a card based on a given value and suit.
        /// </summary>
        /// <param name="value">Value from the CardValue enum.</param>
        /// <param name="suit">Value from the CardSuit enum.</param>
        public Card(CardValue value, CardSuit suit)
        {
            _cardValue = value;
            _cardType = suit;
        }

        /// <summary>
        /// Creates a string representation of a card object and its associated attributes.
        /// </summary>
        /// <returns>String representation of a card and its attributes</returns>
        public override string ToString()
        {
            return $"{CardValue.ToString()} of {CardType.ToString()}";
        }
    }
}