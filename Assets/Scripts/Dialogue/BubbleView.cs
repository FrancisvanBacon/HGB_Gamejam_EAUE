using System;
using Actors.Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;

namespace Dialogue {
    public class BubbleView : MonoBehaviour {

        [SerializeField] private BubbleViewConfig config;

        [SerializeField] private Image bubbleBorderImage;
        [SerializeField] private Image bubbleFillImage;
        [SerializeField] private TMP_Text textfield;

        private void Start() {
            if (config == null) {
                Resources.Load<BubbleViewConfig>("DialogueBubbleSettings");
            }
        }
        
        [YarnCommand("SwitchBubble")]
        public void SwitchBubble(string characterName) {
            
            ClassType type = ClassType.None;

            if (Enum.TryParse(characterName, out ClassType _type)) {
                type = _type;
            }

            var setting = config.GetSettings(type);

            if (setting != null) {
                UpdateBubble(setting);
            };

        }

        private void UpdateBubble(ClassDialogueSettings setting) {

            bubbleBorderImage.sprite = setting.BorderSprite;
            bubbleFillImage.sprite = setting.FillSprite;
            bubbleFillImage.color = setting.FillColour;
            textfield.font = setting.Font;

        }

    }
}