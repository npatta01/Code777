using GalaSoft.MvvmLight;
using System;
using Code777.Model;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using GalaSoft.MvvmLight.Command;
using System.ComponentModel;
using System.Collections.Generic;
using MVVM.Framework;
namespace Code777.ViewModel
{
    /// <summary>
    /// View model for one card
    /// </summary>
    public class CardViewModel : ViewModelBase
    {
       //private members for current state,etc
        private Card _card;
        private int currentState = 0;
        private List<String> visualStates;
        
        /// <summary>
        /// Constrctor
        /// </summary>
        /// <param name="card"></param>
        public CardViewModel(Card card)
        {
            visualStates = new List<string>();
            visualStates.Add("NotSure");
            visualStates.Add("Excluded");
            visualStates.Add("Included");
            VisualStateName = "NotSure";

            _card = card;
            ChangeState = new RelayCommand(ChangeStateMethod);
        }


        /// <summary>
        /// Simple construcotr
        /// </summary>
        public CardViewModel()
        {
            if (IsInDesignMode)
            {
                _card = new Card(Shape.DIAMOND, Code777.Model.Color.BLACK, 3, "/ImagePath/black_3.jpg");
            }

        }


        /// <summary>
        /// Card model
        /// </summary>
        public Card Card
        {
            get
            {
                return _card;
            }
        }


        /// <summary>
        /// Source of image
        /// </summary>
        public ImageSource _image2
        {
            get
            {
                return new BitmapImage(new Uri(_card.ImagePath, UriKind.Relative));

            }
        }
        /// <summary>
        /// number of card
        /// </summary>
        public string _number
        {

            get
            {
                return _card.Number.ToString();

            }
        }
        /// <summary>
        /// path of iamge for card
        /// </summary>
        public Uri path
        {
            get
            {
                return new Uri(_card.ImagePath, UriKind.Relative);

            }
        }

        /// <summary>
        /// Visual state of card
        /// </summary>
        private string _visualStateName;
        public string VisualStateName
        {
            get { return _visualStateName; }
            set
            {
                if (_visualStateName != value)
                {
                    _visualStateName = value;
                    RaisePropertyChanged(ReflectionUtility.GetPropertyName(() => VisualStateName));
                }
            }
        }

        /// <summary>
        /// Method called to change state of card
        /// </summary>
        public RelayCommand ChangeState
        {
            get;

            private set;

        }

        /// <summary>
        /// Change visual state of card
        /// </summary>
        private void ChangeStateMethod()
        {
            currentState = (currentState + 1) % visualStates.Count;

            VisualStateName = visualStates[currentState];
        }
    }
}