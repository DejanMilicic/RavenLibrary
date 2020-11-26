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
                string db = "Library";

                IDocumentStore store = new DocumentStore()
                {
                    Urls = new[] { "http://localhost:8080" },
                    Database = db
                };

                store = store.Initialize();

                var dbr = store.Maintenance.Server.Send(new GetDatabaseRecordOperation(db));
                dbr.Settings["Indexing.DisableQueryOptimizerGeneratedIndexes"] = true.ToString();
                store.Maintenance.Server.Send(new UpdateDatabaseOperation(dbr, dbr.Etag));

                return store;
            });

        public static IDocumentStore Store => LazyStore.Value;
    }
}
