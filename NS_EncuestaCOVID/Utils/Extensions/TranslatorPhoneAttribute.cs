using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace NS_EncuestaCOVID.Utils.Extensions
{
    public class TranslatorPhoneAttribute : DataTypeAttribute
    {
        private string _fieldNameTextIdentifier;
        public TranslatorPhoneAttribute(DataType dataType, string fieldNameTextIdentifier) : base(dataType)
        {
            _fieldNameTextIdentifier = fieldNameTextIdentifier;
        }

        public override bool IsValid(object value)
        {
            if (value != null)
            {
                if (Regex.IsMatch(value.ToString(), @"^(\+\s?)?((?<!\+.*)\(\+?\d+([\s\-\.]?\d+)?\)|\d+)([\s\-\.]?(\(\d+([\s\-\.]?\d+)?\)|\d+))*(\s?(x|ext\.?)\s?\d+)?$", RegexOptions.IgnoreCase))
                {
                    return true;
                }
            }
            

            ErrorMessage = _fieldNameTextIdentifier + " tiene formato inválido";
            return false;
        }
    }
}