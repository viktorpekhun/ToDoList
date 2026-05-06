
using ToDoList.Domain.Shared.Enums;

namespace ToDoList.Domain.Shared
{
    public record Error(string Code, string Message, ErrorType Type)
    {
        public static readonly Error None = new(string.Empty, string.Empty, ErrorType.None);

        public static Error Failure(string code, string message) =>
            new(code, message, ErrorType.Failure);

        public static Error BadRequest(string code, string message) =>
            new(code, message, ErrorType.BadRequest);

        public static Error Validation(string code, string message) =>
            new(code, message, ErrorType.BadRequest);

        public static Error Forbidden(string code, string message) =>
            new(code, message, ErrorType.Forbidden);

        public static Error NotFound(string code, string message) =>
            new(code, message, ErrorType.NotFound);

        public static Error Conflict(string code, string message) =>
            new(code, message, ErrorType.Conflict);


    }
}
