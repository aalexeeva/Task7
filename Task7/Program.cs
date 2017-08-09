using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

namespace Task7
{
    class Program
    {
        public static bool Degree(int a) // проверка, является ли длина введенной функции степенью двойки
        {
            if (a == 2) return true;
            else if (a % 2 == 0) return Degree(a / 2);
            else return false;
        }

        public static string Input() // функция проверки ввода, разрешающая вводить только 0 и 1
        {
            string currentSymbol = string.Empty; // переменная для введенного символа
            bool convertResult = false; // переменная, определяющая верно ли введен символ
            bool ok = false;
            do
            {
                try
                {
                    while (!convertResult)
                    {
                        ConsoleKeyInfo keyPress = ReadKey(true); // ввод символа
                        int input = keyPress.KeyChar; // код введенного символа
                        // символ введен верно, если его код совпадает с кодом нуля или единицы
                        convertResult = Convert.ToInt32(input) == 48 || Convert.ToInt32(input) == 49;
                        if (convertResult) // если символ введен верно, вывод его в консоль
                        {
                            if (input == 48)
                            {
                                Write(0);
                                currentSymbol += "0";
                            }
                            else
                            {
                                Write(1);
                                currentSymbol += "1";
                            }
                        }
                        convertResult = false;
                        if (input == 13) // если нажат интер
                        {
                            ok = Degree(currentSymbol.Length); // проверка длины ввода
                            if (!ok) // если ввод некорректен
                            {
                                WriteLine("\nДлина функции не может быть нечетным числом, необходимо продолжить ввод");
                                Write(currentSymbol);
                            }
                            else // если ввод корректен
                            {
                                WriteLine("\nФункция введена корректно");
                                break;
                            }
                        }
                    }
                }
                catch (FormatException)
                {
                    WriteLine("Ошибка при вводе числа");
                    ok = false;
                }
                catch (OverflowException)
                {
                    WriteLine("Ошибка при вводе числа");
                    ok = false;
                }
            } while (!ok);
            return currentSymbol;
        }

        public static string Action(string first, bool ok)
        {
            string second, third, lol = string.Empty;
            int current = 1, i = 0;
            int[] array = new int[first.Length];
            for (int step = first.Length / 2; step > 0; step /= 2)
            {
                second = first.Substring(0, first.Length/2);
                third = first.Substring(first.Length / 2, first.Length - first.Length / 2);
                if (second == third)
                {
                    array[i] = current;
                    i++;
                }
                else
                {
                    lol += Action(third, false);
                    ok = true;
                }
                current++;
                first = second;
            }
            if (ok)
            {
                int[] result = array.Distinct().ToArray();
                foreach (var t in result)
                {
                    if (t != 0) WriteLine("Фиктивной переменной является переменная " + t);
                }
            }
            return first + lol;
        }

        static void Main(string[] args)
        {
            WriteLine("Введите булеву функцию: ");
            string function = Input();
            string answer = Action(function, true);
            WriteLine("Вектор функции после удаления фиктивных переменных: " + answer);
            Read();
        }
    }
}
