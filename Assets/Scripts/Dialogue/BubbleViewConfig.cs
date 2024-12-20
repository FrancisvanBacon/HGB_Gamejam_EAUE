using System;
using System.Collections.Generic;
using Actors.Player;
using UnityEngine;

namespace Dialogue {

    [CreateAssetMenu(menuName = "Configs", fileName = "DialogueBubbleSettings")]
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

    }
} 