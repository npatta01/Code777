using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Collections.ObjectModel;
using System.Linq;

namespace Code777.Model
{
    /// <summary>
    /// Main class that encapulates a Code777 game
    /// HAs all the logic for the game, and also loads the data
    /// </summary>
    public class Game
    {
        //num of players in the gmae
        private int _numPlayers;
        //current turn
        private int _turn;
        //list of active players
        private List<Player> _players;
        //list of all the cards in the gamse
        private List<Card> _cardDeck;
        //licst of cards in the pile
        private List<Card> _activeCards;
        //list of questions
        private List<Question> _questions;
        //list of unorted questions
        private List<Question> _unsortedQuestion;
        //list of hints for the player
        private List<string> _observations;
        //list of currrently answered questions
        private ObservableCollection<AnsweredQuestion> _answeredQuestions;
        //current player
        private int _currentPlayer;
        //list of friendly choices for numbers
        private List<String> _numberAnswersChoices;
        //list of friendly equlity choices
        private List<String> _compareAnswersChoices;
        //random generator
        private Random random;
        //refernce of question and answers
        private Dictionary<Question, Condition> QuestionReference;
        //currnt answer ofr question
        private int currentQuestionAnswer;


        /// <summary>
        /// Constructor for game
        /// </summary>
        public Game()
        {
            //initialize random generator
            random = new Random(12);
            //set starting turn to -1
            _turn = -1;
            //set currnet player to -1
            _currentPlayer = -1;
            //create the players
            CreatePlayers();

            //load cards from file
            _cardDeck = LoadCards();
            _activeCards = new List<Card>(_cardDeck);
            //shuffle and assign cards to players
            Game.ShuffleList(_activeCards);
            AssignCards();

            //parse the questions and shuffle
            GetQuestions();
            _unsortedQuestion = new List<Question>();
            _unsortedQuestion.AddRange(_questions);
            Game.ShuffleList(_questions);
            GetObservations();
            CreateQuestionChoices();
            //set to to store answered questions
            _answeredQuestions = new ObservableCollection<AnsweredQuestion>();


        }


        #region Properties
        /// <summary>
        /// Get the lsit of friendly hints
        /// </summary>
        public List<string> Observations
        {
            get
            {
                return _observations;

            }
        }

        /// <summary>
        /// Get the list of players
        /// </summary>
        public List<Player> Players
        {
            get
            {
                return _players;
            }

        }

        /// <summary>
        /// Get the list of questions read from the file
        /// </summary>
        public List<Question> UnSortedQuestion
        {

            get
            {
                return _unsortedQuestion;
            }

        }

        /// <summary>
        /// list of active players
        /// </summary>
        public List<Player> ActivePlayers
        {

            get
            {
                return _players;
            }
        }

        /// <summary>
        /// current turn
        /// </summary>
        public int CurrentTurn
        {

            get
            {
                return _turn;

            }

        }

        /// <summary>
        /// Current question
        /// </summary>
        public Question CurrentQuestion
        {
            get
            {
                return _questions[_turn % _questions.Count];
            }
        }

        /// <summary>
        /// Get the list of ansered questions
        /// </summary>
        public ObservableCollection<AnsweredQuestion> AnsweredQuestions
        {
            get
            {
                return _answeredQuestions;
            }


        }
        /// <summary>
        /// Get all the cards in the game
        /// </summary>
        public List<Card> GetAllCards
        {

            get
            {
                return _cardDeck;
            }

        }
        
        #endregion



        #region Methods
        /// <summary>
        /// For the currnt question Get the appropriate answer 
        /// choices
        /// </summary>
        /// <returns></returns>
        public List<string> GetAnswerChoices()
        {

            Condition c = QuestionReference[CurrentQuestion];
            QuestionType qt = c.QuestionType;
            //return choices of numbers from 0 to 3
            if (qt == QuestionType.RACK)
            {

                return _numberAnswersChoices.GetRange(0, _numPlayers);
            }
            else if (qt == QuestionType.COMPARE)
            {
                _compareAnswersChoices = new List<string>();
                _compareAnswersChoices.Add(String.Format("{0} > {1}", c.LeftCard, c.RightCard));
                _compareAnswersChoices.Add(String.Format("{0} <  {1}", c.LeftCard, c.RightCard));
                _compareAnswersChoices.Add(String.Format("{0} =  {1}", c.LeftCard, c.RightCard));
                return _compareAnswersChoices;

            }
            else
            {
                //return choices from 0 to 7
                return _numberAnswersChoices;
            }

        }



