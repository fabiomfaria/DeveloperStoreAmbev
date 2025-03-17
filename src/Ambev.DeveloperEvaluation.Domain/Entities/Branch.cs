using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Entities
{
    public class Branch : BaseEntity
    {
        public string Name { get; private set; }
        public string Location { get; private set; }

        // For EF
        protected Branch() { }

        public Branch(string name, string location)
        {
            Name = name;
            Location = location;
        }
    }
}