using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;

public class RomanNumerals
{
    private static readonly string[] R = { "M", "CM", "D", "CD", "C", "XC", "L", "XL", "X", "IX", "V", "IV", "I" };
    private static readonly int[] N = { 1000,  900, 500,  400, 100,   90,  50,   40,  10,    9,   5,    4,   1 };
  
    public static string ToRoman(int n)
    {
        Console.WriteLine(n);
        var s = new StringBuilder();
        for (var i = 0; i < N.Length; i++) {
            while (n >= N[i]) {
                s.Append(R[i]);
                n -= N[i];
            }
        }
        return s.ToString();
    }

    public static int FromRoman(string romanNumeral)
    {
        Console.WriteLine(romanNumeral);
        var v = 0;
        var s = romanNumeral;
        for (var i = 0; i < N.Length; i++) {
            while (s.Substring(0, Math.Min(s.Length, R[i].Length)) == R[i]) {
                s = s.Substring(R[i].Length);
                v += N[i];
            }
        }
        return v;
    }
  }