        /// <summary>
        /// Get next question
        /// </summary>
        public void NextQuestion()
        {
            _currentPlayer = (_currentPlayer + 1) % (_numPlayers);
            _turn++;
            System.Diagnostics.Debug.WriteLine(String.Format("Turn {0} Player {1}", _turn, _currentPlayer));
            currentQuestionAnswer = AIPlayerAnswerQuestion();

            int correctChoice = currentQuestionAnswer;
            string correct = GetAnswerChoices()[correctChoice];
            _answeredQuestions.Add(new AnsweredQuestion(CurrentQuestion, _players[_currentPlayer], correct, _turn + 1));


        }

        /// <summary>
        /// check is the passed answer is correct
        /// </summary>
        /// <param name="choice"></param>
        /// <returns></returns>
        public bool CheckAnswer(int choice)
        {
            if (currentQuestionAnswer == choice)
            {
                return true;
            }
            return false;

        }

        /// <summary>
        /// Calcualte answer to current question
        /// </summary>
        /// <returns></returns>
        private int AIPlayerAnswerQuestion()
        {
            List<Card>[] list = GetVisibleCard(_currentPlayer);
            List<Card> flat = (from l in list
                               from Card c in l
                               select c).ToList();
            int answer = QuestionProcessor.ProcessQuestion(CurrentQuestion, list, flat, QuestionReference[CurrentQuestion]);
            return answer;


        }

        /// <summary>
        /// is it the player's turn?
        /// </summary>
        /// <returns></returns>
        public Boolean IsPlayerTurn()
        {
            //assume player goes first
            return _turn % 4 == 0;
        }

        /// <summary>
        /// Load card data from CardList.xml
        /// </summary>
        /// <returns></returns>
        private static List<Card> LoadCards()
        {
            XDocument loadedData = XDocument.Load("CardList.xml");
            var personElements = from personElement in loadedData.Descendants("card") select personElement;

            List<Card> cardDeck = new List<Card>();
            foreach (XElement personElement in personElements)
            {
                int times = (int)personElement.Element("count");
                for (int i = 0; i < times; i++)
                {

                    int num = (int)personElement.Element("number");
                    Color color = (Color)Enum.Parse(typeof(Color), (string)personElement.Element("color"), true);
                    string image = "/Image/" + (string)personElement.Element("image");
                    Shape shape = (Shape)Enum.Parse(typeof(Shape), (string)personElement.Element("shape"), true);
                    Card a = new Card(shape, color, num, image);

                    Console.WriteLine(a);
                    System.Diagnostics.Debug.WriteLine(a);
                    cardDeck.Add(a);
                }

            }
            return cardDeck;
        }

        /// <summary>
        /// Load question from QuestionList.xml
        /// </summary>
        private void GetQuestions()
        {
            XDocument loadedData = XDocument.Load("QuestionList.xml");
            var questionElements = from question in loadedData.Descendants("Question") select question;

            _questions = new List<Question>();
            QuestionReference = new Dictionary<Question, Condition>();
            Condition c = null;
            //for each question
            foreach (XElement question in questionElements)
            {
                //question attributes
                int id = (int)question.Element("id");
                string text = (string)question.Element("text");
                string type = (string)question.Element("type");

                //condition that describes what the game is asking
                c = new Condition(QuestionType.RACK, null, null);
                if (type == "Compare")
                {

                    int number = int.MaxValue;
                    Color color = Color.WILDCARD;
                    Card leftCard = new Card();
                    XElement attributes = (XElement)question.Element("Criteria");
                    XElement cardDescription = (XElement)attributes.Element("Left");

                    var cardAttributes = cardDescription.Descendants();
                    foreach (XElement cardAttribute in cardAttributes)
                    {

                        if (cardAttribute.Name.LocalName == "Number")
                        {
                            number = int.Parse(cardAttribute.Value);
                        }
                        else if (cardAttribute.Name.LocalName == "Color")
                        {
                            color = (Color)Enum.Parse(typeof(Color), cardAttribute.Value, true);
                        }
                        Console.WriteLine();

                    }

                    leftCard = new Card(number, color);


                    number = int.MaxValue;
                    color = Color.WILDCARD;
                    cardDescription = (XElement)attributes.Element("Right");
                    cardAttributes = cardDescription.Descendants();
                    foreach (XElement cardAttribute in cardAttributes)
                    {

                        if (cardAttribute.Name.LocalName == "Number")
                        {
                            number = int.Parse(cardAttribute.Value);
                        }
                        else if (cardAttribute.Name.LocalName == "Color")
                        {
                            color = (Color)Enum.Parse(typeof(Color), cardAttribute.Value, true);
                        }
                        Console.WriteLine();

                    }

                    Card rightCard = new Card(number, color);

                    c = new Condition(QuestionType.COMPARE, leftCard, rightCard);
                }//if question sks about rack
                else if (type == "Rack")
                {
                    c = new Condition(QuestionType.RACK, null, null);

                }
                else if (type == "Other")
                {
                    c = new Condition(QuestionType.OTHER, null, null);
                }
                Question q = new Question(id, text);
                QuestionReference.Add(q, c);

                _questions.Add(q);
                System.Diagnostics.Debug.WriteLine(q);

            }
        }



