namespace R.Systems.Shared.WebApi.Jwt;

public class JwtVerificationSettings
{
    public const string PropertyName = "Jwt";

    public string PublicKeyPemFilePath { get; init; } = "";
}
