using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using Code777.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using MVVM.Framework;
using System.Linq.Expressions;
using System.Linq;
namespace Code777.ViewModel
{
  /// <summary>
  /// View model for the game class. (Main View Model)
  /// </summary>
    public class GameViewModel : ViewModelBase
    {
        //reference to navigation service
        private readonly INavigationService _navigationService;
        /// <summary>
        /// Initializes a new instance of the GameViewModel class.
        /// </summary>

        //private variables
        private ObservableCollection<PlayerViewModel> players;
        private int currentState = -1;
        private int humanplayerid = 0;
        private List<String> visualStates;
        private ObservableCollection<string> notes;
        private Game _currentGame;
       
        //Varialbes used in blend in design mode
        private CardsViewModel _allCards;
        private ObservableCollection<CardViewModel> opponent1Card;
        private ObservableCollection<CardViewModel> opponent2Card;
        private ObservableCollection<CardViewModel> opponent3Card;
        private AnsweredQuestion aq;
        /// end
        
        private ObservableCollection<CardViewModel> allCards;
        /// <summary>
        /// get a sample answered question 
        /// </summary>
        
        private AnsweredQuestion sampleAq;


        /// <summary>
        /// Main view model of the game. Does all the navigation and communicate with game model
        /// </summary>
        /// <param name="navigationService"></param>
        public GameViewModel(INavigationService navigationService)
        {
            ///create visual states
            visualStates = new List<string>();
            visualStates.Add("Player1");
            visualStates.Add("Player2");
            visualStates.Add("Player3");
            visualStates.Add("Player4");


            VisualStateName = "Start";
            //create navigation serices
            _navigationService = navigationService;
            _currentGame = new Game();
            notes = new ObservableCollection<string>();

            //create players
            players = new ObservableCollection<PlayerViewModel>();
            players.Add(new PlayerViewModel(_currentGame.ActivePlayers[0]));
            players.Add(new PlayerViewModel(_currentGame.ActivePlayers[1]));
            players.Add(new PlayerViewModel(_currentGame.ActivePlayers[2]));
            players.Add(new PlayerViewModel(_currentGame.ActivePlayers[3]));

            //get all teh cards in the game
            allCards = GetAllCardsinGame();


            ///initialize the command
            NextQuestion = new RelayCommand(() =>
            {
                NextQuestionMethod();


            });


            QuestionsPageNavigate = new RelayCommand(() =>
                {


                    _navigationService.NavigateTo(
                 new Uri(
                     string.Format(ViewModelLocator.QuestionsPage),
                     UriKind.Relative));
                });


            CardManipulationPageNavigate = new RelayCommand(() =>
            {


                _navigationService.NavigateTo(
             new Uri(
                 string.Format(ViewModelLocator.CardManipulationPage),
                 UriKind.Relative));
            });


            NotesNavigatePage = new RelayCommand(() =>
               {
                   _navigationService.NavigateTo(
            new Uri(
                string.Format(ViewModelLocator.NotesPage),
                UriKind.Relative));
               });

            DeleteNote = new RelayCommand<int>((int selected) =>
                {
                    if (selected >= 0 && selected < notes.Count)
                    {
                        notes.RemoveAt(selected);

                    }

                });


            SaveNote = new RelayCommand<string>((string s) =>
                {
                    string g = s.Trim();
                    if (g != string.Empty)
                    {
                        notes.Add(g);
                        _navigationService.GoBack();
                    }

                });

            AddNoteNavigatePage = new RelayCommand(() =>
                {
                    _navigationService.NavigateTo(
            new Uri(
                string.Format(ViewModelLocator.AddNotesPage),
                UriKind.Relative));
                });

            GuessCard = new RelayCommand(GuessCardMethod);

            CheckInput = new RelayCommand<int>(CheckInputCmd);

            System.Diagnostics.Debug.WriteLine("In game constr");


            if (IsInDesignMode)
            {
                
                sampleAq = new AnsweredQuestion(new Question(5, "On how many racks do you see three consecutive numbers? ”"), _currentGame.ActivePlayers[0], "3", 3);

            }

            RaisePropertyChanged("GetOpponent1");
        }


        

        /// <summary>
        /// Get users notes
        /// </summary>
        public ObservableCollection<string> GetNotes
        {
            get
            {
                if (IsInDesignMode)
                {

                    
                    notes.Add("Note 1");
                    notes.Add("Note 2");
                    return notes;

                    
                }
                
                return notes;
            }

        }

        /// <summary>
        /// Get list of observations
        /// </summary>
        public List<string> Observations
        {

            get
            {
                return _currentGame.Observations;
            }
        }

