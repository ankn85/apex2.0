namespace Apex.Services.Constants
{
    public static class SystemKeyword
    {
        public const string UpdateActivityLogTypes = "UpdateActivityLogTypes";

        public const string CreateEmailAccounts = "CreateEmailAccounts";
        public const string UpdateEmailAccounts = "UpdateEmailAccounts";
        public const string DeleteEmailAccounts = "DeleteEmailAccounts";

        public const string CreateUsers = "CreateUsers";
        public const string UpdateUsers = "UpdateUsers";
        public const string DeleteUsers = "DeleteUsers";
        public const string ResetPasswordUsers = "ResetPasswordUsers";

        public static string GetSystemKeyword(string controller, string action)
        {
            string systemKeyword;

            switch (action)
            {
                case "Post":
                    systemKeyword = "Create";
                    break;

                case "Put":
                    systemKeyword = "Update";
                    break;

                default:
                    systemKeyword = action;
                    break;
            }

            return systemKeyword + controller;
        }
    }
}