﻿using System;

namespace MoreMountains.Feedbacks
{
    [Serializable]
    public class MMF_Button
    {
        public delegate void ButtonMethod();

        public string ButtonText;
        public ButtonMethod TargetMethod;

        public MMF_Button (string buttonText, ButtonMethod method)
        {
            ButtonText = buttonText;
            TargetMethod = method;
        }
    }
}