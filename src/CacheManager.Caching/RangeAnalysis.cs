using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace CacheManager.Caching
{
    public class RangeAnalysis
    {
        private string _key;
        private MatchCollection _match;

        public RangeAnalysis(string key)
        {
            _key = key;
            _match = Regex.Matches(key, _regexRule);
        }
        private const string _regexRule = @"{[A-z0-9]*\-[A-z0-9]*}";


        public List<Ranges> Analysis()
        {
            List<Ranges> list = new List<Ranges>();
            if (_match == null || _match.Count == 0)
            {
                return list;
            }
            char[] splitChar = { '{', '}', '-' };
            foreach (Match item in _match)
            {
                if (ValidateCharRule(item.Value))
                {
                    string[] result = item.Value.Split(splitChar, StringSplitOptions.RemoveEmptyEntries);
                    CharRanges cr = new CharRanges();
                    cr.From = result[0].ToCharArray()[0];
                    cr.To = result[1].ToCharArray()[0];
                    list.Add(cr);
                }
                else if (ValidateNumberRule(item.Value))
                {
                    string[] result = item.Value.Split(splitChar, StringSplitOptions.RemoveEmptyEntries);
                    NumberRanges nr = new NumberRanges();
                    nr.From = Convert.ToInt32(result[0]);
                    nr.To = Convert.ToInt32(result[1]);
                    list.Add(nr);
                }
            }
            return list;
        }

        public string Format()
        {
            if (_match == null || _match.Count == 0)
            {
                return _key;
            }
            int index = 0;
            int charIndex = 0;
            StringBuilder str = new StringBuilder();
            foreach (Match item in _match)
            {
                if (ValidateCharRule(item.Value) || ValidateNumberRule(item.Value))
                {
                    if (item.Index != charIndex)
                    {
                        str.Append(_key.Substring(charIndex, item.Index - charIndex));
                    }
                    str.Append("{");
                    str.Append(index);
                    str.Append("}");
                    charIndex = item.Index + item.Length;
                    index++;
                }
                else
                {
                    charIndex = item.Index + item.Length;
                }
            }
            if (charIndex != _key.Length)
            {
                str.Append(_key.Substring(charIndex));
            }
            return str.ToString();
        }

        private bool ValidateCharRule(string rule)
        {
            string regex = @"{[A-z]\-[A-z]}";

            if (Regex.IsMatch(rule, regex))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool ValidateNumberRule(string rule)
        {
            string regex = @"{\d+\-\d+}";
            if (Regex.IsMatch(rule, regex))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
