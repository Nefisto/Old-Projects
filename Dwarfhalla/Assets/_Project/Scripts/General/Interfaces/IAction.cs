using System.Collections;

public interface IAction
{
    public int Cost { get; }

    public bool CanBePerformed();
    public void PreviewExecution();

    public IEnumerator Perform (object context = null);
}