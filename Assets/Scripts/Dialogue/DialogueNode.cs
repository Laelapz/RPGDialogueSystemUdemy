using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


namespace RPG.Dialogue
{
    [System.Serializable]
    public class DialogueNode : ScriptableObject
    {

        [SerializeField] private bool isPlayerSpeaking = false;
        [SerializeField] private string dialogueText;
        [SerializeField] private List<string> childrenNode = new();
        [SerializeField] private Rect rect = new(0, 0, 200, 100);

        [SerializeField] string onEnterAction;
        [SerializeField] string onExitAction;

        public string GetDialogueText()
        {
            return dialogueText;
        }

        public List<string> GetChildrenNode()
        {
            return childrenNode;
        }

        public bool IsPlayerSpeaking()
        {
            return isPlayerSpeaking;
        }

        public string GetOnEnterAction()
        {
            return onEnterAction;
        }

        public string GetOnExitAction()
        {
            return onExitAction;
        }

        public Rect GetRect()
        {
            return rect;
        }

#if UNITY_EDITOR
        public void SetPosition(Vector2 value)
        {
            Undo.RecordObject(this, "Move Dialogue Node");
            rect.position = value;
            EditorUtility.SetDirty(this);
        }

        public void SetIsPlayerSpeaking(bool value)
        {
            Undo.RecordObject(this, "Change Dialogue IsPlayerSpeaking");
            isPlayerSpeaking = value;
            EditorUtility.SetDirty(this);
        }

        public void SetDialogueText(string value)
        {
            Undo.RecordObject(this, "Change Dialogue Text");
            dialogueText = value;
            EditorUtility.SetDirty(this);
        }

        public void AddChild(string childToAdd)
        {
            Undo.RecordObject(this, "Add New Child");
            childrenNode.Add(childToAdd);
            EditorUtility.SetDirty(this);
        }

        public void RemoveChild(string childToRemove)
        {
            Undo.RecordObject(this, "Remove Child");
            childrenNode.Remove(childToRemove);
            EditorUtility.SetDirty(this);
        }

        
#endif

    }
}
