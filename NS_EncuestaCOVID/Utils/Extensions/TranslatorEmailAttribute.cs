using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace NS_EncuestaCOVID.Utils.Extensions
{
    public class TranslatorEmailAttribute : DataTypeAttribute
    {
        private string _fieldNameTextIdentifier;


        public TranslatorEmailAttribute(DataType dataType, string fieldNameTextIdentifier) : base(dataType)
        {
            _fieldNameTextIdentifier = fieldNameTextIdentifier;
        }

        public override bool IsValid(object value)
        {
            if (value != null)
            {

                if (value.ToString() == "")
                {
                    return true;
                }

                if (Regex.IsMatch(value.ToString(), @"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", RegexOptions.IgnoreCase))
                {
                    return true;
                }
            }
            else
            {
                return true;
            }

            
            ErrorMessage = _fieldNameTextIdentifier + " no tiene un formato válido";
            return false;
        }
    }
}