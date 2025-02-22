﻿using System.ComponentModel;

namespace AtmConsole.TextUi
{
    public static class Validator
    {
        public static T? Convert<T>(string prompt)
        {
            bool valid = false;
            string userInput;

            while (!valid)
            {
                userInput = Utility.GetUserInput(prompt);
                try
                {
                    var converter = TypeDescriptor.GetConverter(typeof(T));
                    if (converter != null)
                    {
                        return (T)converter.ConvertFromString(userInput);
                    }
                    else
                    {
                        return default;
                    }
                }
                catch
                {
                    Utility.PrintMessage("Invalid Input. Try again.", false);
                }
            }
            return default;
        }
    }
}
