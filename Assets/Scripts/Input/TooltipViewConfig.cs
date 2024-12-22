using System;
using System.Collections.Generic;
using UnityEngine;

namespace Input {
    [CreateAssetMenu(menuName = "Configs/TooltipSettings", fileName = "TooltipSettings")]
    public class TooltipViewConfig : ScriptableObject {

        [SerializeField] private List<TooltipInputBinding> bindings;
        public List<TooltipInputBinding> Bindings => bindings;

    }

    [Serializable]
    public class TooltipInputBinding {

        public InputType InputType;
        public Sprite KeyboardSprite;
        public Sprite XBoxControllerSprite;

    }
}