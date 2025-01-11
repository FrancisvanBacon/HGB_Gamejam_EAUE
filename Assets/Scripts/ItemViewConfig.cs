using System;
using Items;
using JetBrains.Annotations;
using UnityEngine;

[CreateAssetMenu(menuName = "Configs/ItemViewConfig", fileName = "ItemViewConfig")]
public class ItemViewConfig : ScriptableObject {
        
        [Serializable]
        private class ItemView {
                public ItemType Item;
                public Sprite AssignedSprite;
        }

        [SerializeField] private ItemView[] itemViews;
                
        [CanBeNull]
        public Sprite GetItemSprite(ItemType itemType) {

                foreach (var view in itemViews) {

                        if (view.Item.Equals(itemType)) {
                                return view.AssignedSprite;
                        }
                        
                }

                return null;
        }

}