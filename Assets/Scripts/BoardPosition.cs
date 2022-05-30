using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BoardPosition : MonoBehaviour
{
    private BoardManager boardManager;

    private Position boardPosition;
    public bool isHighlighted { get; private set; }
    public bool isCaptureSpot { get; private set; }
    public void HighlightTile(bool yes)
    {
        isHighlighted = yes;
    }
    public void MarkCaptureTile(bool yes)
    {
        isCaptureSpot = yes;
    }
    public bool isOccupied { get; private set; } = false;
    public PieceColor occupantColor { get; private set; } = PieceColor.Empty;
    public PieceController pieceOccupant { get; private set; }
    public PieceController previousPieceOccupant { get; private set; }

    public void OccupyBoardTile(PieceColor _occupanntColor, PieceController _pieceController)
    {
        isOccupied = true;
        occupantColor = _occupanntColor;
        pieceOccupant = _pieceController;
    }
    public void DeOccupyBoardTile()
    {
        isOccupied = false;
        occupantColor = PieceColor.Empty;
        pieceOccupant = null;
    }


    private Renderer rend;


    private void Awake()
    {
        rend = GetComponent<Renderer>();
        boardManager = FindObjectOfType<BoardManager>();
    }

    public void SetBoardPositionColor(Color color)
    {
        rend.material.color = color;
    }
    public Color GetBoardPositionColor()
    {
        return rend.material.color;
    }

    public BoardPosition GetBoardPositionController()
    {
        return this;
    }

    public void SetPosition(int x, int y)
    {
        char c = ' ';
        switch (x)
        {

            case 1:
                c = 'a';
                break;
            case 2: 
                c = 'b';
                break;
            case 3:
                c = 'c';
                break;
            case 4:
                c = 'd';
                break;
            case 5:
                c = 'e';
                break;
            case 6:
                c = 'f';
                break;
            case 7:
                c = 'g';
                break;
            case 8:
                c = 'h';
                break;
            default:
                break;

        }
        boardPosition = new Position(c, y);
    }
    public Position GetBoardPosition()
    {
        return this.boardPosition;
    }
    public string GetBoardPositionString()
    {
        string s = ("Board Pos " + boardPosition.xPos.ToString() + boardPosition.yPos.ToString());
        return s;
    }

    private void OnMouseDown()
    {
        Debug.Log("Tile was clicked");
        if (isHighlighted) 
        {
            if (isOccupied)
            {
                previousPieceOccupant = pieceOccupant;
            }
            isOccupied = true;
            
            boardManager.selectedTileCtrl.pieceOccupant.GetComponent<PieceController>().MovePiece(boardPosition, isCaptureSpot);
            boardManager.DeHighlightTiles();
            boardManager.DeSelectTile();
        }
    }
}

[System.Serializable]
public class Position
{
    public char xPos;
    public int yPos;

    public Position(char _xPos, int _yPos)
    {
        xPos = _xPos;
        yPos = _yPos;
    }
    public Position(string xy)
    {
        xPos = xy[0];
        yPos = xy[1] - 48; //convert the char to int by removing 48 i.e. ASCII
    }
    public Position (int _x, int _y)
    {
        xPos = BoardManager.PosIntToChar(_x);
        yPos = _y;
    }
}
