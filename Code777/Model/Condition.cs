
namespace Code777.Model
{
    /// <summary>
    /// A class that encapulates a condition in the question
    /// in the Code 777 game. Currenlty only used forr questions that
    /// asks if the user sees more of one card or of the other
    /// </summary>
    /// <author>Nidhin Pattaniyil </author>
    public class Condition
    {
        #region Properties
        /// <summary>
        /// Type of Question this condition is for
        /// </summary>
        private QuestionType _questionType;
        public QuestionType QuestionType
        {
            get
            {
                return _questionType;
            }

        }

        /// <summary>
        /// Left Card in the condition if any
        /// </summary>
        private Card _leftCard;
        public Card LeftCard
        {
            get
            {
                return _leftCard;
            }
        }

        /// <summary>
        /// Right condition in the card if any
        /// </summary>
        private Card _rightCard;
        public Card RightCard
        {
            get
            {
                return _rightCard;
            }
        }

        #endregion

        /// <summary>
        /// Create a new condition
        /// </summary>
        /// <param name="qt"></param>
        /// <param name="leftCard"></param>
        /// <param name="rightCard"></param>
        public Condition(QuestionType qt, Card leftCard, Card rightCard)
        {
            _questionType = qt;
            _leftCard = leftCard;
            _rightCard = rightCard;

        }

    }
}