        /// <summary>
        /// get hte next quesiotn
        /// </summary>
        private void NextQuestionMethod()
        {
            currentState = (currentState + 1) % visualStates.Count;

            VisualStateName = visualStates[currentState];
            _currentGame.NextQuestion();

            RaisePropertyChanged("PreviouslyAnsweredQuestion");
            if (_currentGame.IsPlayerTurn())
            {
                _navigationService.NavigateTo(
             new Uri(
                 string.Format(ViewModelLocator.AnswerQuestionPage),
                 UriKind.Relative));
            }



        }

        /// <summary>
        /// check users guess
        /// </summary>
        private void GuessCardMethod()
        {
            List<Card> humancard = players[humanplayerid].Hand;
            ObservableCollection<CardViewModel> cards = AllGameCards;

            var choosencards = cards.Where((c) => c.VisualStateName == "Included").Select((c) => c.Card).ToList();

            DialogMessage message = null;
            if (choosencards.Count < 3)
            {
                message = new DialogMessage("You need to guess three cards at once", null)
                {
                    Button = MessageBoxButton.OK,
                    Caption = "Not Enough Cards"
                };

            }
            else if (choosencards.Count > 3)
            {
                message = new DialogMessage("You choose more than 3 cards", null)
                {
                    Button = MessageBoxButton.OK,
                    Caption = "Too Many Cards"
                };
            }
            else
            {
                int correct = humancard.Except(choosencards).Count();

                if (correct == 0)
                {
                    message = new DialogMessage("You guessed correctly", null)
                    {
                        Button = MessageBoxButton.OK,
                        Caption = "Correct Guess"
                    };
                    players[humanplayerid].Score++;
                    //RaisePropertyChanged("Score");

                }
                else
                {
                    message = new DialogMessage("You guessed wrong", null)
                    {
                        Button = MessageBoxButton.OK,
                        Caption = "Wrong Guess"
                    };

                }
                Player p = players[humanplayerid].Player;
                _currentGame.ReassignCards(p);
                players[humanplayerid].UpdateCards();
            }
            Messenger.Default.Send(message);
            if (players[humanplayerid].Score == 3)
            {

                message = new DialogMessage("You won game", GameWonCallBack)
                {
                    Button = MessageBoxButton.OK,
                    Caption = "Won Game"
                };
                Messenger.Default.Send(message);

            }

        }



        /// <summary>
        /// Get opponent 1's card
        /// </summary>
        public PlayerViewModel GetOpponent1
        {
            get
            {
                return players[1];
            }

        }

        /// <summary>
        /// get opponents card
        /// </summary>
        public PlayerViewModel GetOpponent2
        {
            get
            {
                return players[2];
            }

        }
        /// <summary>
        /// get opponents card
        /// </summary>
        public PlayerViewModel GetOpponent3
        {
            get
            {
                return players[3];
            }

        }

        /// <summary>
        /// helper method to check users anser
        /// </summary>
        /// <param name="num"></param>
        private void CheckInputCmd(int num)
        {
            
            if (_currentGame.CheckAnswer(num))
            {
 RaisePropertyChanged("PreviouslyAnsweredQuestion");
                _navigationService.GoBack();
            }
            else
            {
                var message = new DialogMessage("Sorry Your Answer was Wrong", null)
                {
                    Button = MessageBoxButton.OK,
                    Caption = "Wrong Answer"
                };
                Messenger.Default.Send(message);

            }
        }
        /// <summary>
        /// A call back dialog
        /// </summary>
        /// <param name="result"></param>
        private void DialogMessageCallback(MessageBoxResult result) { }



        /// <summary>
        /// A call back dialog
        /// </summary>
        /// <param name="result"></param>
        private void GameWonCallBack(MessageBoxResult result) {
            //VisualStateName = "Disabled";
            _navigationService.GoBack();
            //_navigationService.GoBack();
        }

        /// <summary>
        /// get current turn
        /// </summary>
        public int CurrentTurn
        {

            get
            {

                return _currentGame.CurrentTurn;
            }

        }
        /// <summary>
        /// get current question
        /// </summary>
        public string CurrentQuestion
        {

            get
            {
                return _currentGame.CurrentQuestion.QuestionString;

            }

        }
        /// <summary>
        /// get chocies for current question
        /// </summary>
        public List<string> CurrentChoices
        {


            get
            {

                return _currentGame.GetAnswerChoices();
            }

        }

        /// <summary>
        /// Get all questions
        /// </summary>
        public List<Question> AllGameQuestions
        {

            get
            {

                return _currentGame.UnSortedQuestion;
            }
        }


        /// <summary>
        /// Visual state of game page
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
        /// A smple answered question. used when designing
        /// </summary>
        public AnsweredQuestion sampleAnweredQuestion
        {

            get
            {
                return aq;

            }

        }

