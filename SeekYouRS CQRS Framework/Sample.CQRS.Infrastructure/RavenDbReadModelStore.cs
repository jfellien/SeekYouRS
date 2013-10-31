using System.Collections.Generic;
using System.Linq;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Listeners;
using SeekYouRS.Contracts;

namespace Sample.CQRS.Infrastructure
{
    public class RavenDBReadModelStore : IStoreAndRetrieveReadModels
    {
        readonly DocumentStore _store;

        public RavenDBReadModelStore()
        {
            _store = new DocumentStore
            {
                ConnectionStringName = "MyConnection"
            };
            _store.Initialize();

            // Wird nur für das Unit Testing benötigt
            _store.RegisterListener(new NoStaleQueriesListener());
        }

        public void Add(dynamic model)
        {
            using (var session = _store.OpenSession())
            {
                session.Store(model);
                session.SaveChanges();
            }
        }

        public void Change(dynamic model)
        {
            using (var session = _store.OpenSession())
            {
                var modelTypeName = model.GetType().Name;
                var modelId = string.Format("{0}s/{1}", modelTypeName, model.Id.ToString());

                session.Store(model, modelId);
                session.SaveChanges();
            }
        }

        public void Remove(dynamic model)
        {
            using (var session = _store.OpenSession())
            {
                var modelTypeName = model.GetType().Name;
                var modelId = model.Id;
                var query = string.Format("{0}s/{1}", modelTypeName, modelId);

                var toRemove = session.Load<object>(query);

                session.Delete<object>(toRemove);
                session.SaveChanges();
            }
        }

        public IEnumerable<T> Retrieve<T>()
        {
            using (var session = _store.OpenSession())
            {
                return session.Query<T>().ToList();
            }
        }
    }

    /// <summary>
    ///     Diese Klasse wird für das Unit testing benötigt, da das Aufbauen des Index zu langsam für die Tests ist
    /// </summary>
    internal class NoStaleQueriesListener : IDocumentQueryListener
    {
        public void BeforeQueryExecuted(IDocumentQueryCustomization queryCustomization)
        {
            queryCustomization.WaitForNonStaleResults();
        }
    }
}