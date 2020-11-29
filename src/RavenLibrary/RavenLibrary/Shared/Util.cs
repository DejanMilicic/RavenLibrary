namespace RavenLibrary.Shared
{
    public static class Util
    {
        static string usersBooksCollection = "UsersBooks";

        public static string GetUserBookCollection(string userId, string bookId)
        {
            return $"{usersBooksCollection}/{userId}-{bookId}/";
        }

        public static string GetUserBookCollectionUserPrefix(string userId)
        {
            return $"{usersBooksCollection}/{userId}-";
        }
    }
}
