using System.Collections;

/// <summary>
/// For things that provide some kind of yieldable animation
/// </summary>
public interface IAnimation : IMonobehavior
{
    public IEnumerator Animate (object settings = null)
    {
        yield break;
    }
}