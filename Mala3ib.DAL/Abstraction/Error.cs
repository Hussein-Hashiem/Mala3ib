using Mala3ib.DAL.Enums;

namespace Mala3ib.DAL.Abstraction
{
    public record Error(string Code, string Description, ErrorType? StatusCode)
    {
        public static readonly Error None = new Error(string.Empty,string.Empty, ErrorType.None);
    }
}
