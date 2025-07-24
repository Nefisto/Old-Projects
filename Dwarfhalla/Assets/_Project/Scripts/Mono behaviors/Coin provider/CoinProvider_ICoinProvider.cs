public partial class CoinPooler
{
    public IAnimation GetNewCoin()
    {
        var instance = Instantiate(coinPrefab);
        instance.Setup();

        return instance;
    }
}