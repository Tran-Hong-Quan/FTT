using Chess.Game;
using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ChessGame : MonoBehaviour
{
    [SerializeField] private Chess.Game.ChessGameManager chessManager;
    [SerializeField] private TMP_Text tmpText;
    [SerializeField] private CinemachineVirtualCamera virtualCamera;

    public Button outButton;
    public UnityEvent<string, GameResult.Result> onChessGameDone;

    private void Awake()
    {
        chessManager.onMovePiece.AddListener(UpdateViewMove);
        chessManager.onEndGame.AddListener(OnDonePlayChess);
    }

    Transform baseFollow;
    public void PlayChess()
    {
        gameObject.SetActive(true);
        baseFollow = virtualCamera.Follow;
        virtualCamera.Follow = null;
        virtualCamera.transform.position = transform.position - Vector3.forward * 10;
    }

    public void StopPlayChess()
    {
        virtualCamera.Follow = baseFollow;
        gameObject.SetActive(false);
    }

    public void PlayAgain()
    {
        chessManager.NewGame(true);
        tmpText.text = "";
    }

    public void UpdateViewMove(string move)
    {
        move += "\n";
        tmpText.text += move;
    }

    public string GetMoveText()
    {
        return tmpText.text;
    }

    public void OnDonePlayChess(GameResult.Result result)
    {
        onChessGameDone?.Invoke(GetMoveText(),result);
    }
}
