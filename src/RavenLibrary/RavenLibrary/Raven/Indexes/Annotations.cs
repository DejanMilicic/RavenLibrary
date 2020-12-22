using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Raven.Client.Documents.Indexes;

namespace RavenLibrary.Raven.Indexes
{
    public class Annotations : AbstractIndexCreationTask
    {
        public override string IndexName => "Annotations";

        public override IndexDefinition CreateIndexDefinition()
        {
            return new IndexDefinition
            {
                Maps =
                {
                    @"from u in docs.UserBooks
select new {
    u.user,
    u.at,
    at_yyyy = u.at.ToString(""yyyy""),
    at_yyyy_MM = u.at.ToString(""yyyy-MM""),
    at_yyyy_MM_dd = u.at.ToString(""yyyy-MM-dd"")
}"
                }
            };
        }
    }
}
