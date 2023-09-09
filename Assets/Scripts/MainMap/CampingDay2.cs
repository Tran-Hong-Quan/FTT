using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampingDay2 : MonoBehaviour
{
    [SerializeField] Vector3 HungStartPos;
    [SerializeField] Vector3 MaiStartPos;
    [SerializeField] Vector3 NganStartPos;
    [SerializeField] Vector3 MinhStartPos;
    [SerializeField] Vector3 NamStartPos;
    [SerializeField] Vector3 playerStartPos;

    [SerializeField] Transform firstMissionPoint;
    [SerializeField] Transform carParkMissionPoint;

    private CharacterController Minh;
    private CharacterController Nam;
    private CharacterController Hung;
    private CharacterController Ngan;
    private CharacterController Mai;
    private PlayerController player;

    private MainMapManager mainMapManager;
    private MainMapDay2Texts texts;
    public void Init(MainMapManager mainMapManager)
    {
        texts = Resources.Load<MainMapDay2Texts>($"Texts/MainMap/Day2/{PlayerPrefs.GetString("Language", "Eng")}");

        this.mainMapManager = mainMapManager;

        Minh = mainMapManager.Minh;
        Nam = mainMapManager.Nam;
        Hung = mainMapManager.Hung;
        Ngan = mainMapManager.Ngan;
        Mai = mainMapManager.Mai;
        player = mainMapManager.player;

        player.DisableMoveAndUI();
        Invoke(nameof(ShowFirstMission), 1);
        SetStartDay2Character();
    }

    private void ShowFirstMission()
    {
        player.SetArrowPointer(firstMissionPoint);
        GameManager.instance.textBoard.ShowText(texts.firstMission, player.EnableMoveAndUI);
    }

    private void SetStartDay2Character()
    {
        StartCoroutine(FirstMissionCheck());

        SetCharacterPos(Minh, MinhStartPos, Vector2.up);
        SetCharacterPos(Ngan, NganStartPos, Vector2.right);
        SetCharacterPos(Hung, HungStartPos, Vector2.up);
        SetCharacterPos(Nam, NamStartPos, Vector2.down);
        SetCharacterPos(player, playerStartPos, Vector2.down);
        //SetCharacterPos(Mai, MaiStartPos, Vector2.left);
        Mai.gameObject.SetActive(false);

        Minh.AddConversationToCharacter(texts.MinhFirstConversation);
        Nam.AddConversationToCharacter(texts.NamFirstConversation);
        Hung.AddConversationToCharacter(texts.HungFirstConversation);
        Minh.AddConversationToCharacter(texts.MinhFirstConversation);
    }

    private IEnumerator FirstMissionCheck()
    {
        while (true)
        {
            yield return null;

            if (((Vector2)firstMissionPoint.position - (Vector2)player.transform.position).sqrMagnitude < 2f)
            {
                MonologueAboutFirstMissionAndCallPlayerBack();
                yield break;
            }
        }
    }

    private void MonologueAboutFirstMissionAndCallPlayerBack()
    {
        player.DisableMoveAndUI();
        player.OffArrowPointer();
        player.anim.SetMove(false);

        Mai.gameObject.SetActive(true);
        Mai.transform.position = player.transform.position + new Vector3(1, -1) * 10;

        GameManager.instance.dialogueManager.StartDialogue(texts.playerMonologueAboutFirstMisson, MaiApproachPlayer);

        void MaiApproachPlayer()
        {
            Mai.UpdateMoveAnimation();
            Mai.transform.DOMove(player.transform.position + Vector3.right, 5).OnComplete(MaiCallPlayerToMovoToCarPark);
        }

        void MaiCallPlayerToMovoToCarPark()
        {
            player.anim.SetDirection(Vector2.right);
            Mai.StopMove();
            Mai.anim.SetDirection(Vector2.left);
            GameManager.instance.dialogueManager.StartDialogue(texts.callPlayerBackToCar, MaiMoveOut);
        }

        void MaiMoveOut()
        {
            Mai.UpdateMoveAnimation();
            Mai.transform.DOMove(player.transform.position + new Vector3(1, -1) * 10, 5).OnComplete(InitToCarPark);
        }

        void InitToCarPark()
        {
            Mai.gameObject.SetActive(false);
            player.EnableMoveAndUI();
            player.SetArrowPointer(carParkMissionPoint);
            StartCoroutine(CheckMoveToCarParkMission());
        }
    }

    private IEnumerator CheckMoveToCarParkMission()
    {
        while (true)
        {
            if (((Vector2)carParkMissionPoint.position - (Vector2)player.transform.position).sqrMagnitude < 2f)
            {
                ConfessToPlayer();
                yield break;
            }
            yield return null;
        }
    }

    private void ConfessToPlayer()
    {
        player.DisableMoveAndUI();
        GameManager.instance.transitions.Transition(1, 1, StartDialogueConfess, MoveCharaceterToCarPark);

        void MoveCharaceterToCarPark()
        {
            Mai.gameObject.SetActive(true);

            Vector3 playerPos = player.transform.position;
            Nam.transform.position = playerPos + Vector3.right;
            Hung.transform.position = playerPos + Vector3.down + Vector3.left;
            Mai.transform.position = playerPos + Vector3.down + Vector3.right * 2;
            Ngan.transform.position = playerPos + Vector3.down * 2;
            Minh.transform.position = playerPos + Vector3.down * 2 + Vector3.left;

            player.anim.SetDirection(Vector2.down);
            Nam.anim.SetDirection(Vector2.down);
            Mai.anim.SetDirection(Vector2.left);
            Hung.anim.SetDirection(Vector2.right);
            Ngan.anim.SetDirection(Vector2.up);
            Minh.anim.SetDirection(Vector2.up);
        }

        void StartDialogueConfess()
        {
            GameManager.instance.dialogueManager.StartDialogue(texts.confessTheTruth, StartMission3);
        }
    }

    private void SetCharacterPos(CharacterController character, Vector3 pos, Vector2 dir)
    {
        character.transform.position = pos;
        character.anim.SetDirection(dir);
    }

    private void StartMission3()
    {
        Debug.Log("Start Misssion 3");
    }
}
