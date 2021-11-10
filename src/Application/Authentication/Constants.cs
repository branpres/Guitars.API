namespace Application.Authentication;

public static class Constants
{
    public static class Roles
    {
        public const string ADMIN = "Admin";
        public const string READ_ONLY_USER = "Read Only User";
    }

    public static class Claims
    {
        public const string REFRESH_TOKEN = "RefreshToken";

        public const string CREATE_GUITAR = "CreateGuitar";
        public const string READ_GUITAR = "ReadGuitar";
        public const string UPDATE_GUITAR = "UpdateGuitar";
        public const string DELETE_GUITAR = "DeleteGuitar";
        public const string STRING_GUITAR = "StringGuitar";
        public const string TUNE_GUITAR = "TuneGuitar";
    }

    public static class ClaimTypes
    {
        public const string HAS_PERMISSION = "Has Permission";
    }

    public static class Policies
    {
        public const string WRITE = "Write";
        public const string READ = "Read";
    }
}