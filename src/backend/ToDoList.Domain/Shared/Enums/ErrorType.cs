

namespace ToDoList.Domain.Shared.Enums
{
    public enum ErrorType
    {
        None = 0,
        Failure = 500,
        BadRequest = 400,
        Forbidden = 403,
        NotFound = 404,
        Conflict = 409
    }
}
