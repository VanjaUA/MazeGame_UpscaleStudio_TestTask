using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DoorController : MonoBehaviour
{
    [SerializeField] private TextMeshPro keyText;
    [SerializeField] private Animator animator;

    [SerializeField] private int keysToOpen;
    private int keys = 0;

    [SerializeField] private AudioSource doorOpens;

    private const string OPEN_DOOR_TRIGGER = "OpenDoor";

    private void Start()
    {
        UpdateKeyText();
    }

    private void UpdateKeyText()
    {
        keyText.text = keys + "/" + keysToOpen;
    }

    public void TryOpenDoor(int keysAmount) 
    {
        keys = keysAmount;
        UpdateKeyText();
        if (keys >= keysToOpen)
        {
            animator.SetTrigger(OPEN_DOOR_TRIGGER);
            doorOpens.Play();
        }
    }
}
