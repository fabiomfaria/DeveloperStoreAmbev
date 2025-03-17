using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Entities
{
    public class Customer : BaseEntity
    {
        public string Name { get; private set; }
        public string Email { get; private set; }
        public string Phone { get; private set; }

        // For EF
        protected Customer() { }

        public Customer(string name, string email, string phone)
        {
            Name = name;
            Email = email;
            Phone = phone;
        }
    }
}