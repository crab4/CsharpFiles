using System;
using System.IO;
using System.Collections.Generic;
using System.Reflection;

namespace BrainFuckInterprer {
    class Program {
        static void Main(string[] args) {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            string _code = Console.ReadLine();
            FuckInterpreter(_code);
        }

        static void FuckInterpreter(string code) {/*интерпретатор БрейнФака. Мы тут пораскинули и решили, что лучше делать через файл. Записали всё дело в файл, потом начали по нему бегать.
                                                   * такие дела. Предпологается, что код уже отформатирован, удалены пробелы и \n */// Мы - это Краб четыре)
                                                    //Опять же. ПРЕДПОЛОГАЕТСЯ, что введён валидный адекватный код!!! Это ж интерпретатор!
            string wayFile = "code.txt";
            using (StreamWriter codeFile = new StreamWriter(wayFile, false, System.Text.Encoding.Default)) {
                codeFile.WriteLine(code);
            }

            string _codeLine;// Такую форму записи переменных подсмотрел у одного дядьки на Хабре, показалось удобным, позаимствую, как когда то фигурные скобки подвинулись вверх на одну строку(=
            int[] _memory = new int[1024];//допустим у нас 1024 ячейки. Можно реализовать и с байтами. Но я подумал так красИвее;
            int _pointer = 0; //Указатель на ячейки памяти
            int _codePointer = 0;
            Stack<int> _brackets = new Stack<int>();//здесь будем хранить адрес ячеек, в которых были обнаружены циклы(
            Dictionary<int, int> _loops = new Dictionary<int, int>(); // Здесь будем хранить адреса совпадающих циклов. Чтобы в конце цикла проще было перепрыгнуть на нужное место
            string[] inputs = Console.ReadLine().Split(' ', StringSplitOptions.RemoveEmptyEntries);
            int _inputIndex = 0;


            using(StreamReader codeFile = new StreamReader(wayFile)) {
                _codeLine = codeFile.ReadToEnd();
            }
            while (_codePointer < _codeLine.Length) {
                switch (_codeLine[_codePointer]) {
                    case ':':
                        Console.Write($"{_memory[_pointer]} ") ;
                        break;
                    case ';':
                        _memory[_pointer] = Int32.Parse(inputs[_inputIndex++]);
                        break;
                    case '>':
                        _pointer++;
                        break;
                    case '<':
                        _pointer--;
                        break;
                    case '+':
                        _memory[_pointer]++;
                        break;
                    case '-':
                        _memory[_pointer]--;
                        break;
                    case '[':
                        if (_memory[_pointer] == 0) { // Если цикл подошёл к концу или не прошёл условия
                            if (_loops.ContainsKey(_codePointer))// проверяем, есть ли адрес в словаре для этой скобки
                                _codePointer = _loops[_codePointer];
                            else {
                                int _pointOnBracket = _codePointer;
                                int _depth = 1;
                                while (_depth != 0 && _codePointer < _codeLine.Length) {
                                    _codePointer++;
                                    if (_codeLine[_codePointer] == ']')
                                        _depth--;
                                    if (_codeLine[_codePointer] == '[')
                                        _depth++;
                                }
                                if (_depth == 0) // нужно будет вернуться к [, чтобы успешно вытащить её из стека
                                    _loops.Add(_pointOnBracket, _codePointer);
                            }
                        } else
                            _brackets.Push(_codePointer);
                            break;
                    case ']':
                        int _tryingStack;
                        if (_brackets.TryPop(out _tryingStack)) {
                            _codePointer = _tryingStack;
                            _codePointer--;
                     
                        }
                        break;
                    default: _codePointer++;
                        break;
                }
                _codePointer++;
            }
        }
    }
}
