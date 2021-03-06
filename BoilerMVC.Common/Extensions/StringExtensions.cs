﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BoilerMVC
{
    public static class StringExtensions
    {
        public static string Shorten(this string str, int toLength, string cutOffReplacement = " ...")
        {
            if (string.IsNullOrEmpty(str) || str.Length <= toLength)
                return str;
            else
                return str.Remove(toLength) + cutOffReplacement;
        }

        public static string SeperateWords(this string str)
        {
            if (string.IsNullOrWhiteSpace(str))
                return str;

            string output = "";
            char[] chars = str.ToCharArray();

            for (int i = 0; i < chars.Length; i++)
            {
                if (i == chars.Length - 1 || i == 0 || char.IsWhiteSpace(chars[i]))
                {
                    output += chars[i];
                    continue;
                }

                if (char.IsUpper(chars[i]) && char.IsLower(chars[i - 1]))
                    output += " " + chars[i];
                else
                    output += chars[i];
            }

            return output;
        }
    }
}