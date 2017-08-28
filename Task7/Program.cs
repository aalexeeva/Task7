using System;
using static System.Console;

namespace Task7
{
    class Program
    {
        public static bool Degree(int a) // проверка, является ли длина введенной функции степенью двойки
        {
            if (a == 2) return true;
            return a % 2 == 0 && Degree(a / 2);
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
                for (var i = 0; i < func.Length; i++) // разделение функции на две половины
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
            for (var i = 0; i < func.Length / 2; i++)
                p[i] = func[2 * i];
            return p;
        }

        public static bool Exit() // выход из программы
        {
            WriteLine("Желаете начать сначала или нет? \nВведите да или нет");
            var word = Convert.ToString(ReadLine()); // ответ пользователя
            Clear();
            if (word == "да" || word == "Да" || word == "ДА")
            {
                Clear();
                return false;
            }
            Clear();
            WriteLine("Вы ввели 'нет' или что-то непонятное. Нажмите любую клавишу, чтобы выйти из программы.");
            ReadKey();
            return true;
        }

        static void Main(string[] args)
        {
            bool okay;
            do
            {
                WriteLine("Введите булеву функцию: ");
                var function = Input();
                _useless = new bool[(int) Math.Log(function.Length, 2)];
                var answer = Action(new[] {function}, out bool c);
                for (var i = 0; i < _useless.Length; i++)
                    WriteLine(_useless[i] ? "{0} переменная фиктивна" : "{0} переменная существенна", i + 1);
                WriteLine("Вектор функции после удаления фиктивных переменных: " + answer[0]);
                okay = Exit();
            } while (!okay);
        }
    }
}
