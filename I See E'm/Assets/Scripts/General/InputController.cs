public static class InputController
{
    private static GameInputs input;

    public static GameInputs Input
    {
        get
        {
            if (input == null)
            {
                input = new GameInputs();
                input.Enable();
            }

            return input;
        }
    }
}