        /// <summary>
        /// get all the cards in the game
        /// </summary>
        /// <returns></returns>
        private ObservableCollection<CardViewModel> GetAllCardsinGame()
        {
            ObservableCollection<CardViewModel> allC = new ObservableCollection<CardViewModel>();
            foreach (Card c in _currentGame.GetAllCards)
            {
                allC.Add(new CardViewModel(c));

            }
            return allC;
        }

        
        /// <summary>
        /// Get all the cards in the Code777 game
        /// </summary>
        public ObservableCollection<CardViewModel> AllGameCards
        {
            get
            {

                return allCards;
            }
            set
            {
                System.Diagnostics.Debug.WriteLine("In set players card method");
                _allCards.Hand2 = value;
                RaisePropertyChanged("PlayersCard");


            }
        }
        /// <summary>
        /// GEt opponent 1's card
        /// </summary>
        public ObservableCollection<CardViewModel> Opponent1Card
        {

            get
            {
                return opponent1Card;

            }
        }

        /// <summary>
        /// GEt opponent 2's card
        /// </summary>
        public ObservableCollection<CardViewModel> Opponent2Card
        {

            get
            {
                return opponent2Card;

            }
        }

        /// <summary>
        /// GEt opponent 3's card
        /// </summary>
        public ObservableCollection<CardViewModel> Opponent3Card
        {

            get
            {
                return opponent3Card;

            }
        }
        /// <summary>
        /// GEt players's card
        /// </summary>
        

        /// <summary>
        /// GEt card for player with passed id
        /// </summary>
        private CardsViewModel GetCards(int playerId)
        {
            CardsViewModel playersCard = new CardsViewModel();
            List<Card> playerCard = _currentGame.GetPlayersCard(playerId);
            ObservableCollection<CardViewModel> playersHand = new ObservableCollection<CardViewModel>();
            foreach (Card p in playerCard)
            {
                playersHand.Add(new CardViewModel(p));

            }
            playersCard.Hand2 = playersHand;
            return playersCard;

        }



        /// <summary>
        /// get all the answered questions 
        /// </summary>
        public ObservableCollection<AnsweredQuestion> AnsweredQuestions
        {
            get
            {
                if (IsInDesignMode)
                {
                    AnsweredQuestion aq=new AnsweredQuestion(new Question(5,"What’s the best phone?"),_currentGame.ActivePlayers[0],"“Wait…there are other phones?”",2);
                    AnsweredQuestion aq2=new AnsweredQuestion(new Question(5,"On how many racks do you see three consecutive numbers? ”"),_currentGame.ActivePlayers[0],"3",3);

                    ObservableCollection<AnsweredQuestion> a =new ObservableCollection<AnsweredQuestion>();
                    a.Add(aq);
                    a.Add(aq2);
                    return a;
                }
                return _currentGame.AnsweredQuestions;

            }


        }

        /// <summary>
        /// Get the previously answered quesetion
        /// </summary>
        public AnsweredQuestion PreviouslyAnsweredQuestion
        {
            get
            {

                if (IsInDesignMode)
                {

                    return sampleAq;
                }

                int size = _currentGame.AnsweredQuestions.Count - 1;
                if (CurrentTurn == -1)
                {
                    return sampleAnweredQuestion;

                }
                return _currentGame.AnsweredQuestions[size];
            }

        }


        #region Commands
        /// <summary>
        /// command to get next question
        /// </summary>
        public RelayCommand NextQuestion
        {
            get;
            private set;
        }

        /// <summary>
        /// comand to navigate to question page
        /// </summary>
        public RelayCommand QuestionsPageNavigate
        {
            get;
            private set;
        }


        /// <summary>
        /// Check users answer
        /// </summary>
        public RelayCommand<int> CheckInput
        {
            get;
            private set;
        }

        /// <summary>
        /// check users guess
        /// </summary>
        public RelayCommand GuessCard
        {
            get;
            private set;

        }
        /// <summary>
        /// Command to navigateto card quess page
        /// </summary>
        public RelayCommand CardManipulationPageNavigate
        {

            get;
            private set;
        }

        /// <summary>
        /// commad to navigate page
        /// </summary>
        public RelayCommand NotesNavigatePage
        {

            get;
            private set;
        }

        /// <summary>
        /// command to delete note
        /// </summary>
        public RelayCommand<int> DeleteNote
        {

            get;
            private set;
        }

        /// <summary>
        /// command to save note
        /// </summary>
        public RelayCommand<string> SaveNote
        {

            get;
            private set;
        }

        /// <summary>
        /// Commande to navigate to add note page
        /// </summary>
        public RelayCommand AddNoteNavigatePage
        {

            get;
            private set;

        }

        #endregion

    }
}