using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RavenLibrary.Shared
{
    public static class Util
    {
        public static string GetUserBookId(string userId, string bookId)
        {
            return $"UsersBooks/{userId}-{bookId}/";
        }
    }
}
