using GalaSoft.MvvmLight;
using Code777.Model;
using System.Collections.ObjectModel;
using Code777.View;
using System.Xml.Linq;
using System.Linq;
using System;

namespace Code777.ViewModel
{
   /// <summary>
   /// View Model for the starting Page. 
   /// Currently not implemented because only option to start a new game
   /// </summary>
    public class MainViewModel : ViewModelBase
    {
        

        /// <summary>
        /// The <see cref="WelcomeTitle" /> property's name.
        /// </summary>
        public const string WelcomeTitlePropertyName = "WelcomeTitle";

        private string _welcomeTitle = string.Empty;

        /// <summary>
        /// Gets the WelcomeTitle property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string WelcomeTitle
        {
            get
            {
                return _welcomeTitle;
            }

            set
            {
                if (_welcomeTitle == value)
                {
                    return;
                }

                _welcomeTitle = value;
                RaisePropertyChanged(WelcomeTitlePropertyName);
            }
        }

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            

            if (IsInDesignMode)
            {
                Card a = new Card(Shape.DIAMOND, Color.BLACK, 7, "/ImagePath/pink_7.jpg");
                Card b = new Card(Shape.DIAMOND, Color.BLACK, 6, "/ImagePath/pink_6.jpg");
                

                CardsViewModel cards = new CardsViewModel();
                hand = cards.Hand2;

            }
            else
            {
                Card a = new Card(Shape.DIAMOND, Color.BLACK, 7, "/ImagePath/pink_7.jpg");
                Card b = new Card(Shape.DIAMOND, Color.BLACK, 6, "/ImagePath/pink_6.jpg");
               
                CardsViewModel cards = new CardsViewModel();
                hand = cards.Hand2;

            }


            
            
            
        }


        #region DATA
        public ObservableCollection<CardViewModel> _hand;
        public ObservableCollection<CardViewModel> hand
        {
            get { return _hand; }
            set
            {
                _hand = value;
                RaisePropertyChanged("Hand");
            }
        }
        #endregion
      
    }
}