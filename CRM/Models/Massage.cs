using Enums;

namespace Models
{
    public sealed class Massage
    {
        public Guid Id { get; set; }
        public Guid IdSender { get; set; }
        public Guid IdRecipient { get; set; }
        public string Name { get; set; }
        public string Theme { get; set; }
        public string Text { get; set; }
        public Roles Role { get; set; }
        public override string ToString() =>
            $"Name: {Name} Theme:{Theme}";
    }
}
