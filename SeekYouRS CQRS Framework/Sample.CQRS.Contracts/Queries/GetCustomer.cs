using System;

namespace Sample.CQRS.Contracts.Queries
{
    public class GetCustomer
    {
        public Guid Id { get; set; }
    }
}