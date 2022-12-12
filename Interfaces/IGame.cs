using Newtonsoft.Json;

public interface IGame
{
    [JsonProperty("id")]
    public Guid Id { get; set; }
}