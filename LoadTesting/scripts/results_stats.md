getAnnotationsForUserBook_All
http://18.194.13.34:5000/annotations/userbook?userBookId=users/2407963-ebooks/46133
result size: 3.0MB

getAnnotationsForUser_All
http://18.194.13.34:5000/annotations/user?userId=users/2407963
result: 
InvalidOperationException: 'AsyncEnumerableReader' reached the configured maximum size of the buffer when enumerating a value of type 'RavenLibrary.Controllers.AnnotationController+<GetUserAnnotations>d__5'. This limit is in place to prevent infinite streams of 'IAsyncEnumerable<>' from continuing indefinitely. If this is not a programming mistake, consider ways to reduce the collection size, or consider manually converting 'RavenLibrary.Controllers.AnnotationController+<GetUserAnnotations>d__5' into a list rather than increasing the limit.

getBooksForUser_All
http://18.194.13.34:5000/books/user?userId=users/2407963
InvalidOperationException: 'AsyncEnumerableReader' reached the configured maximum size of the buffer when enumerating a value of type 'RavenLibrary.Controllers.BookController+<GetUserBooks>d__5'. This limit is in place to prevent infinite streams of 'IAsyncEnumerable<>' from continuing indefinitely. If this is not a programming mistake, consider ways to reduce the collection size, or consider manually converting 'RavenLibrary.Controllers.BookController+<GetUserBooks>d__5' into a list rather than increasing the limit.