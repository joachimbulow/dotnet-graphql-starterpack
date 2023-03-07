public class StateFixture : IDisposable
{
    public static string CommonPassword = "TestPassword123!";

    // Strings
    private string name = "John Doe";
    private int _nameCounter = 0;

    // Email
    private string emailPrefix = "test";
    private string emailSuffix = "@test.com";

    private int _emailCounter = 0;

    public StateFixture()
    {
    }

    public string nextName()
    {
        return "John Doe" + _nameCounter++;
    }

    public string nextEmail()
    {
        return emailPrefix + _emailCounter++ + emailSuffix;
    }

    public string nextPassword()
    {
        return StateFixture.CommonPassword;
    }


    public void Dispose()
    {
        // ... clean up test data from the database ...
    }

}