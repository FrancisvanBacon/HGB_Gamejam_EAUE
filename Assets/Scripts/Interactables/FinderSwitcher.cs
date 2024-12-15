using System;
using System.Collections.Generic;
using Actors.Player;
using UnityEngine;

namespace Interactables {
    public class FinderSwitcher : MonoBehaviour {

        [SerializeField] private List<FinderWrapper> finders;
        public InteractableFinder CurrentFinder;

        public void SwitchFinders(ClassType classType) {
            
            foreach (var finder in finders) {
                if (finder.ClassType.Contains(classType)) {
                    finder.Finder.enabled = true;
                    CurrentFinder = finder.Finder;
                    continue;
                }
                finder.Finder.enabled = false;
            }
            
        }

    }

    [Serializable]
    public class FinderWrapper {
        public List<ClassType> ClassType;
        public InteractableFinder Finder;
    }
}