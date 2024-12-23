using System;
using System.Collections.Generic;
using Actors.Player;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Dialogue {

    [CreateAssetMenu(menuName = "Configs/DialogueBubbleSettings", fileName = "DialogueBubbleSettings")]
    public class BubbleViewConfig : ScriptableObject {

        [SerializeField] private List<ClassDialogueSettings> settings;


        public ClassDialogueSettings GetSettings(ClassType classType) {

            foreach (var setting in settings) {

                if (setting.Class.Equals(classType)) {
                    return setting;
                }
                
            }

            return null;
        } 
    }

    [Serializable]
    public class ClassDialogueSettings {

        public ClassType Class;
        public Sprite BorderSprite;
        public Sprite FillSprite;
        public Color FillColour;
        [FormerlySerializedAs("font")] public TMP_FontAsset Font;

    }
} 