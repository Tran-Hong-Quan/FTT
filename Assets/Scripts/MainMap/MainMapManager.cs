using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMapManager : MonoBehaviour
{
    public CharacterController Ngan;
    public CharacterController Minh;
    public CharacterController Mai;
    public CharacterController Nam;
    public CharacterController Hung;
    public PlayerController player;

    [SerializeField] private ParticleSystem[] effs;
    [SerializeField] private StartCamping startCamping;
    [SerializeField] private CampingDay2 campingDay2;
    [SerializeField] private CampingDay3 campingDay3;

    private void Start()
    {
        InventoryManager.instance.AddItemToInventory(ItemType.NormalItem, "Phone", 1, player.inventory);
        Debug.Log("Progress: " + (GameProgress)PlayerPrefs.GetInt("Progress"));
        switch ((GameProgress)PlayerPrefs.GetInt("Progress"))
        {
            case GameProgress.StartCamping:
                InitStartCamping();
                break;
            case GameProgress.CampingDay2:
                InitCampingDay2();
                break;
            case GameProgress.CampingDay3:
                InitCampingDay3();
                break;
        }
    }

    public void AddConversationToCharacter(CharacterController character, Dialogue dialogue, Action onDone = null)
    {
        var onInteract = character.interact.onInteract;
        character.interact.canInteract = true;
        onInteract.RemoveAllListeners();
        onInteract.AddListener(TalkWithCharacter);

        void TalkWithCharacter(InteractableEntity entity)
        {
            DisablePlayerMoveAndUI();
            GameManager.instance.dialogueManager.StartDialogue(dialogue, DoneTalkWithCharacter);

            void DoneTalkWithCharacter()
            {
                EnablePlayerMoveAndUI();
                player.ShowInteractButton();
            }

            onDone?.Invoke();
        }
    }
    public void AddConversationToCharacter(CharacterController character, Dialogue[] dialogue, Action onDone = null)
    {
        var onInteract = character.interact.onInteract;
        character.interact.canInteract = true;
        onInteract.RemoveAllListeners();
        onInteract.AddListener(TalkWithCharacter);

        void TalkWithCharacter(InteractableEntity entity)
        {
            DisablePlayerMoveAndUI();
            GameManager.instance.dialogueManager.StartDialogue(dialogue, DoneTalkWithCharacter);

            void DoneTalkWithCharacter()
            {
                EnablePlayerMoveAndUI();
                player.ShowInteractButton();
            }

            onDone?.Invoke();
        }
    }

    public void PlayParticalEffect(int index, Vector3 pos)
    {
        effs[index].transform.position = pos;
        effs[index].Play();
        GameManager.instance.soundManager.PlayCommondSound("Get Item");
    }

    public void DisablePlayerMoveAndUI()
    {
        player.DisableMove();
        player.HideUI();
    }

    public void EnablePlayerMoveAndUI()
    {
        player.EnableMove();
        player.ShowUI();
    }

    public void InitStartCamping()
    {
        startCamping.Init(Ngan, Minh, Mai, Nam, Hung, player, this);
    }

    public void InitCampingDay2()
    {
        campingDay2.Init(this);
    }

    public void InitCampingDay3()
    {
        campingDay3.Init();
    }
}
