using System.Text.Json.Serialization;

namespace Wine_cellar.Entities
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ErrorCode
    {
        UnLogError,
        WineNotFound,
        NoSpaceError,
        AppelationError,
        DrawerNotFound,
        AppelationNotFound,
        NotAdminError,
        AppelationAlreadyExists,
        CellarNotFound,
        CellarAlreadyExists,
        LoginError,
        CGUError,
        InvalidPassword,
        InvalidEmail,
        MinorError,
        EmaiAlreadyExists,
        UserNotFound,
        DeleteUserError
    }
}
