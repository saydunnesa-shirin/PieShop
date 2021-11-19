/*
 * Created By  	: Md. Mozaffar Rahman Sebu
 * Created Date	: Aug,19,2021
 * Updated By  	: Md. Mozaffar Rahman Sebu
 * Updated Date	: Sep,30,2021
 * (c) Datavanced LLC
 */

using PassionCare.Utility.Enums;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;

namespace PassionCare.Utility.Helpers
{
    public class ValidationHelper
    {
        /// <summary>
        /// Generic Method to validate http request's model and generate response stream
        /// </summary>
        /// <param name="tuples"></param>
        /// <returns></returns>
        public static (HttpStatusCode httpCode, string message) ValidateHttpReqMethod(params (object @object, string message, int type, object valueToCompare)[] tuples)
        {
            foreach (var (@object, message, type, valueToCompare) in tuples)
            {
                switch (type)
                {
                    case 0:
                        return new ValueTuple<HttpStatusCode, string>(HttpStatusCode.BadRequest, message);
                    case (int)AppEnums.ValidationType.IsNullOrEmpty:
                        if (@object is string var1 && string.IsNullOrWhiteSpace(var1))
                            goto case 0;
                        break;
                    case (int)AppEnums.ValidationType.IsPositiveInteger:
                        if (!IsPositiveInteger(@object))
                            goto case 0;
                        break;
                    case (int)AppEnums.ValidationType.IsPositiveIntegerOrNull:
                        if (!IsPositiveIntegerOrNull(@object))
                            goto case 0;
                        break;
                    case (int)AppEnums.ValidationType.IsLessThanOrEqual:
                        if (@object is <= 0)
                            goto case 0;
                        break;
                    case (int)AppEnums.ValidationType.IsGreaterThanOrEqual:
                        if (!IsGreaterThanOrEqual(@object, valueToCompare))
                            goto case 0;
                        break;
                    case (int)AppEnums.ValidationType.IsGreaterThan:
                        if (!IsGreaterThan(@object, valueToCompare))
                            goto case 0;
                        break;
                    case (int)AppEnums.ValidationType.IsLengthLessThan:
                        if (!IsLengthLessThan((string)@object, (int)valueToCompare))
                            goto case 0;
                        break;
                    case (int)AppEnums.ValidationType.IsLengthLessThanOrEqual:
                        if (!IsLengthLessThanOrEqual((string)@object, (int)valueToCompare))
                            goto case 0;
                        break;
                    case (int)AppEnums.ValidationType.IsLengthGreaterThan:
                        if (!IsLengthGreaterThan((string)@object, (int)valueToCompare))
                            goto case 0;
                        break;
                    case (int)AppEnums.ValidationType.IsLengthGreaterThanOrEqual:
                        if (!IsLengthGreaterThanOrEqual((string)@object, (int)valueToCompare))
                            goto case 0;
                        break;
                    case (int)AppEnums.ValidationType.IsValidDate:
                        if (!IsValidDate(@object))
                            goto case 0;
                        break;
                    case (int)AppEnums.ValidationType.IsNullObject:
                        if (@object is null)
                            goto case 0;
                        break;
                    case (int)AppEnums.ValidationType.IsValidEmail:
                        if (!IsValidEmail((string)@object))
                            goto case 0;
                        break;
                    default:
                        break;
                }
            }
            return new ValueTuple<HttpStatusCode, string>(HttpStatusCode.OK, string.Empty);
        }

        /// <summary>
        /// Generic Method to validate http request's model and generate response stream
        /// </summary>
        /// <param name="tuples">
        ///    object = value,
        ///    string = warning essage
        ///    int = validation type
        ///    object = value comparer
        /// </param>
        /// <returns></returns>
        public static List<(HttpStatusCode httpCode, string message)> ValidateHttpReqMethodList(params (object @object, string message, int type, object valueToCompare)[] tuples)
        {
            var returnTuple = new List<(HttpStatusCode httpCode, string message)>();
            foreach (var (@object, message, type, valueToCompare) in tuples)
            {
                switch (type)
                {
                    case 0:
                        returnTuple.Add(new ValueTuple<HttpStatusCode, string>(HttpStatusCode.BadRequest, message));
                        break;
                    case (int)AppEnums.ValidationType.IsNullOrEmpty:
                        if (@object is string var1 && string.IsNullOrEmpty(var1))
                            goto case 0;
                        break;

                    case (int)AppEnums.ValidationType.IsLessThanOrEqual:
                        if (@object is <= 0)
                            goto case 0;
                        break;
                    default:
                        break;
                }
            }
            return returnTuple;
        }

        #region
        public static bool IsLengthLessThan(string text, int valueToCompare)
        {
            return !string.IsNullOrWhiteSpace(text) && text.Length < valueToCompare;
        }

        public static bool IsLengthLessThanOrEqual(string text, int valueToCompare)
        {
            return !string.IsNullOrWhiteSpace(text) && text.Length <= valueToCompare;
        }

        public static bool IsLengthGreaterThan(string text, int valueToCompare)
        {
            return !string.IsNullOrWhiteSpace(text) && text.Length > valueToCompare;
        }

        public static bool IsLengthGreaterThanOrEqual(string text, int valueToCompare)
        {
            return !string.IsNullOrWhiteSpace(text) && text.Length >= valueToCompare;
        }

        public static bool IsPositiveInteger(object obj)
        {
            return obj is >= 0;
        }
        public static bool IsPositiveIntegerOrNull(object obj)
        {
            return obj is >= 0 or null;
        }
        public static bool IsGreaterThanOrEqual(object obj, object valueToCompare)
        {
            switch (obj)
            {
                case int obj1 when valueToCompare is int valueToCompare1 && obj1 >= valueToCompare1:
                case double val2 when valueToCompare is double valueToCompare2 && val2 >= valueToCompare2:
                case decimal val3 when valueToCompare is decimal valueToCompare3 && val3 >= valueToCompare3:
                    return true;
                default:
                    return false;
            }
        }
        public static bool IsGreaterThan(object obj, object valueToCompare)
        {
            switch (obj)
            {
                case int obj1 when valueToCompare is int valueToCompare1 && obj1 > valueToCompare1:
                case double val2 when valueToCompare is double valueToCompare2 && val2 > valueToCompare2:
                case decimal val3 when valueToCompare is decimal valueToCompare3 && val3 > valueToCompare3:
                    return true;
                default:
                    return false;
            }
        }
        public static bool IsValidDate(object obj)
        {
            return obj is DateTime;
        }

        private static bool IsValidEmail(string emailAddress)
        {
            const string validEmailPattern = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                                             @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                                             @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            //@"^[a-z0-9][-a-z0-9._]+@([-a-z0-9]+[.])+[a-z]{2,5}$"
            return new Regex(validEmailPattern, RegexOptions.IgnoreCase).IsMatch(emailAddress);
        }

        #endregion
    }
}
