public interface IServiceSetupContext { }

public interface IService
{
    public void Setup (IServiceSetupContext setupContext = null) { }
}