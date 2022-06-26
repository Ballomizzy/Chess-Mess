using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameSettings gameSettings;
    [SerializeField]
    private GameUI gameUI;
    public PieceColor currentColorTurn { get; private set; }
    private PieceColor startColor;

    [SerializeField]
    private ColorPiecesManager whiteManager, blackManager;

    private MoveLogsManager moveLogsManager;
    public void SwitchTurn()
    {
        switch (currentColorTurn)
        {
            case PieceColor.White:
                currentColorTurn = PieceColor.Black;
                blackManager.CheckForStaleMate();
                blackManager.MakeChildrenSelectable(true);
                whiteManager.MakeChildrenSelectable(false);
                break;
            case PieceColor.Black:
                currentColorTurn = PieceColor.White;
                whiteManager.CheckForStaleMate();
                whiteManager.MakeChildrenSelectable(true);
                blackManager.MakeChildrenSelectable(false);
                break;
            default:
                break;
        }
    }

    public void MakeAllPiecesUnSelectable(bool yes)
    {
        if (yes)
        {
            blackManager.MakeChildrenSelectable(false);
            whiteManager.MakeChildrenSelectable(false);
        }
        else
        {
            switch (currentColorTurn)
            {
                case PieceColor.White:
                    whiteManager.MakeChildrenSelectable(true);
                    blackManager.MakeChildrenSelectable(false);
                    break;
                case PieceColor.Black:
                    blackManager.MakeChildrenSelectable(true);
                    whiteManager.MakeChildrenSelectable(false); 
                   break;
                default:
                    break;
            }
        }

    }

    public void CheckMate(PieceColor pieceThatLost)
    {
        switch (pieceThatLost)
        {
            case PieceColor.White:
                gameUI.DisplayWinner(PieceColor.Black);
                MoveLogsManager.MoveLogInfo lastInfo = moveLogsManager.lastInfo;
                MoveLogsManager.MoveLogInfo info = new MoveLogsManager.MoveLogInfo(lastInfo.piece, lastInfo.oldPos, lastInfo.newPos, lastInfo.isCheck, true, lastInfo.isCapture, lastInfo.isPromote, lastInfo.promotePiece, lastInfo.isKingSideCastle, lastInfo.isQueenSideCastle);
                moveLogsManager.lastEdittedLogText.text = moveLogsManager.MoveLogText(info);
                break;
            case PieceColor.Black:
                gameUI.DisplayWinner(PieceColor.White);
                MoveLogsManager.MoveLogInfo lastInfo1 = moveLogsManager.lastInfo;
                MoveLogsManager.MoveLogInfo info1 = new MoveLogsManager.MoveLogInfo(lastInfo1.piece, lastInfo1.oldPos, lastInfo1.newPos, lastInfo1.isCheck, true, lastInfo1.isCapture, lastInfo1.isPromote, lastInfo1.promotePiece, lastInfo1.isKingSideCastle, lastInfo1.isQueenSideCastle);
                moveLogsManager.lastEdittedLogText.text = moveLogsManager.MoveLogText(info1);
                break;
            default:
                break;
        }
    }
    public void StaleMate()
    {
        gameUI.DisplayStaleMate();
    }

    private void Start()
    {        
        startColor = gameSettings.firstMoveColor;
        currentColorTurn = startColor;
        moveLogsManager = FindObjectOfType<MoveLogsManager>();
    }
}