        /// <summary>
        /// load observations from xml
        /// </summary>
        private void GetObservations()
        {
            XDocument loadedData = XDocument.Load("Observation.xml");
            var questionElements = from observation in loadedData.Descendants("Observation") select observation;
            _observations = new List<string>();

            foreach (XElement observation in questionElements)
            {

                Console.WriteLine(observation);
                string text = ((string)observation.Element("text")).Trim();
                _observations.Add(text);
            }
        }

        /// <summary>
        /// reassign cards to the player
        /// </summary>
        /// <param name="p"></param>
        public void ReassignCards(Player p)
        {
            _activeCards.AddRange(p.MyCards);

            ShuffleList(_activeCards);
            AssignCardsToPlayer(p);
        }

        /// <summary>
        /// Get the condtioin of the passed question
        /// </summary>
        /// <param name="q"></param>
        /// <returns></returns>
        public Condition GetCondition(Question q)
        {
            return QuestionReference[q];

        }

       

        /// <summary>
        /// get the passed players card
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public List<Card> GetPlayersCard(int i)
        {

            return _players[i].MyCards;

        }

        

        /// <summary>
        /// GEt cards visible to the passed player
        /// </summary>
        /// <param name="_playerid"></param>
        /// <returns></returns>
        public List<Card>[] GetVisibleCard(int _playerid)
        {
            List<Card>[] list = new List<Card>[_numPlayers - 1];

            int i = (_playerid + 1) % (_numPlayers);
            list[0] = GetPlayersCard(i);
            i = (i + 1) % (_numPlayers);
            list[1] = GetPlayersCard(i);

            i = (i + 1) % (_numPlayers);

            list[2] = GetPlayersCard(i);
            return list;
        }



        /// <summary>
        /// Method that shuffles a list
        /// </summary>
        /// <typeparam name="E"></typeparam>
        /// <param name="list"></param>
        public static void ShuffleList<E>(IList<E> list)
        {
            Random random = new Random();
            if (list.Count > 1)
            {
                for (int i = list.Count - 1; i >= 0; i--)
                {
                    E tmp = list[i];
                    int randomIndex = random.Next(i + 1);

                    //Swap elements
                    list[i] = list[randomIndex];
                    list[randomIndex] = tmp;
                }
            }
        }

        /// <summary>
        /// Assigns cards to all players
        /// </summary>
        private void AssignCards()
        {
            System.Diagnostics.Debug.WriteLine("In Assign Cards");
            foreach (Player p in _players)
            {
                AssignCardsToPlayer(p);
            }
            System.Diagnostics.Debug.WriteLine("Finished Assigning Cards");

        }
        /// <summary>
        /// Assign cards to the passed player
        /// </summary>
        /// <param name="p"></param>
        private void AssignCardsToPlayer(Player p)
        {
            List<Card> playerTiles = new List<Card>();
            //get the last thrre cards
            playerTiles = _activeCards.GetRange(_activeCards.Count - 4, 3);
            _activeCards.RemoveRange(_activeCards.Count - 4, 3);

            p.MyCards = playerTiles;
            System.Diagnostics.Debug.WriteLine(p);
        }

        /// <summary>
        /// create four players
        /// </summary>
        private void CreatePlayers()
        {
            _numPlayers = 4;
            _players = new List<Player>(4);
            _players.Add(new Player("Hero", String.Format("/Image/{0}", "player1.jpg")));
            _players.Add(new Player("Zeus", String.Format("/Image/{0}", "player2.jpg")));
            _players.Add(new Player("Loki", String.Format("/Image/{0}", "player3.jpg")));
            _players.Add(new Player("Thor", String.Format("/Image/{0}", "thor.jpg")));
        }

        /// <summary>
        /// Create the chocies for the question
        /// </summary>
        private void CreateQuestionChoices()
        {
            _numberAnswersChoices = new List<string>();
            _compareAnswersChoices = new List<string>();
            _numberAnswersChoices.Add("Zero");
            _numberAnswersChoices.Add("One");
            _numberAnswersChoices.Add("Two");
            _numberAnswersChoices.Add("Three");
            _numberAnswersChoices.Add("Four");
            _numberAnswersChoices.Add("Five");
            _numberAnswersChoices.Add("Six");
            _numberAnswersChoices.Add("Seven");
        }

        #endregion
    }
}
