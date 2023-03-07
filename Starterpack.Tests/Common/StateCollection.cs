[CollectionDefinition("State collection")]
public class StateCollection : ICollectionFixture<StateFixture>
{
    // This class just creates the shared StateFixture that can be constructor injected in test classes
    // Using [CollectionDefinition("State collection")]
}