namespace Sample
{
    public static class SampleSettings
    {
        public static bool CanHover { get; private set; } = true;

        public static void EnableHover()
            => CanHover = true;

        public static void DisableHover()
            => CanHover = false;
    }
}