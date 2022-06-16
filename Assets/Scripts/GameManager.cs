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

    public void CheckMate(PieceColor pieceThatLost)
    {
        switch (pieceThatLost)
        {
            case PieceColor.White:
                gameUI.DisplayWinner(PieceColor.Black);
                break;
            case PieceColor.Black:
                gameUI.DisplayWinner(PieceColor.White);
                break;
            default:
                break;
        }
    }
    public void StaleMate()
    {
        gameUI.DisplayStaleMate();
    }

    private void Awake()
    {        
        startColor = gameSettings.firstMoveColor;
        currentColorTurn = startColor;
    }
}
