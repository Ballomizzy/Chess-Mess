using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    [SerializeField]
    private Text winnerText;


    public void DisplayWinner(PieceColor pieceColor)
    {
        winnerText.color = Color.green;
        switch (pieceColor)
        {
            case PieceColor.White:
                winnerText.text = "White Wins!";
                break;
            case PieceColor.Black:
                winnerText.text = "Black Wins!";
                break;
            default:
                break;
        }
    }

    public void DisplayStaleMate()
    {
        winnerText.color = Color.yellow;
        winnerText.text = "Stale Mate";
    }
}
