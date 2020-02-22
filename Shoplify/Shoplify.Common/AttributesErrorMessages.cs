namespace Shoplify.Common
{
    public static class AttributesErrorMessages
    {
        #region User

        public const string UsernameInvalidLength = "Username must be at least {2} and at max {1} characters long.";
        public const string PasswordInvalidLength = "Password must be at least {2} and at max {1} characters long.";
        public const string ConfirmPasswordInvalid = "The password and confirmation password do not match.";

        #endregion

        #region Advertisement

        public const string AdvertisementTitleInvalidLength = "Title must be at min {2} characters and max {1} characters long.";
        public const string AdvertisementTitleRequired = "Title is required.";
        public const string AdvertisementInvalidNumber = "Phone number should be valid!";

        #endregion
    }
}
