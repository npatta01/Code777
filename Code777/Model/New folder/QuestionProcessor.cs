
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System;

namespace Code777.Model
{
    /// <summary>
    /// The class tat process the question asked in the gmae
    /// </summary>
    public class QuestionProcessor
    {
        /// <summary>
        /// for the given question and state come up with an answer
        /// </summary>
        /// <param name="q"></param>
        /// <param name="combinedList"></param>
        /// <param name="combined"></param>
        /// <param name="c1"></param>
        /// <returns></returns>
        public static int processQuestion(Question q, List<Card>[] combinedList, List<Card> combined, Condition c1)
        {
            Condition c = c1;
            int result;

            if (c.QuestionType == QuestionType.RACK || c.QuestionType == QuestionType.OTHER)
            {
                result = RackQuestion(q, combinedList);

            }
            else
            {

                result = EvaluateExpression(combined, c1);
            }
            return result;


        }
        /// <summary>
        /// Evaluate the comparision question
        /// </summary>
        /// <param name="combined"></param>
        /// <param name="c1"></param>
        /// <returns></returns>
        private static int EvaluateExpression(List<Card> combined, Condition c1)
        {

            int count1;
            int count2;
            // Compose the expression tree that represents the parameter card  to the predicate.
            ParameterExpression pe = Expression.Parameter(typeof(Card), "card");


            // Create an expression tree that represents the expression 'card == LeftCard'.
            Expression constantCardtoEqual = Expression.Constant(c1.LeftCard, typeof(Card));
            Expression cardEqualexpr = Expression.Equal(pe, constantCardtoEqual);

            var exp = Expression.Lambda<Func<Card, bool>>(cardEqualexpr, new ParameterExpression[] { pe });
            count1 = combined.Count(exp.Compile());


            // Create an expression tree that represents the expression 'card == LeftCard'.
            constantCardtoEqual = Expression.Constant(c1.RightCard, typeof(Card));
            cardEqualexpr = Expression.Equal(pe, constantCardtoEqual);

            exp = Expression.Lambda<Func<Card, bool>>(cardEqualexpr, new ParameterExpression[] { pe });

            count2 = combined.Count(exp.Compile());

            if (count1 == count2)
            {
                return 2;
            }
            else if (count1 > count2) return 0;
            else return 1;

        }


        /// <summary>
        /// Sqitch question for each question
        /// </summary>
        /// <param name="q"></param>
        /// <param name="combinedList"></param>
        /// <returns></returns>
        private static int RackQuestion(Question q, List<Card>[] combinedList)
        {
            int result = 0;
            switch (q.QuestionId)
            {
                case 1:
                    result = question1(combinedList);
                    break;
                case 2:
                    result = question2(combinedList);
                    break;
                case 3:
                    result = question3(combinedList);
                    break;
                case 4:
                    result = question4(combinedList);
                    break;
                case 5:
                    result = question5(combinedList);
                    break;

                case 6:
                    result = question6(combinedList);
                    break;

                case 7:
                    result = question7(combinedList);
                    break;


                case 8:
                    result = question8(combinedList);
                    break;

                case 9:
                    result = question9(combinedList);
                    break;

                case 10:
                    result = question10(combinedList);
                    break;

                case 11:
                    result = question11(combinedList);
                    break;
            }
            return result;

        }

        /// <summary>
        /// solving question 11
        /// </summary>
        /// <param name="combinedList"></param>
        /// <returns></returns>
        private static int question11(List<Card>[] combinedList)
        {
            Card a = new Card(1, Color.GREEN);
            Card b = new Card(5, Color.BLACK);
            Card d = new Card(7, Color.PINK);

            int count = (from l in combinedList
                         from Card c in l
                         select c).Where((c) => (c == a || c == b || c == d)).Distinct().Count();
            return count;
        }

