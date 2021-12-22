namespace R.Systems.Shared.Core.Interfaces;

public interface IRsaKeys
{
    public string? PublicKey { get; init; }

    public string? PrivateKey { get; init; }
}
