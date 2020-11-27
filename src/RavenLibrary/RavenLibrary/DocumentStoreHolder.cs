using System;
using Raven.Client.Documents;
using Raven.Client.ServerWide.Operations;

namespace RavenLibrary
{
    public static class DocumentStoreHolder
    {
        private static readonly Lazy<IDocumentStore> LazyStore =
            new Lazy<IDocumentStore>(() =>
            {
                var store = new DocumentStore
                {
                    Urls = new[] { "http://localhost:8080" },
                    Database = "Library"
                };

                return store.Initialize();
            });

        public static IDocumentStore Store => LazyStore.Value;
    }
}
