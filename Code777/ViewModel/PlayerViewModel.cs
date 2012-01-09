using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Code777.Model;
using GalaSoft.MvvmLight;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace Code777.ViewModel
{
    /// <summary>
    /// A view model for the player object
    /// </summary>
    public class PlayerViewModel : ViewModelBase
    {

        /// <summary>
        /// refernfce to player
        /// </summary>
        private Player _person;
        
        /// <summary>
        /// player hand
        /// </summary>
        private ObservableCollection<CardViewModel> _hand;

        
        /// <summary>
        /// Player image
        /// </summary>
        public Uri ImagePath
        {
            get
            {
                return new Uri(_person.Image, UriKind.Relative);

            }
        }
        /// <summary>
        /// Get card of palyer
        /// </summary>
        public List<Card> Hand
        {

            get
            {
                return _person.MyCards;
            }

        }
        /// <summary>
        /// Get player model
        /// </summary>
        public Player Player
        {

            get
            {
                return _person;
            }
        }
        /// <summary>
        /// Get the cards of the player
        /// </summary>
        public ObservableCollection<CardViewModel> Hand2
        {
            get
            {
                return _hand;
            }
            private set
            {
                _hand = value;
                RaisePropertyChanged("Hand2");

            }
        }

        /// <summary>
        /// get palyers name
        /// </summary>
        public string Name
        {
            get
            {
                return _person.Name;
            }

        }
        /// <summary>
        /// update playe's socre
        /// </summary>
        public int Score
        {
            get
            {
                return _person.Score;

            }
            set
            {
                _person.Score++;

                RaisePropertyChanged("Score");

            }



        }


        /// <summary>
        /// Construcot
        /// </summary>
        /// <param name="p"></param>
        public PlayerViewModel(Player p)
        {
            _person = p;
            UpdateCards();

        }

        /// <summary>
        /// Update players card
        /// </summary>
        public void UpdateCards()
        {

            ObservableCollection<CardViewModel> t = new ObservableCollection<CardViewModel>();
            foreach (Card c in _person.MyCards)
            {
                t.Add(new CardViewModel(c));

            }
            Hand2 = t;
        }


    }
}
