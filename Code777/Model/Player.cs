using System;
using System.Collections.Generic;
using System.Text;
using GalaSoft.MvvmLight;

namespace Code777.Model
{
    /// <summary>
    /// Player class: encapulstes a player in the game
    /// </summary>
    public class Player : ViewModelBase
    {
        //id of player
        private String _id;
        //cards of player
        private List<Card> _myTiles;
        //image path
        private string _image;
        //score
        private int _score;

        /// <summary>
        /// Player construcotr
        /// </summary>
        /// <param name="id"></param>
        /// <param name="image"></param>
        public Player(String id, string image)
        {
            this._id = id;
            _myTiles = new List<Card>();
            _image = image;
            _score = 0;
        }

        /// <summary>
        /// Players card
        /// </summary>
        public List<Card> MyCards
        {
            get
            {
                return _myTiles;
            }
            set
            {
                _myTiles = value;
                

            }

        }

        #region Properties
        /// <summary>
        /// Path of image
        /// 
        /// </summary>
        public String Image
        {

            get
            {

                return _image;
            }
        }

        /// <summary>
        /// Name of player
        /// </summary>
        public string Name
        {
            get
            {
                return _id;

            }


        }
        /// <summary>
        /// Path of player image icon
        /// </summary>
        public Uri ImagePath
        {
            get
            {
                return new Uri(_image, UriKind.Relative);
                
            }
        }

        /// <summary>
        /// Score of the player
        /// </summary>
        public int Score
        {
            get
            {
                return _score;
            }
            set
            {
                if (_score != value)
                {
                    _score = value;
                    //RaisePropertyChanged("Score");
                }

            }

        }
        /// <summary>
        /// To string method
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {

            StringBuilder builder = new StringBuilder();
            foreach (Card cat in _myTiles) // Loop through all strings
            {
                builder.Append(cat).Append("|"); // Append string to StringBuilder
            }
            return String.Format("{0}:{1}", _id, builder.ToString());
        }


        #endregion
    }
}
