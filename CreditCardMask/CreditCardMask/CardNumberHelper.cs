using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace CreditCardMask
{
    public class CardNumberHelper
    {
        /*
        '0' - обязательная цифра
        '9' - необязательная цифра
        '?' – обязательный любой символ
        '*' – необязательный любой символ
        'A' – обязательная цифра или буква
        'a' – необязательная цифра или буква
        */

        private static readonly char[] _maskChars = { '0', '9', '?', '*', 'A', 'a' };

        public List<string> Masks { get; private set; }
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
                List<string> masks;

                if (string.IsNullOrEmpty(mask))
                {
                    masks = new List<string>();
                }
                else
                {
                    masks = new List<string> { mask };
                    Masks = GetMasks(masks, '9', '0');
                    Masks = GetMasks(masks, '*', '?');
                    Masks = GetMasks(masks, 'a', 'A');
                }

                patterns = new List<string>();
                if (Masks != null)
                    foreach (string m in Masks)
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

                if (Masks != null)
                {
                    foreach (string m in Masks)
                        texts.Add(Format(text, m));

                    for (int i = 0; i < Masks.Count; i++)
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

                    var maskMaxLen = Masks.Max(x => x.Length);
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

        private static List<string> GetMasks(List<string> masks, char optional, char mandatory)
        {
            if (masks?.Count == 0) return new List<string>();

            var flag = true;
            while (flag)
            {
                flag = false;

                for (int i2 = 0; i2 < masks.Count; i2++)
                {
                    for (int i1 = 0; i1 < masks[i2].Length; i1++)
                    {
                        var char1 = masks[i2][i1];

                        if (char1 == optional)
                        {
                            var left = masks[i2].Substring(0, i1);
                            var right = masks[i2].Substring(i1 + 1, masks[i2].Length - i1 - 1);
                            var mask1 = (left + mandatory + right).Trim();
                            var mask2 = (left + right).Trim();
                            masks.RemoveAt(i2);
                            if (!masks.Contains(mask1)) masks.Add(mask1);
                            if (mask2 != "" && !masks.Contains(mask2)) masks.Add(mask2);
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
                    case '0':
                        sb1.Append("[0-9]");
                        break;
                    case '?':
                        sb1.Append(".");
                        break;
                    case 'A':
                        sb1.Append("[a-zA-Z0-9]");
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
