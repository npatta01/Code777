using GalaSoft.MvvmLight;
using System.Collections.ObjectModel;
using Code777.Model;
using Code777.View;

namespace Code777.ViewModel
{
    /// <summary>
    /// A view model for a card
    /// </summary>
    public class CardsViewModel : ViewModelBase
    {


        /// <summary>
        /// refeence to cards in the hand
        /// </summary>
        public ObservableCollection<CardViewModel> Hand2 { get; set; }

        /// <summary>
        /// Initializes a new instance of the CardsViewModel class.
        /// </summary>
        public CardsViewModel()
        {

            if (IsInDesignMode)
            {
                Hand2 = new ObservableCollection<CardViewModel>();
                Card a = new Card(Shape.DIAMOND, Color.BLACK, 3, "/ImagePath/black_3.jpg");
                Card b = new Card(Shape.DIAMOND, Color.BLACK, 5, "/ImagePath/black_5.jpg");
                
                Hand2.Add(new CardViewModel(a));
                Hand2.Add(new CardViewModel(b));
            }
            else
            {
                GetHand();
            }
            
        }
        /// <summary>
        /// Add the carsd to an observabel collection
        /// </summary>
        private void GetHand()
        {
           
            Hand2 = new ObservableCollection<CardViewModel>();
            Card a = new Card(Shape.DIAMOND, Color.BLACK, 3, "/ImagePath/black_3.jpg");
            Card b = new Card(Shape.DIAMOND, Color.BLACK, 5, "/ImagePath/black_5.jpg");
           
            Hand2.Add(new CardViewModel(a));
            Hand2.Add(new CardViewModel(b));
            Hand2.Add(new CardViewModel(b));
        }


    }
}