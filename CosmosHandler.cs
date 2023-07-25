using Humanizer;
using Microsoft.Azure.Cosmos;

public static class CosmosHandler
{ 
    public static async Task ManageCustomerAsync(string name, string email, string state, string country)
    {
        Container container = await GetContainer();
        string id = name.Kebaberize();

        var customer = new {
            id = id,
            name = name,
            address = new {
                state = state,
                country = country
            }
        };

        var response = await container.CreateItemAsync(customer);
        Console.WriteLine($"[{response.StatusCode}]\t{id}\t{response.RequestCharge} RUs");

    }

    private static readonly CosmosClient _client;
    static CosmosHandler()
    {
        _client = new CosmosClient(
            accountEndpoint: "https://4f18f3a4-0ee0-4-231-b9ee.documents.azure.com:443/", 
            authKeyOrResourceToken: "ugR2sPHqFgVPL02fsjRWg5QSrtlNGnKBWQH810w3z6uDsZDz9nUPwCrDwykaoE54jwAs2b36jRtMACDbrHKfQw=="
        );
    }

    private static async Task<Container> GetContainer()
    { 

        Database database = _client.GetDatabase("cosmicworks");
        List<string> keyPaths = new()
        {
            "/address/country",
            "/address/state"
        };

        ContainerProperties properties = new(
            id: "customers",
            partitionKeyPaths: keyPaths
        );

        return await database.CreateContainerIfNotExistsAsync(
            containerProperties: properties
        );
    }

}