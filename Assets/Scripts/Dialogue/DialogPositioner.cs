using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Yarn.Unity;
using Debug = FMOD.Debug;

public class DialogPositioner : MonoBehaviour
{

   [SerializeField] private Transform _jesterTransform;
   [SerializeField] private Transform _hunterTransform;
   [SerializeField] private Transform _warriorTransform;
   [SerializeField] private Transform _narratorTransform;
   
   private DialogueRunner _dialogueRunner;
   private TMP_Text _characterNameTMP;
   
   private Vector3 offset = new Vector3(0, 2f, 0);

   private void Start()
   {
      _dialogueRunner = FindFirstObjectByType<DialogueRunner>();
      GameObject characterNameGameObject = GameObject.Find("Character Name");
      _characterNameTMP = characterNameGameObject.GetComponent<TMP_Text>();
   }

   private void Update()
   {
      if (_dialogueRunner.IsDialogueRunning)
      {
         UnityEngine.Debug.Log("Dialogue Running");
         string classType = _characterNameTMP.text;
         UnityEngine.Debug.Log(classType);
         switch (classType) {
            case "Jester":
               transform.position = _jesterTransform.position + offset;
               break;
            case "Hunter":
               transform.position = _hunterTransform.position + offset;
               break;
            case "Warrior":
               transform.position = _warriorTransform.position + offset;
               break;
            case "None":
               transform.position = _narratorTransform.position + offset;
               break;
            default:
               transform.position = _narratorTransform.position + offset;
               break;
         }
      }
   }
}
