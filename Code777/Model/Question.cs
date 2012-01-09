using System;

namespace Code777.Model
{


    /// <summary>
    /// A question in the code77 game
    /// </summary>
    public class Question
    {
        #region Properties
        /// <summary>
        /// id of the qustion
        /// </summary>
        private int _id;
        public int QuestionId
        {
            get
            {
                return _id;
            }

        }

        /// <summary>
        /// 
        /// strig of the quesiton
        /// </summary>
        private string _questionString;
        public string QuestionString
        {
            get
            {
                return _questionString;
            }
        }

        #endregion

        /// <summary>
        /// Construcor
        /// </summary>
        /// <param name="id"></param>
        /// <param name="question"></param>
        public Question(int id, string question)
        {

            this._id = id;
            this._questionString = question;

        }

        /// <summary>
        /// To string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return String.Format("Q{0} : {1}", _id, _questionString);
        }


    }
}
