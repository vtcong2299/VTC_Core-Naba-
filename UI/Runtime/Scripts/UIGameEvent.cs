using Vtcong.EventManager;
using UnityEngine;

namespace Vtcong.Core
{
    public class UIBackgroundShowEvent : GameEvent
    {
        public UIElement uiElement;
        public bool instantAction;
    }

    public class UIBackgroundHideEvent : GameEvent
    {
        public UIElement uiElement;
        public bool instantAction;
    }
}