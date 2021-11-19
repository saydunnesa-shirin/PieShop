/*
 * Created By  	: Md. Mozaffar Rahman Sebu
 * Created Date	: Aug,19,2021
 * Updated By  	: Md. Mozaffar Rahman Sebu
 * Updated Date	: Aug,19,2021
 * (c) Datavanced LLC
 */

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PieShop.Utility.Constants;
using System;
using System.ComponentModel;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;

namespace Chesed.Utility.Extensions
{
    public static class AppExtensions
    {
        public static DateTime GetLocalZoneDate(this DateTime date)
        {
            return TimeZoneInfo.ConvertTimeFromUtc(date, TimeZoneInfo.Local).Date;
        }

        public static bool EqualsWithLower(this string value1, string value2)
        {
            return value1.ToLower().Equals(value2.ToLower());
        }

        public static string ToDescription<TEnum>(this TEnum EnumValue) where TEnum : IConvertible
        {
            if (EnumValue is Enum)
            {
                Type type = EnumValue.GetType();
                Array values = System.Enum.GetValues(type);

                foreach (int val in values)
                {
                    if (val == EnumValue.ToInt32(CultureInfo.InvariantCulture))
                    {
                        var memInfo = type.GetMember(type.GetEnumName(val));
                        var descriptionAttribute = memInfo[0]
                            .GetCustomAttributes(typeof(DescriptionAttribute), false)
                            .FirstOrDefault() as DescriptionAttribute;

                        if (descriptionAttribute != null)
                        {
                            return descriptionAttribute.Description;
                        }
                    }
                }
            }

            throw new ArgumentNullException();
        }

        public static T GetValueByJsonKey<T>(this JsonResult jObject, string key)
        {
            string json = JsonConvert.SerializeObject(jObject.Value, Formatting.Indented);
            JObject obj = JObject.Parse(json);
            return (T)Convert.ChangeType(obj[key], typeof(T));
        }

        public static T GetTokenValue<T>(this HttpRequest context, string claimType)
        {
            try
            {
                var value = (new JwtSecurityTokenHandler().ReadToken(context.HttpContext.Request.GetTokenFromRequest()) as JwtSecurityToken)
                    .Claims.First(claim => claim.Type == claimType).Value;
                return (T)Convert.ChangeType(value, typeof(T));
            }
            catch (Exception ex)
            {
                return (T)Convert.ChangeType(null, typeof(T));
            }
        }

        public static string GetTokenFromRequest(this HttpRequest context)
        {
            string fullToken = context.Headers[AppHttpHeaders.Token];
            return fullToken.Replace(AppHttpHeaders.AuthenticationSchema, "");
        }
    }
}
