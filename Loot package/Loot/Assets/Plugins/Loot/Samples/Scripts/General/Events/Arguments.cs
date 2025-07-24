using System;
using System.Collections.Generic;

namespace Sample
{
    public class UpdateScrollViewArguments : EventArgs
    {
        public List<HUDScrollVIewItem> ItemsToShow;
    }

    public class UpdateHeaderArguments : EventArgs
    {
        public HUDHeader Header;
    }
}