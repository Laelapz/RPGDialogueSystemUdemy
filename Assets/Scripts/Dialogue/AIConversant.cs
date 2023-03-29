using RPG.Control;
using RPG.Dialogue;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIConversant : MonoBehaviour, IRaycastable
{
    [SerializeField] private string _NPCName;
    [SerializeField] private Dialogue _NPCDialogue;

    public CursorType GetCursorType()
    {
        return CursorType.Dialogue;
    }

    public bool HandleRaycast(PlayerController callingController)
    {
        if (_NPCDialogue == null) return false;

        if (Input.GetMouseButtonDown(0))
        {
            PlayerConversant playerConversant = callingController.GetComponent<PlayerConversant>();
            playerConversant.StartDialogue(this, _NPCDialogue);
        }
        return true;
    }

    public string GetConversantName()
    {
        return _NPCName;
    }
}
