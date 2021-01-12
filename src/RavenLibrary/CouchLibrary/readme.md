# Setup

docker run -d --name db -p 8091-8096:8091-8096 -p 11210-11211:11210-11211 couchbase
docker run -d -p 8091-8096:8091-8096 -p 11210-11211:11210-11211 couchbase

http://localhost:8091/

# Define Indexes

CREATE PRIMARY INDEX ON `Library`

# Resources

https://docs.couchbase.com/dotnet-sdk/current/hello-world/overview.html
https://docs.couchbase.com/dotnet-sdk/current/hello-world/start-using-sdk.html
https://github.com/couchbaselabs

https://medium.com/@punskaari/how-to-get-started-with-couchbase-with-asp-net-core-using-linq2couchbase-and-dependency-injection-e27b794d8126

https://github.com/iSatishYadav/getting-started-with-couchbase-linq2couchbase-aspnetcore-dependency-injection

