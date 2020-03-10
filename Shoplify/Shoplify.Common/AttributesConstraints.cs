using System;

namespace Shoplify.Common
{
    public static class AttributesConstraints
    {
        #region User

        public const int UsernameMaxLength = 20;
        public const int UsernameMinLength = 3;
        public const int PasswordMaxLength = 100;
        public const int PasswordMinLength = 6;

        #endregion

        #region Category

        public const int CategoryNameMaxLength = 30;

        #endregion

        #region Message

        public const int MessageTextMaxLength = 255;

        #endregion

        #region Comment

        public const int CommentTextMaxLength = 255;

        #endregion

        #region Advertisement

        public const int AdvertisementNameMaxLength = 30;

        public const int AdvertisementNameMinLength = 3;

        public const string AdvertisementMinPrice = "0.1";

        public const string AdvertisementMaxPrice = "100000000";

        public const int AdvertisementDescriptionMaxLength = 1000;

        public const int AdvertisementAddressMaxLength = 100;

        public const int AdvertisementNumberMaxLength = 10;

        #endregion

        #region Report

        public const int ReportDescriptionMaxLength = 255;

        #endregion

        #region Order

        public const int OrderUserFirstNameMaxLength = 12;

        public const int OrderUserLastNameMaxLength = 20;

        public const string OrderMinPrice = "0.1";

        public const string OrderMaxPrice = "100000000";

        public const int OrderDescriptionMaxLength = 255;

        public const int OrderAddressMaxLength = 100;

        #endregion

        #region Courier

        public const int CourierFirstNameMaxLength = 12;

        public const int CourierLastNameMaxLength = 12;

        #endregion

        #region Notification

        public const int NotificationTextMaxLength = 255;

        #endregion
    }
}
