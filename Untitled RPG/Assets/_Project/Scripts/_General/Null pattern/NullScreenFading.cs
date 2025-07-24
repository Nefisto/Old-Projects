using System.Collections;

public class NullScreenFading : IScreenFading
{
    public IEnumerator FadeOut (IScreenFading.Settings settings = null)
    {
        yield break;
    }

    public IEnumerator FadeIn (IScreenFading.Settings settings = null)
    {
        yield break;
    }
}