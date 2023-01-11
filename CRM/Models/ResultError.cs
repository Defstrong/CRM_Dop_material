using Enums;

namespace Models
{
    public sealed class Result<T>
    {
        public string TextError { get; set; }
        public ErrorStatus Error { get; set; }
        public bool IsSuccessfully { get; set; }
        public T ResultOperation { get; set; }

    }
}
