public class RomanNumerals
{   

    public static string ToRoman(int n)
    {
        return "";
    }

    public static int FromRoman(string romanNumeral)
    {   
        int iCurrent = 0;
        int iLast = 0;
        int result = 0;
        char[] charArr = romanNumeral.ToCharArray();
        foreach (char ch in charArr)
        {
            iCurrent = GetIntFromNumeral(ch);
            if (iCurrent>iLast && iLast!=0)
            {
                result += iCurrent-iLast;
                iLast = 0;
            }
            else
            {
                result += iCurrent;
                iLast = iCurrent;
            }
            
        }  
        return result;
    }
  
    public static int GetIntFromNumeral(char numeralChar)
    {
        int i = 0;
        switch(numeralChar)
        {
            case 'I':
                i = 1;
                break;
            case 'V':
                i = 5;
                break;
            case 'X':
                i = 10;
                break;
            case 'L':
                i = 50;
                break;
            case 'C':
                i = 100;
                break;    
            case 'D':
                i = 500;
                break;
            case 'M':
                i = 1000;
                break;
            default:
                break;
        }
        return i;
    }
  }