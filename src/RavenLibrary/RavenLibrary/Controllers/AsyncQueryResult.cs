﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Raven.Client.Documents;
using Raven.Client.Documents.Linq;
using Raven.Client.Documents.Session;

namespace RavenLibrary.Controllers
{
    public class AsyncQueryResult<T> : IActionResult
    {
        private readonly IAsyncDocumentSession _session;
        private readonly IAsyncDocumentQuery<T> _query;

        public AsyncQueryResult(IAsyncDocumentSession session, IAsyncDocumentQuery<T> query)
        {
            _session = session;
            _query = query;
        }

        public AsyncQueryResult(IAsyncDocumentSession session, IQueryable<T> query) 
            : this(session, query.ToAsyncDocumentQuery())
        {
                
        }

        public async Task ExecuteResultAsync(ActionContext context)
        {
            using (_session)
            {
                var response = context.HttpContext.Response;
                response.ContentType = "application/json";
                await _session.Advanced.StreamIntoAsync(_query, response.Body, context.HttpContext.RequestAborted);
            }
        }

    }
}
