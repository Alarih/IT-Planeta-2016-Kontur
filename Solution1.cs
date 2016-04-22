using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{

    /// <summary>
    /// Рациональное число
    /// </summary>
    public struct Rational : IComparable<Rational>
    {
        /// <summary>
        /// Числитель
        /// </summary>
        public readonly int Numerator;

        /// <summary>
        /// Знаменатель
        /// </summary>
        public readonly int Denominator;

        /// <summary>
        /// Значение с плавающей запятой
        /// </summary>
        public readonly double Value;

        /// <summary>
        /// Создание рационального числа
        /// </summary>
        /// <param name="numerator">Числитель</param>
        /// <param name="denominator">Знаменатель</param>
        public Rational(int numerator, int denominator)
        {
            Value = 0;

            //проверка знаменателя
            if (denominator == 0)
            {
                throw new ArgumentException();
            }

            //инициализируем числитель и знаменатель
            Numerator = numerator;
            Denominator = denominator;

            Value = (float)Numerator / (float)Denominator;

        }

        #region сокращение дроби
        /// <summary>
        /// Сокращение дроби
        /// </summary>
        /// <returns></returns>
        public Rational Simplify()
        {
            int nod = NOD(Numerator, Denominator);

            if (nod != 0)
            {
                return new Rational(Numerator / nod, Math.Abs(Denominator) / nod);
            }
            else
            {
                return this;
            }
            
        }

        /// <summary>
        /// Нахождение наибольшего общего делителя для сокращения дроби
        /// </summary>
        /// <returns></returns>
        private int NOD(int numerator, int denominator)
        {
            int temp;
            numerator = Math.Abs(numerator);
            denominator = Math.Abs(denominator);

            while (denominator != 0 && numerator != 0)
            {
                if (numerator % denominator > 0)
                {
                    temp = numerator;
                    numerator = denominator;
                    denominator = temp % denominator;
                }
                else
                {
                    break;
                }
            }

            if (denominator != 0 && numerator != 0)
            {
                return denominator;
            }
            else
            {
                return 0;
            }
                
        }
        #endregion

        #region перегрузка операторов 

        //Сокращенные сумма, произведение, разность, частное и обратное число.
        //В случае, если числитель или знаменатель результирующей дроби не лежит в диапазоне int,
        //выбрасывается OverflowException

        public static Rational operator +(Rational r1, Rational r2)
        {
            return new Rational(checked(r1.Numerator * r2.Denominator + r2.Numerator * r1.Denominator), checked(r1.Denominator * r2.Denominator)).Simplify();

        }

        public static Rational operator -(Rational r1, Rational r2)
        {
            return new Rational(checked(r1.Numerator * r2.Denominator - r2.Numerator * r1.Denominator), checked(r1.Denominator * r2.Denominator)).Simplify();

        }

        public static Rational operator -(Rational r1)
        {
            return new Rational(-r1.Numerator, -r1.Denominator).Simplify();

        }

        public static Rational operator *(Rational r1, Rational r2)
        {
            return new Rational(checked(r1.Numerator * r2.Numerator), checked(r1.Denominator * r2.Denominator)).Simplify();

        }

        public static Rational operator /(Rational r1, Rational r2)
        {
            return new Rational(checked(r1.Numerator * r2.Denominator), checked(r1.Denominator * r2.Numerator)).Simplify();

        }
        #endregion

        /// <summary>
        /// Преобразование рационального числа в строку
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (Numerator == 0)
            {
                return "0";
            }

            if (Denominator == 1)
            {
                return Numerator.ToString();
            }

            return Numerator.ToString() + "/" + Denominator.ToString();

        }

        public int CompareTo(Rational other)
        {
            return Value.CompareTo(other.Value);
        }

        /// <summary>
        /// Неявное преобразование в тип double
        /// </summary>
        public static implicit operator double(Rational number)
        {
            return number.Value;

        }

        /// <summary>
        /// Явное преобразование из типа int
        /// </summary>
        public static explicit operator Rational(int number)
        {
            return new Rational(number, 1);

        }
    }
    
}
