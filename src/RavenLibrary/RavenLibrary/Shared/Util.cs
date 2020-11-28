namespace RavenLibrary.Shared
{
    public static class Util
    {
        static string usersBooksCollection = "UsersBooks";

        public static string GetUserBookId(string userId, string bookId)
        {
            return $"{usersBooksCollection}/{userId}-{bookId}/";
        }
        public static string GetUserBookIdPrefix(string userId)
        {
            return $"{usersBooksCollection}/{userId}-";
        }
    }
}