        /// <summary>
        /// solving question 10
        /// </summary>
        /// <param name="combinedList"></param>
        /// <returns></returns>
        private static int question10(List<Card>[] combinedList)
        {
            int existingnumbers = (from l in combinedList
                                   from Card c in l
                                   select c.Number).Distinct().Count();
            return 9 - existingnumbers;
        }
        // <summary>
        /// solving question 9
        /// </summary>
        /// <param name="combinedList"></param>
        /// <returns></returns>
        private static int question9(List<Card>[] combinedList)
        {
            int count = (from l in combinedList
                         from Card c in l
                         group c by c.Color into k
                         select new { num = k.Key, count = k.Count() })
                       .Count((l) => l.count > 2);

            return count;
        }
        // <summary>
        /// solving question 8
        /// </summary>
        /// <param name="combinedList"></param>
        /// <returns></returns>
        private static int question8(List<Card>[] combinedList)
        {
            int count = (
                from l in combinedList
                from Card c in l
                group c by c.Color into k
                select new { num = k.Key }).Count();

            return count;
        }
        // <summary>
        /// solving question 7
        /// </summary>
        /// <param name="combinedList"></param>
        /// <returns></returns>
        private static int question7(List<Card>[] combinedList)
        {
            int rack = 0;

            foreach (List<Card> list in combinedList)
            {
                var t = list.OrderBy((c) => c.Number).Select((c) => c.Number).ToArray();
                if ((t[1] == t[0] + 1) && (t[2] == t[1] + 1))
                {
                    rack++;
                }

            }
            return rack;
        }
        // <summary>
        /// solving question 6
        /// </summary>
        /// <param name="combinedList"></param>
        /// <returns></returns>
        private static int question6(List<Card>[] combinedList)
        {
            int matchingracks = 0;

            foreach (List<Card> list in combinedList)
            {
                int result = (

                from c in list
                group c by c into k1
                where k1.Count() > 1
                select new { Number = k1.Key, Count = k1.Count() }).Count();

                if (result > 0)
                {
                    matchingracks++;

                }

            }

            return matchingracks;
        }
        // <summary>
        /// solving question 5
        /// </summary>
        /// <param name="combinedList"></param>
        /// <returns></returns>
        private static int question5(List<Card>[] combinedList)
        {
            int count = combinedList.
               Select(c => c.Count((card) => card.Number % 2 == 0))

               .Count(l => l == 3 || l == 0);

            return count;
        }
        // <summary>
        /// solving question 4
        /// </summary>
        /// <param name="combinedList"></param>
        /// <returns></returns>
        private static int question4(List<Card>[] combinedList)
        {
            int matchingracks = 0;

            foreach (List<Card> list in combinedList)
            {
                int result = (from ff in list
                              group ff by ff.Color into l
                              select new { Color = l.Key, Count = l.Count() }).Count();

                if (result > 0)
                {
                    matchingracks++;

                }

            }

            return matchingracks;




        }
        // <summary>
        /// solving question 3
        /// </summary>
        /// <param name="combinedList"></param>
        /// <returns></returns>
        private static int question3(List<Card>[] combinedList)
        {
            int matchingracks = 0;

            foreach (List<Card> list in combinedList)
            {
                int result = (

                from c in list.Distinct()
                group c by c.Number into k1
                where k1.Count() > 1
                select new { Number = k1.Key, Count = k1.Count() }).Count();

                if (result > 0)
                {
                    matchingracks++;

                }

            }

            return matchingracks;
}

        // <summary>
        /// solving question 2
        /// </summary>
        /// <param name="combinedList"></param>
        /// <returns></returns>
        private static int question2(List<Card>[] combinedList)
        {
            int count = combinedList.Select(c => c.Sum((Card d) => d.Number)).Count(l => l < 12);

            return count;
        }

        // <summary>
        /// solving question 1
        /// </summary>
        /// <param name="combinedList"></param>
        /// <returns></returns>
        private static int question1(List<Card>[] combinedList)
        {
                       int count = combinedList.
                Select(c =>
                    c.Sum((Card d) => d.Number))
                .Count(l => l >= 18);

            return count;
        }
    }
}
