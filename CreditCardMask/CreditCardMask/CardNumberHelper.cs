using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace CreditCardMask
{
    public class CardNumberHelper
    {
        private const char AnyCharMask = '*'; // обязательное, любой символ
        private const char LetterCharMask = '?'; // обязательное, буква
        private const char NumberCharMask = '0'; // обязательное, цифра
        private const char AnyNumberMask = '9'; // не обязательное, цифра
        private static readonly char[] _maskChars = { NumberCharMask, AnyCharMask, LetterCharMask, AnyNumberMask };

        private List<string> masks;
        private List<string> patterns;
        private List<string> texts;

        public string FormattedText { get; set; }
        public bool IsValid { get; set; }

        private string mask;
        public string Mask
        {
            get => mask;
            set
            {
                mask = value;
                masks = GetMasks(mask);

                patterns = new List<string>();
                if (masks != null)
                    foreach (string m in masks)
                        patterns.Add(GetPattern(m));

            }
        }

        private string text;
        public string Text
        {
            get => text;
            set
            {
                text = value;

                texts = new List<string>();
                IsValid = false;
                FormattedText = text;

                if (masks != null)
                {
                    foreach (string m in masks)
                        texts.Add(Format(text, m));

                    for (int i = 0; i < masks.Count; i++)
                    {
                        if (Regex.IsMatch(texts[i], patterns[i]))
                        {
                            FormattedText = texts[i];
                            IsValid = true;
                            break;
                        }
                    }
                }

                if (!IsValid && texts.Count > 0)
                {
                    FormattedText = texts[0];

                    var maskMaxLen = masks.Max(x => x.Length);
                    if (FormattedText.Length > maskMaxLen)
                        FormattedText = FormattedText.Substring(0, maskMaxLen);
                }
            }
        }

        //

        public CardNumberHelper() { }

        public CardNumberHelper(string mask, string text)
        {
            Mask = mask;
            Text = text;
        }

        //

        private static List<string> GetMasks(string mask)
        {
            var masks = new List<string> { mask };

            var flag = true;
            while (flag)
            {
                flag = false;

                for (int i2 = 0; i2 < masks.Count; i2++)
                {
                    for (int i1 = 0; i1 < masks[i2].Length; i1++)
                    {
                        if (masks[i2][i1] == '9')
                        {
                            string left = masks[i2].Substring(0, i1);
                            string right = masks[i2].Substring(i1, masks[i2].Length - i1 - 1);
                            var mask1 = (left + '0' + right).Trim();
                            var mask2 = (left + right).Trim();
                            masks.RemoveAt(i2);
                            if (!masks.Contains(mask1)) masks.Add(mask1);
                            if (!masks.Contains(mask2)) masks.Add(mask2);
                            flag = true;
                            break;
                        }
                    }
                    if (flag) break;
                }
            }

            return masks;
        }

        private static string Format(string text, string mask)
        {
            var result = "";
            var i2 = 0;
            foreach (var @char in mask)
            {
                if (i2 == text.Length) return result;
                if (_maskChars.Contains(@char))
                {
                    result += text[i2];
                    i2++;
                }
                else
                {
                    if (text[i2] == @char) i2++;
                    result += @char;
                }
            }
            if (i2 < text.Length) result += text.Substring(i2);

            return result;
        }

        private static string GetPattern(string mask)
        {
            var sb1 = new StringBuilder("^");

            foreach (var @char in mask)
            {
                switch (@char)
                {
                    case '*':
                        sb1.Append(".");
                        break;
                    case '?':
                        sb1.Append("[a-zA-Z]");
                        break;
                    case '0':
                        sb1.Append("[0-9]");
                        break;
                    case '9':
                        sb1.Append("[0-9]?");
                        break;
                    case ' ':
                        sb1.Append("\\s");
                        break;
                    default:
                        sb1.Append($"[\"{@char}\"]");
                        break;
                }
            }
            sb1.Append("$");

            return sb1.ToString();
        }
    }
}
