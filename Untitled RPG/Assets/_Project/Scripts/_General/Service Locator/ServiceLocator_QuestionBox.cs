using UnityEngine;

public static partial class ServiceLocator
{
    private static QuestionBox questionBox;

    public static QuestionBox QuestionBox
    {
        get
        {
            if (questionBox != null)
                return questionBox;

            questionBox = Object.FindObjectOfType<QuestionBox>();
            Object.DontDestroyOnLoad(questionBox);

            return questionBox;
        }
        set => questionBox = value;
    }
}