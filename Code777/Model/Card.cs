using System;

namespace Code777.Model
{
		/// <summary>
		/// A class that encapsulates the idea of a Card in the Code777
		/// game. A card has a color, shape and number
		/// </summary>
		/// <author> Nidhin Pattaniyil <ntp5633@cs.rit.edu</author>
		public class Card
		{
				#region Properties
				/// <summary>
				/// Reference to shape of the card
				/// </summary>
				private Shape _shape;
				public Shape Shape
				{
						get { return _shape; }

				}

				/// <summary>
				/// Refernce to color of card
				/// </summary>
				private Color _color;
				public Color Color
				{
						get { return _color; }

				}

				/// <summary>
				/// Number of Card
				/// </summary>
				private int _number;
				public int Number
				{
						get { return _number; }


				}

				/// <summary>
				/// Path of image of the card
				/// </summary>
				private string _image;
				public string ImagePath
				{
						get { return _image; }


				}
				#endregion

				/// <summary>
				/// Constructor of card
				/// </summary>
				/// <param name="shape"></param>
				/// <param name="color"></param>
				/// <param name="number"></param>
				/// <param name="image"></param>
				public Card(Shape shape, Color color, int number, string image):this(number,color)
				{
						_shape = shape;
						_image = image;
				}

				/// <summary>
				/// Construcotr for a wildcard card
				/// </summary>
				public Card()
				{
						_color = Color.WILDCARD;
						_number = int.MaxValue;
				}


				/// <summary>
				/// Construcotr of Card
				/// </summary>
				/// <param name="number"></param>
				/// <param name="color"></param>
				public Card(int number, Color color)
				{
						_number = number;
						_color = color;

				}

				#region methods

				/// <summary>
				/// To String Method
				/// </summary>
				/// <returns></returns>
				public override string ToString()
				{
						string c = (Color == Color.WILDCARD) ? "" : Color.ToString();
						string n = (Number == int.MaxValue) ? " " : Number.ToString();
						return String.Format("{0} {1}", c, n);
				}

				/// <summary>
				/// Equals Method of card. A card is equal to a wildcard card
				/// Card matches other card's number and color
				/// </summary>
				/// <param name="obj"></param>
				/// <returns></returns>
				public override bool Equals(object obj)
				{
						// Check for null values and compare run-time types.
						if (obj == null || GetType() != obj.GetType())
								return false;

						Card tmp = (Card)obj;
						if (_color == tmp.Color || _color == Color.WILDCARD || tmp._color == Color.WILDCARD)
						{
								if (_number == tmp.Number || tmp.Number == int.MaxValue || Number == int.MaxValue)
								{

										return true;
								}

						}
						return false;

				}

				/// <summary>
				/// Hashcode:
				/// Value is just the color and number
				/// </summary>
				/// <returns></returns>
				public override int GetHashCode()
				{
						return (int)_color ^ _number;

				}




				/// <summary>
				/// Operator equals override for two cards
				/// </summary>
				/// <param name="a"></param>
				/// <param name="b"></param>
				/// <returns></returns>
				public static bool operator ==(Card a, Card b)
				{
						// If both are null, or both are same instance, return true.
						if (System.Object.ReferenceEquals(a, b))
						{
								return true;
						}

						// If one is null, but not both, return false.
						if (((object)a == null) || ((object)b == null))
						{
								return false;
						}

						// Return true if the fields match:
						return a.Equals(b);
				}

				/// <summary>
				/// Operator ovveride for not equals
				/// </summary>
				/// <param name="a"></param>
				/// <param name="b"></param>
				/// <returns></returns>
				public static bool operator !=(Card a, Card b)
				{
						return !(a == b);
				}
				#endregion
		}
}
