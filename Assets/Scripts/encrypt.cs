using System;
using System.Collections.Generic;

namespace ConsoleApp1
{
    class Program
    {

        static char[] letters = new char[] {'a', 'b','c','d','e','f','g','h','i','j','k','l','m','n','o','p','q','r','s','t','u','v','w','x','y','z'};
        static int shiftAmount = 0;
        static int shiftDirectionMultiplier = 1;//change to -1 to decode

    static void Main(string[] args)
    {
       //Take user input
            Console.WriteLine("Type your message:\n");
            string userText = Console.ReadLine().ToLower();

            //  take string and turn into int32
            Console.WriteLine("Type the shift number: \n ");
            shiftAmount = Convert.ToInt32(Console.ReadLine());
            Encrypt(userText);
    }

    static int ShiftLetterIndex(char letter)
    {
        int tmpIndex = 0;
        for(int i = 0; i < letters.Length; i++)
        {
            if(letters[i] == letter)
            {
                tmpIndex = i;
                break;
            }
        }
        tmpIndex = (tmpIndex+(shiftAmount*shiftDirectionMultiplier))%letters.Length-1;
        return tmpIndex;
    }

    static string Encrypt(string toEncode)
    {
        shiftDirectionMultiplier = 1;
        return ShiftString(toEncode);
    }

    static string ShiftString(string s)
    {
        string tmpStr = "";
        s = s.ToLower();
        //Console.WriteLine(toEncode[0]);
        foreach(char c in s)
        {
           if(Array.Exists(letters, element => element == c)) tmpStr += letters[ShiftLetterIndex(c)];
           else tmpStr += c;
        }
        Console.WriteLine(tmpStr);
        return tmpStr;
    }

    static string Decrypt(string toDecode)
    {
        shiftDirectionMultiplier = -1;
        return ShiftString(toDecode);
    }

    }
}
