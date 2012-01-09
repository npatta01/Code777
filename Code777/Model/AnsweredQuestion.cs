

namespace Code777.Model
{
    /// <summary>
    /// AnsweredQuestion: A class that represents a question that
    /// was already answered
    /// </summary>
    /// <author> Nidhin Pattaniyil <ntp5633@cs.rit.edu</author>
    public class AnsweredQuestion
    {

        /// <summary>
        /// Construtor for Answer question
        /// </summary>
        /// <param name="referencedQuestion">Question that was asked</param>
        /// <param name="player">player who answered</param>
        /// <param name="answer">answer given to that question</param>
        /// <param name="_turnAnswered"> turn question was answered</param>
        public AnsweredQuestion(Question referencedQuestion, Player player, string answer, int turnanswered)
        {
            _question = referencedQuestion;
            _player = player;
            _answer = answer;
            this._turnAnswered = turnanswered;
        }

        #region Properties
        /// <summary>
        /// Reference to Question
        /// </summary>
        private Question _question;
        public Question Question
        {
            get
            {
                return _question;
            }

        }

        /// <summary>
        /// Reference to Player who answered question
        /// </summary>
        private Player _player;
        public Player Player
        {
            get
            {
                return _player;
            }
        }

        /// <summary>
        /// Reference to answer given
        /// </summary>
        private string _answer;
        public string Answer
        {
            get
            {
                return _answer;
            }
        }

        /// <summary>
        /// Turn question was answered
        /// </summary>
        private int _turnAnswered;
        public string TurnAnswered
        {
            get
            {
                return "Turn " + _turnAnswered;
            }
        }
        #endregion
    }
}
