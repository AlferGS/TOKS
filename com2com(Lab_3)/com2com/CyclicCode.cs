using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace com2com
{
    public partial class CyclicCode: Form
    {
        private string message { get; set; }
        public string part;
        public string partResult = "";
        public string returnString = "";
        public string debugString = "";
        public string substring;
        public string debugSubstring;
        int leftShiftCount = 0;
        // перевод из string в масив char // из char перевод в dec // из dec перевод в binary
        public string StringToBiteString(string _message)
        {
            message = _message;
            string newMessage = "";
            for (int i = 0; i < message.Length; i++)
            {
                if(message[i] == ' ' || '0' <= message[i]) 
                {
                    if (message[i] <= '9')
                    {
                        newMessage += "0" + Convert.ToString(message[i], 2);
                        continue;
                    }
                }
                if(message[i] == '\r' || message[i] == '\n') { newMessage += "000" + Convert.ToString(message[i], 2); continue; }
                newMessage += Convert.ToString(message[i], 2);
            }
            return newMessage;
        }

        public string BiteStringToString(string _message)
        {
            string newMessage = "";                         //строка для вывода
            int numOfBytes = _message.Length / 7;           //количество байтов (char)
            int ascInt;                                     //массив битов
            for (int i = 0; i < numOfBytes; i++)
            {
                string oneBinaryByte = _message.Substring(7 * i, 7);    //извлекаем подстроку(один char)
                ascInt = Convert.ToInt32(oneBinaryByte, 2);            //преобразуем bin в dec
                newMessage += Convert.ToChar(ascInt);                 //преобразуем dec в ASCII символ
            }
            return newMessage;
        }

        public string CodingBiteString(string biteMessage)
        {
            returnString = "";
            int startSubstring = 0, lengthSubstring = 7;
            string polynom = "111111", nepolynom = "000000";
            for (int i = 0; i < biteMessage.Length / 7; i++)
            {
                substring = biteMessage.Substring(startSubstring, lengthSubstring) + "00000";              //get substring
                part = substring.Substring(0, 6);
                Console.WriteLine("substr- " + substring + "\tpart - " + part);
                Divide(polynom, nepolynom);
                
                part = partResult.Substring(1, 5);
                Console.WriteLine("last part - " + part);
                substring = substring.Substring(0,7) + part;
                Console.WriteLine("Final substring - " + substring +
                                  "\n________________________________");
                returnString += substring;
                startSubstring += lengthSubstring; 
            }
            return returnString;
        }

        public string DecodingBiteString(string biteMessage)
        {
            returnString = "";
            debugString = "";
            int startSubstring = 0, lengthSubstring = 12;
            string polynom = "111111", nepolynom = "000000";
            for (int i = 0; i < biteMessage.Length / lengthSubstring; i++)
            {
                substring = biteMessage.Substring(startSubstring, lengthSubstring);              //get substring
                part = substring.Substring(0, 6);
                debugSubstring = biteMessage.Substring(startSubstring, lengthSubstring);              //get substring
                Console.WriteLine("substr- " + substring + "\tpart - " + part);
                Divide(polynom, nepolynom);

                part = partResult.Substring(1, 5);
                Console.WriteLine("last part - " + part);
                if(part == "00000") 
                {
                    returnString += substring.Substring(0, 7);
                } else {
                    Console.WriteLine("Error in decode");
                    substring = AutoCorrect();
                    if (leftShiftCount != 0) { substring = rightShift(substring); }
                    returnString += substring.Substring(0, 7);
                }
                Console.WriteLine("Final substring - " + returnString +
                                  "\n________________________________");
                startSubstring += lengthSubstring;
                for(int j = 0, k=0; j< debugSubstring.Length; j++, k++) { 
                    if(k == 7){
                        if (debugString[j - 1] != ' ') { debugString += " ("; }
                    }
                    if (debugSubstring[j] == '[' || debugSubstring[j] == ']') { k--; }
                    debugString += debugSubstring[j];
                }
                if (debugString[debugString.Length - 1] != ')') { debugString += ")"; }
                debugString += ": " + part + "\n";
            }                                                       
            Console.WriteLine("debugString - " + debugString);
            return returnString;
        }
        
        public void Divide(string polynom, string nepolynom)
        {
            for (int j = 0, index = 6; j < 7; j++, index++)
            {
                partResult = "";
                if (part[0] == polynom[0])
                {
                    Console.WriteLine("div to 111111");
                    for (int k = 0; k < 6; k++)
                    {
                        if (part[k] == polynom[k])
                        {
                            partResult = partResult + "0";
                            Console.WriteLine("partResult[" + k + "] - " + partResult);
                        }
                        else
                        {
                            partResult = partResult + "1";
                            Console.WriteLine("partResult[" + k + "] - " + partResult);
                        }
                    }
                }
                else if (part[0] == nepolynom[0])
                {
                    Console.WriteLine("div to 000000");
                    for (int k = 0; k < 6; k++)
                    {
                        if (part[k] == nepolynom[k])
                        {
                            partResult = partResult + "0";
                            Console.WriteLine("partResult[" + k + "] - " + partResult);
                        }
                        else
                        {
                            partResult = partResult + "1";
                            Console.WriteLine("partResult[" + k + "] - " + partResult);
                        }
                    }
                }
                if (j != 6)
                {
                    part = partResult.Substring(1, 5);
                    // определить количество действий при делении 
                    part = part + substring[index];
                    Console.WriteLine("part - " + part);
                }
            }
        }

        public string MakeMistake(string message) {
            char[] newMessage = message.ToCharArray();
            for (int i = 0, j = 12; j < message.Length; i+=12,j+=12)
            {
                Console.WriteLine("rand mistake from "+ i + "to " + j);
                Random rnd = new Random();
                System.Threading.Thread.Sleep(20);
                int flag = rnd.Next(0, 100);
                if (flag < 30) {
                    Console.WriteLine("!!!!!!!!!!!!!!!!!!!!NEW MISTAKE!!!!!!!!!!!!!!!!!!!!!!");
                    System.Threading.Thread.Sleep(20);
                    int pos = rnd.Next(i, j);  // Заменить на длинну кадра
                    if (newMessage[pos] == '0') { newMessage[pos] = '1'; }
                    else { newMessage[pos] = '0'; }
                }
            }
            Console.WriteLine(newMessage);
            return new string(newMessage);
        }
        public string AutoCorrect() // буква с остатком и остаток от деления при получении
        {
            Console.WriteLine("//////////////////////////////////////////// \n autoCorrect Start");
            Console.WriteLine("substring - " + substring);
            while (true)
            {
                int weight = 0;
                // Check weight
                for (int i = 0; i < part.Length; i++){
                    if (part[i] == '1') { weight++; }
                }
                Console.WriteLine("weight - " + weight);
                // SUB/2 substring with NZeroPart
                if (weight <= 1)
                {
                    add();
                    Console.WriteLine("weight <= 1; add part to substring");
                    Console.WriteLine("substring - " + substring);
                    break;
                }
                // leftShift and repite divide, while weight !<= 1
                else
                {
                    substring = leftShift(substring);                   //mistake
                    part = substring.Substring(0, 6);
                    Console.WriteLine("weight > 1; leftshift and divide");
                    Console.WriteLine("substring LShifted- " + substring + "///////////////////////////////////\n"+ "NEW DIVIDE");
                    Divide("111111", "000000");   // 
                    Console.WriteLine("substring divided- " + substring);
                    part = partResult.Substring(1, 5);
                    Console.WriteLine("last part - " + part);
                }
            }
            return substring;
        }
        private void add()
        {
            char[] substrArray = substring.ToCharArray();
            char[] debugArray = substring.Substring(0, 7).ToCharArray();
            Array.Resize(ref debugArray, 14);
            char[] partArr = part.ToCharArray();
            for (int i = 0, j = (substring.Length - part.Length), x = (substring.Length - part.Length); i < part.Length; i++, j++, x++)
            {
                if (partArr[i] == substrArray[j]) {
                    if (partArr[i] == '1' && substrArray[j] == '1')
                    {
                        Console.WriteLine("_!debugSubstring[" + x + "] - " + new string(debugArray));
                        debugArray[x++] = '[';
                        Console.WriteLine("_!debugSubstring[" + x + "] - " + new string(debugArray));
                        debugArray[x++] = '0';
                        substrArray[j] = '0';
                        Console.WriteLine("_!debugSubstring[" + x + "] - " + new string(debugArray));
                        debugArray[x] = ']';
                        Console.WriteLine("_!debugSubstring[" + x + "] - " + new string(debugArray));
                        Console.WriteLine("_!substrSubstring[" + x + "] - " + new string(substrArray));
                    }
                    else
                    {
                        substrArray[j] = '0';
                        debugArray[x] = '0';
                    }
                }
                else {
                    if (substrArray[j] == '0' && partArr[i] == '1')
                    {
                        Console.WriteLine("_!debugSubstring[" + x + "] - " + new string(debugArray));
                        debugArray[x++] = '[';
                        Console.WriteLine("_!debugSubstring[" + x + "] - " + new string(debugArray));
                        debugArray[x++] = '1';
                        substrArray[j] = '1';
                        Console.WriteLine("_!debugSubstring[" + x + "] - " + new string(debugArray));
                        debugArray[x] = ']';
                        Console.WriteLine("_!debugSubstring[" + x + "] - " + new string(debugArray));
                        Console.WriteLine("_!substrSubstring[" + x + "] - " + new string(substrArray));
                    }
                    else
                    {
                        substrArray[j] = '1';
                        debugArray[x] = '1';
                    }
                }
                Console.WriteLine("debugSubstring[" + x + "] - " + new string(debugArray));
            }
            substring = new string(substrArray);
            debugSubstring = new string(debugArray);
            Console.WriteLine("(add)debugSubstring - " + debugSubstring);
            Console.WriteLine("added substring - " + substring);
        }
        private string leftShift(string message) {
            leftShiftCount++;
            char[] strArr = message.ToCharArray();
            char ch = strArr[0];
            for (int j = 0; j < strArr.Length; j++)
            {
                if (j == strArr.Length - 1)
                {
                    strArr[strArr.Length - 1] = ch;
                    break;
                }
                strArr[j] = strArr[j + 1];
            }
            message = new string(strArr);
            return message;
        }
        private string rightShift(string message)
        {
            Console.WriteLine("rightShift");
            while (leftShiftCount != 0)
            {
                char[] strArr = message.ToCharArray();
                char ch = strArr[strArr.Length - 1];
                for (int j = strArr.Length - 1; j >= 0; j--)
                {
                    if (j == 0)
                    {
                        strArr[j] = ch;
                        break;
                    }
                    strArr[j] = strArr[j - 1];
                }
                message = new string(strArr);
                Console.WriteLine("substring - " + message);
                leftShiftCount--;
            }
            return message;
        }
    }
}
