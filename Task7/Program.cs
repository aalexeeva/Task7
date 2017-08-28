using System;
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
            bool ok;
            do
            {
                while (true)
                {
                    ConsoleKeyInfo keyPress = ReadKey(true); // ввод символа
                    // символ введен верно, если его код совпадает с кодом нуля или единицы
                    switch (keyPress.Key)
                    {
                        case ConsoleKey.D0:
                            CursorLeft = 0;
                            currentSymbol += "0";
                            Write(currentSymbol);
                            break;
                        case ConsoleKey.D1:
                            CursorLeft = 0;
                            currentSymbol += "1";
                            Write(currentSymbol);
                            break;
                        case ConsoleKey.Backspace:
                            if (currentSymbol.Length > 0)
                            {
                                CursorLeft = 0;
                                currentSymbol = currentSymbol.Substring(0, currentSymbol.Length - 1);
                                Write("{0} ", currentSymbol);
                                CursorLeft = currentSymbol.Length;
                            }
                            break;
                    }

                    if (keyPress.Key != ConsoleKey.Enter) continue;
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
            while (!ok);

            return currentSymbol;
        }

        public static int VariableCount = -1; // счетчик действий
        private static bool[] _useless; // фиктивные переменные

        public static string[] Action(string[] func, out bool changed)
        {
            if (func[0].Length != 1) // проверка длины исходной функции
            {
                VariableCount++;

                var arr = new string[func.Length * 2]; // массив с разделенной функцией
                for (int i = 0; i < func.Length; i++) // разделение функции на две половины
                {
                    arr[2 * i] = func[i].Substring(0, func[i].Length / 2); 
                    arr[2 * i + 1] = func[i].Substring(func[i].Length / 2);
                }

                arr = Action(arr, out bool c); // проверка половин функции
                if (c) // проверка на удаленные переменные
                    func = arr;
                else // создание вектора функции без фиктивных переменных
                {
                    for (int i = 0; i < func.Length; i++)
                        func[i] = arr[2 * i] + arr[2 * i + 1];
                }

                _useless[VariableCount] = c;
            }

            if (func.Length == 1) // если длина функции равна единице
            {
                changed = false;
                return func;
            }

            // проверка наличия фиктивной переменной
            bool useless = true;
            for (int i = 0; i < func.Length / 2 && useless; i++)
                if (func[2 * i] != func[2 * i + 1])
                    useless = false;

            if (!useless) // если фиктивных переменных не найдено
            {
                changed = false;
                return func;
            }

            changed = true;
            var p = new string[func.Length / 2];
            for (int i = 0; i < func.Length / 2; i++)
                p[i] = func[2 * i];
            return p;
        }

        static void Main(string[] args)
        {
            WriteLine("Введите булеву функцию: ");
            string function = Input();
            _useless = new bool[(int)Math.Log(function.Length, 2)];
            var answer = Action(new[] { function }, out bool c);
            for (int i = 0; i < _useless.Length; i++)
                if (_useless[i])
                    WriteLine("{0} переменная фиктивна", i + 1);
            WriteLine("Вектор функции после удаления фиктивных переменных: " + answer[0]);
            ReadKey(true);
        }
    }
}
