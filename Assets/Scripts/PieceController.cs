using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PieceController : MonoBehaviour
{

    private BoardManager boardManager;
    private PiecesManager piecesManager;
    Position currrentPosition, endPositionPos;
    Vector3 positionToMoveTo, currentPosition;
    private bool isPositionSet, isMoving;


    private Piece pieceClass;
    public void InitPiece(Piece piece)
    {
        pieceClass = piece;
    }

    [Header("Piece Stats")]
    private int moves = 0;
    [SerializeField]
    private float pieceMoveSpeed = 10f;

    public void InitPosition(Position position)
    {
        if (!isPositionSet)
        {
            currrentPosition = position;
            isPositionSet = true;
        }
    }

    public void MovePiece(Position position, bool isCapturingMove)
    {

        positionToMoveTo = boardManager.GetBoardTilePos(position);
        
        BoardPosition newBoardPosition = BoardManager.GetBoardTile(position);
        newBoardPosition.OccupyBoardTile(pieceClass.pieceColor, this);
        
        BoardPosition oldBoardPosition = BoardManager.GetBoardTile(currrentPosition);
        oldBoardPosition.DeOccupyBoardTile();


        isMoving = true;
        endPositionPos = position;
        if (isCapturingMove)
        {
            //Cpature animation
            BoardManager.GetBoardTile(position).previousPieceOccupant.GoToJail();
        }

    }

    private void StopMoving()
    {
        //yield return new WaitForSeconds(1f);
        isMoving = false;
        currrentPosition = endPositionPos;
        endPositionPos = null;
        moves++;
    }
    public void MoveAnimation(Vector3 position)
    {
        transform.position = Vector3.MoveTowards
        (
            transform.position, 
            new Vector3(position.x, transform.position.y, position.z), 
            pieceMoveSpeed * Time.deltaTime
        );
    }
    public void GoToJail()
    {
        switch (pieceClass.pieceColor)
        {
            case PieceColor.White:
                Transform jail = piecesManager.BlackJailGO.transform;
                transform.position = new Vector3(jail.position.x, transform.position.y, transform.position.x);
                transform.SetParent(jail);
                jail.GetComponent<JailManager>().ArrangePieces();
                break;
            case PieceColor.Black:
                Transform jail1 = piecesManager.WhiteJailGO.transform;
                transform.position = new Vector3(jail1.position.x, transform.position.y, transform.position.x);
                transform.SetParent(jail1);
                jail1.GetComponent<JailManager>().ArrangePieces();
                break;
            default:
                break;
        }
    }

    private void Awake()
    {
        boardManager = FindObjectOfType<BoardManager>();
        piecesManager = FindObjectOfType<PiecesManager>();
    }
    private void Start()
    {
        InitPosition(pieceClass.startPosition);
    }

    private void Update()
    {
        pieceClass.piecePosition = currrentPosition;
        pieceClass.moves = moves;

        if (isMoving)
        {
            MoveAnimation(positionToMoveTo);
            if(transform.position == currentPosition)
                StopMoving();
        }
        if (!pieceClass.controller)
        {
            pieceClass.controller = this;
        }
    }

    private void LateUpdate()
    {
        if (isMoving)
        {
            currentPosition = transform.position;
        }
    }
    public Move FindPossibleMoves()
    {
        return pieceClass.Move();
    }


    private void OnMouseDown()
    {
        boardManager.SelectTile(currrentPosition);
        Move moves = FindPossibleMoves();
        boardManager.HighlightTiles(moves.movablePositions, moves.captureMoves);
    }
}


/*
 * 
//Move a piece
1. User Selects the piece \
    a. Spawn Selector under piece but above board \

2. Show the available spots
    a. Check if spot is available
    b. Check if spot would make king on check
    c. if King, Check if new spot would make king on check
    d. Show spots

3. User Selects spot
    a. Piece moves
    b. Highlight previous spot
    c. Highlight new spot
    d. Occupy new Spot and deOccupy old spot

*/

