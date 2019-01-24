using ApplicationCore.Common;

namespace ApplicationCore.Entities
{
    public class RoomType : Entity
    {
        public string Name { get; private set; }

        private RoomType()
        {
        }

        public RoomType(string name)
        {
            Contract.Require(!string.IsNullOrWhiteSpace(name), "Room type name is required");

            Name = name;
        }
    }
}
