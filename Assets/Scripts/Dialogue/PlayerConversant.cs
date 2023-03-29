using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace RPG.Dialogue
{
    public class PlayerConversant : MonoBehaviour
    {
        private Dialogue currentDialogue;
        private DialogueNode _currentNode = null;
        private bool isChoosing = false;
        private AIConversant _currentConversant = null;
        [SerializeField] private string _playerName;

        public event Action onConversationUpdated;

        public void StartDialogue(AIConversant newConversant, Dialogue newDialogue)
        {
            _currentConversant = newConversant;
            currentDialogue = newDialogue;
            _currentNode = currentDialogue.GetRootNode();
            TriggetEnterAction();
            onConversationUpdated();

        }

        public void Quit()
        {
            currentDialogue = null;
            TriggetExitAction();
            _currentNode = null;
            isChoosing = false;
            _currentConversant = null;
            onConversationUpdated();
        }

        public bool IsActive()
        {
            return currentDialogue != null;
        }

        public bool IsChoosing()
        {
            return isChoosing;
        }

        public string GetText()
        {
            if (_currentNode == null) return "";

            return _currentNode.GetDialogueText();
        }

        public IEnumerable<DialogueNode> GetChoices()
        {
            foreach(DialogueNode choice in currentDialogue.GetPlayerChildren(_currentNode))
            {
                yield return choice;
            }
        }

        public void SelectChoice(DialogueNode choosenNode)
        {
            _currentNode = choosenNode;
            TriggetEnterAction();
            isChoosing = false;
            Next();
        }

        public void Next()
        {
            int numPlayerResponses = currentDialogue.GetPlayerChildren(_currentNode).Count();
            if(numPlayerResponses > 0)
            {
                isChoosing = true;
                TriggetExitAction();
                onConversationUpdated.Invoke();
                return;
            }

            DialogueNode[] children = currentDialogue.GetAIChildren(_currentNode).ToArray();
            TriggetExitAction();
            _currentNode = children[Random.Range(0, children.Length)];
            TriggetEnterAction();

            onConversationUpdated.Invoke();
        }

        public bool HasNext()
        {
            return _currentNode.GetChildrenNode().Count > 0;
        }

        private void TriggetEnterAction()
        {
            if (_currentNode == null || _currentNode.GetOnEnterAction() == "") return;

            TriggerAction(_currentNode.GetOnEnterAction());
        }

        private void TriggetExitAction()
        {

            if (_currentNode == null || _currentNode.GetOnExitAction() == "") return;

            TriggerAction(_currentNode.GetOnExitAction());
        }

        private void TriggerAction(String action)
        {
            if (action == "") return;

            foreach (DialogueTrigger trigger in _currentConversant.GetComponents<DialogueTrigger>())
            {
                trigger.Trigger(action);
            }
        }

        public string GetCurrentConversantName()
        {
            if (isChoosing || _currentNode.IsPlayerSpeaking()) return _playerName;
            return _currentConversant.GetConversantName();
        }
    }

}