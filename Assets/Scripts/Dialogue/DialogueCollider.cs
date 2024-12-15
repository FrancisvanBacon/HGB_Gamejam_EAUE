using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Actors.Player;
using UnityEngine;
using Yarn.Unity;

public class DialogueCollider : MonoBehaviour
{
    [SerializeField] private ClassType[] _triggerOnClasses;
    [SerializeField] private bool _isRetriggerable;
    [SerializeField] private String _yarnTitle;

    private LayerMask _mask;
    private List<GameObject> _objectsInsideTrigger = new List<GameObject>();
    private DialogueRunner _dialogueRunner;
    private GameObject _textBox;

    private bool _isTriggered;

    private void Start()
    {
        _mask = LayerMask.GetMask("PlayerActors");
        if(_triggerOnClasses.Length == 0) throw new Exception("TriggerOnClasses can't be null: " + gameObject.name);
        _dialogueRunner = FindFirstObjectByType<DialogueRunner>();
        _textBox = GameObject.Find("Textbox");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if ((1 << other.gameObject.layer & _mask) != 0)
        {
            if (!_objectsInsideTrigger.Contains(other.gameObject))
            {
                _objectsInsideTrigger.Add(other.gameObject);

                if (_isRetriggerable)
                {
                    if (IsAllTriggerOnClasses())
                    {
                        ShowDialogue();
                    }
                }
                else
                {
                    if (!_isTriggered && IsAllTriggerOnClasses())
                    {
                        _isTriggered = true;
                        ShowDialogue();                        
                    }
                }
            }
        }
    }

    private void ShowDialogue()
    {
        if(_dialogueRunner.CurrentNodeName == _yarnTitle) return;
        _dialogueRunner.StartDialogue(_yarnTitle);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (_objectsInsideTrigger.Contains(other.gameObject))
        {
            _objectsInsideTrigger.Remove(other.gameObject);
        }
    }

    private bool IsAllTriggerOnClasses()
    {
        
        if(_triggerOnClasses.Contains(ClassType.None)) return true;
        
        foreach (var classType in _triggerOnClasses)
        {
            bool found = false;
            foreach (var gameObjectInTrigger in _objectsInsideTrigger)
            {
                var characterStateController = gameObjectInTrigger.GetComponent<CharacterStateController>();
                if (characterStateController != null && characterStateController.ClassType == classType)
                {
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                return false;
            }
        }

        return true;
    }
}