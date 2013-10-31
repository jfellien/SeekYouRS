using System;

namespace Sample.CQRS.Contracts.Commands
{
    public class ChangeCustomer
    {
        public string Name { get; set; }

        public Guid Id { get; set; }
    }
}