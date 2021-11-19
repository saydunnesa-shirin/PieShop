/*
 * Created By  	: Md. Mozaffar Rahman Sebu
 * Created Date	: Aug,19,2021
 * Updated By  	: Md. Mozaffar Rahman Sebu
 * Updated Date	: Aug,19,2021
 * (c) Datavanced LLC
 */

using System.ComponentModel;

namespace PassionCare.Utility.Enums
{
    public class AppEnums
    {
        public enum Status
        {
            Active = 1,
            InActive = 2,
            Deleted = 3
        }


        public enum MessageUserType
        {
            Secretary = 1,
            Patient = 2,
            Aide = 3
        }

        public enum UserTypeNote
        {
            Patient = 1,
            Aide = 2
        }

        public enum MessageType
        {
            Text = 1,
            Image = 2,
            File = 3
        }

        public enum OrganizationType
        {
            Providence = 1589,
            EmblemHealth = 2,
            Teleflex = 3,
            Cigna = 4,
            Aetna = 5,
        }

        public enum ResponseCode
        {
            Success = 200,
            InternalServerError = 500,
            Failed = 404,
            Warning = 400,
            UnAuthorize = 401
        }

        public enum DaysOfWeek
        {
            Sunday = 1,
            Monday = 2,
            Tuesday = 3,
            Wednesday = 4,
            Thursday = 5,
            Friday = 6,
            Saturday = 7,
            
        }

        public enum TokenAlgorithm
        {
            [Description("HS256")]
            HmacSha256,
            [Description("HS384")]
            HmacSha384,
            [Description("HS512")]
            HmacSha512,
            [Description("RS256")]
            RsaSha256,
            [Description("RS384")]
            RsaSha384,
            [Description("RS512")]
            RsaSha512,
            [Description("ES256")]
            EcdsaSha256,
            [Description("ES384")]
            EcdsaSha384,
            [Description("ES512")]
            EcdsaSha512,
            [Description("PS256")]
            RsaSsaPssSha256,
            [Description("PS384")]
            RsaSsaPssSha384
        }

        public enum ValidationType
        {
            IsNullOrEmpty = 1,
            IsNullOrWhiteSpace = 2,
            IsLessThanOrEqual = 3,
            IsGreaterThan = 4,
            IsGreaterThanOrEqual = 5,
            IsEqual = 6,
            IsNotEqual = 7,
            IsValidEmail = 8,
            IsNull = 9,
            IsDuplicateItem = 10,
            IsPositiveInteger = 11,
            IsPositiveIntegerOrNull = 12,
            IsLengthLessThan = 13,
            IsLengthLessThanOrEqual = 14,
            IsLengthGreaterThan = 15,
            IsLengthGreaterThanOrEqual = 16,
            IsValidDate = 17,
            IsNullObject = 18
        }

        public enum ScheduleTaskStatus
        {
            Ongoing = 1,
            Canceled = 2,
            Deleted = 3,
            Completed = 4
        }

        public enum Source
        {
            HHAX = 1,
            SMARTSHEET = 2,
            INDEED = 3
        }
    }
}
