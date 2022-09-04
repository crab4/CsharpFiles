using System;
using System.Collections.Generic;

namespace base32 {
    class Program {

        static void Main(string[] args) {
            string answer = string.Empty;
            int count = Int32.Parse(Console.ReadLine());
            while(count-- > 0) {
                BaseBin takken = new BaseBin(Console.ReadLine());
                Base32 enCode = new Base32(takken);
                answer += $"{enCode} ";
                count--;
                if (count == -1)
                    break;
                enCode = new Base32(Console.ReadLine());
                takken = new BaseBin(enCode);
                answer += $"{takken} ";
            }
            Console.WriteLine(answer);
        }

        
        class Base32 {
            public string m_mail;
            //ABCDEFGHIJKLMNOPQRSTUVWXYZ234567
            public static Dictionary<int, string> enCode = new Dictionary<int, string>{
                    { 0, "A" }, { 1, "B" }, { 2, "C" }, { 3, "D" }, { 4, "E" }, { 5, "F" }, { 6, "G" }, { 7, "H" },
                    { 8, "I" }, { 9, "J" }, { 10, "K" }, { 11, "L" }, { 12, "M" }, { 13, "N" }, { 14, "O" }, { 15, "P" },
                    { 16, "Q" }, { 17, "R" }, { 18, "S" }, { 19, "T" }, { 20, "U" }, { 21, "V" }, { 22, "W" }, { 23, "X" },
                    { 24, "Y" }, { 25, "Z" }, { 26, "2" }, { 27, "3" }, { 28, "4" }, { 29, "5" }, { 30, "6" }, { 31, "7" },
                };

            public Base32(string mail) { m_mail = mail; }
            public Base32(BaseBin letter) {
                
                
                m_mail = string.Empty;
                int i = 0;
                for (; i < letter.m_number.Length; i += 5) {
                    m_mail += enCode[ConvertTo32(letter.m_number[i], letter.m_number[i + 1], letter.m_number[i + 2], letter.m_number[i + 3], letter.m_number[i + 4])];
                }

                static byte ConvertTo32(byte one, byte second, byte third, byte fourth, byte fiveth) {
                    byte result = (byte)(one * 16 + second * 8 + third * 4 + fourth * 2 + fiveth * 1);
                    return result;
                }

            }
            public override string ToString() {
                return m_mail;
            }
        }




        //Кста, можно было побаловать себя битовыми операциями, аля записывать не в 8 разных байтов, а хранить всё в одном. Но, если честно, мне лень, помню как делать ещё со школы, так к чему лишние хлопоты(=
        class BaseBin {
            public byte[] m_number;


            //Думал как обычно делением перевести, но столько лишних телодвижений от постоянных сдвигов и передвижений. Что имхо так элегантнее перевести в двоичную систему
            public  BaseBin(string letter) {
                int extension = 5 - letter.Length % 5;
                int count = extension;
                while (count-- > 0)
                    letter += $"{extension}";
                m_number = new byte[letter.Length * 8];
                for(var i = 0; i < letter.Length; i++) {
                    byte temp = (byte)letter[i];
                    byte modul = 128;
                    int position = 0;
                    while (temp != 0) {
                        if (temp >= modul) {
                            m_number[i * 8 + position] = 1;
                            temp -= modul;
                        }
                        modul /= 2;
                        position++;
                    }
                }
            }
            public BaseBin(Base32 input) {
                //ABCDEFGHIJKLMNOPQRSTUVWXYZ234567  Может стоило бахнуть сразу байтовый словарь? Не херня какая-то(= Начал заполнять и стало ужасно скучно, оставлю интами
                //А вот тут я, конечно, обгадился, что не написал функцию для перевода десятичного числа в 8 битное
                Dictionary<char, byte> deCode = new Dictionary<char, byte> {
                    {'A',0 },{'B',1 },{'C',2 },{'D',3 },{'E',4 },{'F',5 },{'G',6 },{'H',7 },
                    {'I',8 },{'J',9 },{'K',10 },{'L',11 },{'M',12 },{'N',13 },{'O',14 },{'P',15 },
                    {'Q',16 },{'R',17 },{'S',18 },{'T',19 },{'U',20 },{'V',21 },{'W',22 },{'X',23 },
                    {'Y',24 },{'Z',25 },{'2',26 },{'3',27 },{'4',28 },{'5',29 },{'6',30 },{'7',31 },
                };
                m_number = new byte[input.m_mail.Length * 5];
                for(var i =0; i < input.m_mail.Length; i++) {
                    byte temp = deCode[input.m_mail[i]];
                    byte modul = 16;
                    int position = 0;
                    while(temp != 0) {
                        if(temp>= modul) {
                            m_number[i*5+position]= 1;
                            temp -= modul;
                        }
                        modul /= 2;
                        position++;
                    }
                }
            }
            public int ConvertToDec(int[] array) {
               int result=0;
               int power = 7;
               for(var i =0; i < 8; i++) {
                    result +=array[i] * (int)Math.Pow(2,power--);
               }
                return result;
            }
            public override string ToString() {
                string answer = string.Empty;
                for(var i =0; i < m_number.Length; i+=8) {
                    int[] tmp = new int[8];
                    for (var j = 0; j < 8; j++)
                        tmp[j] = m_number[i + j];
                    char wonderfullLetter = (char)ConvertToDec(tmp);
                    answer += wonderfullLetter;
                }
                int last = 0;
                if (Int32.TryParse(answer[answer.Length - 1].ToString(), out last))
                    answer = answer.Remove(answer.Length - last);
                return answer;
            }

        }
    }
}
