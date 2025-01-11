using System;
using System.Collections.Generic;
using Actors.Player;
using Items;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHudView : MonoBehaviour {

        [Serializable]
        private class ClassView {
                public ClassType Class;
                public Image AssignedRenderer;
        }
        
        [SerializeField] private ClassView[] classViews;

        [SerializeField] private ItemViewConfig config;

        [SerializeField] private CanvasGroup canvas;

        private Dictionary<ClassType, Image> m_lookup = new Dictionary<ClassType, Image>();

        private void Start() {

                foreach (var view in classViews) {
                        m_lookup.TryAdd(view.Class, view.AssignedRenderer);
                }

                if (config == null) {
                        config = Resources.Load<ItemViewConfig>("ItemViewConfig");
                }

                canvas.alpha = 1f;

        }


        public void UpdateView(ClassType type, ItemType itemType) {

                if (m_lookup.TryGetValue(type, out Image image)) {
                        image.sprite = config.GetItemSprite(itemType);
                }

        }
}

