using Chess.Game;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class ChessGame : MonoBehaviour
{
    [SerializeField] private Chess.Game.ChessGameManager chessManager;
    [SerializeField] private TMP_Text tmpText;

    public UnityEvent<string, GameResult.Result> onChessGameDone;

    private void Awake()
    {
        chessManager.onMovePiece.AddListener(UpdateViewMove);
        chessManager.onEndGame.AddListener(OnDonePlayChess);
    }

    public void PlayChess()
    {
        gameObject.SetActive(true);
    }

    public void StopPlayChess()
    {
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
