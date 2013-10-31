using System;

namespace Sample.CQRS.Contracts.Models
{
    public class CustomerModel
    {
        public string Name { get; set; }

        public Guid Id { get; set; }
    }
}