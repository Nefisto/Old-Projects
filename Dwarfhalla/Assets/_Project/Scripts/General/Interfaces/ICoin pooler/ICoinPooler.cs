public interface ICoinPooler
{
    public IAnimation GetNewCoin() => new NullAnimation();
}

public class NullCoinPooler : ICoinPooler { }