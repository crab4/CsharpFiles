using System;
using System.Runtime.InteropServices.ComTypes;
/* Бинарная куча, где корень - минимальное число
Далеко не самая лучшая реализация, т.к. не стал использовать коллекции с динамическим размером массива. Хотелось побаловаться.
Если понадобится в будущем, не забудь, пожалуйста int[] заменить на что-то более адекватное*/
namespace BinaryHeap {
    class Program {
        static void Main(string[] args) {
            Console.ReadLine();
            string[] temp = Console.ReadLine().Split(' ', StringSplitOptions.RemoveEmptyEntries);
            int[] binaryHeap = new int[0];
            foreach(var member in temp) {
                if (Int32.Parse(member) == 0)
                    binaryHeap = GiveMinimal(binaryHeap, out int hello);
                else
                    binaryHeap = InsertNumber(binaryHeap,Int32.Parse(member));
            }
            foreach (var binMember in binaryHeap)
                Console.Write($"{binMember} ");
        }


        // Нашёл на вики плюс-минус алгоритм. Функция возвращает в нормальное состояние поддерево с корнем в index
        // При условии, что правое и левое поддерево уже являются бинарными кучками
        static void Heapify(int index, ref int[] array) {
            int indexSmaller = index;
            if (index * 2 + 1 < array.Length)
                if (array[indexSmaller] > array[index * 2 + 1])
                    indexSmaller = index * 2 + 1;
            if (index * 2 + 2 < array.Length)
                if (array[indexSmaller] > array[index * 2 + 2])
                    indexSmaller = index * 2 + 2;
            if (indexSmaller != index) {
                int temp = array[index];
                array[index] = array[indexSmaller];
                array[indexSmaller] = temp;
                Heapify(indexSmaller, ref array);
            }
        }
        //Метод для повышения, новйы элемент обязан быть меньше текущего, но должен быть положительным. Для спуска прекрасно подойдёт и Heapify
        static void GoToUp(ref int[] array, int index, int number) {
            if (number < 0 || array[index] > number)
                throw new Exception("Крестражи? Я никогда не слышал ничего о крестражаx");
            array[index] = number;
            while(index > 0 && array[index] < array[(index - 1) /2]) {
                int temp = array[index];
                array[index] = array[(index - 1) / 2];
                array[(index - 1) / 2] = temp;
                index = (index - 1) / 2;
            }

        }
        static int[] InsertNumber(int[] array, int number) {
            int[] arrayNew = new int[array.Length + 1];
            for (var i = 0; i < array.Length; i++)
                arrayNew[i] = array[i];
            GoToUp(ref arrayNew, arrayNew.Length-1, number);
            return arrayNew;
        }
        //Построение из Массива кучи. В текущем здании не пригодится, но мало ли, где всплывёт, удобно будет ничего не дописывать, а просто скопировать.
        static int[] BuildHeap(int[] array) {
            for (var i = (array.Length  - 1)/2; i >= 0; i--)
                Heapify(i, ref array);
            return array;
        }
        //пирамидальная сортировка. Ксати прикольная штука(=
        static void PyramidalSort(ref int[] array) {
            array = BuildHeap(array);
            int size = array.Length;
            for(int i = size-1; i>=0; i--) {
                int tmp = array[0];
                array[0] = array[i];
                array[i] = array[0];
                int[] newarray = new int[i];
                if (i != 0) {
                    for (var j = 0; j < i; j++)
                        newarray[j] = array[j];
                }
                Heapify(0, ref newarray);
                for(var j =0; j< i; j++) {
                    array[j] = newarray[j];
                }

            }
        }



        //Извлечение минимального элемента.
        static int[] GiveMinimal(int[] array, out int min) {
            if (array.Length < 1)
                throw new Exception("Министерство пало");
            min = array[0];
            array[0] = array[array.Length - 1];
            int[] newArray = new int[array.Length - 1];
            for (var i = 0; i < array.Length - 1; i++)
                newArray[i] = array[i];
            Heapify(0, ref newArray);
            return newArray;
        }

    }



    
}
