using System;

namespace Sample.CQRS.Contracts.Commands
{
    public class CreateCustomer
    {
        public string Name { get; set; }

        public Guid Id { get; set; }
    }
}