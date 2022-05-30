using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Piece
{
    public PiecesType pieceType;
    public PieceColor pieceColor;
    public GameObject pieceGameObject;
    public Position piecePosition, startPosition;
    public PieceController controller;
    public int moves = 0;
    public bool isEnPassantable;

    public void SetValues(PiecesType _piecesType, PieceColor _pieceColor, GameObject _pieceGameObject, Position _startPosition)
    {
        pieceType = _piecesType;
        pieceColor = _pieceColor;
        pieceGameObject = _pieceGameObject;
        startPosition = _startPosition;
    }

    public Move Move()
    {
        switch (pieceType)
        {
            case PiecesType.Pawn:
                return pawnMove();
            case PiecesType.Rook:
                return rookMove();
            case PiecesType.Knight:
                return knightMove();
            case PiecesType.Bishop:
                return bishopMove();
            case PiecesType.Queen:
                return queenMove();
            case PiecesType.King:
                return kingMove();
            default:
                return new Move();
        }
    }

    private Move pawnMove()
    {
        List<Position> maxMoves = new List<Position>();
        List<Position> captureMoves = new List<Position>();
        List<Position> specialMoves = new List<Position>();
        int sign = 0;//Use this to change the forward of the piece if it's the other piece
        switch (pieceColor)
        {
            case PieceColor.White:
                sign = -1;//negative
                break;
            case PieceColor.Black:
                sign = 1;
                break;
            default:
                break;
        }
        // Check for pawn capture spots
        for (int i = 0, x = 1; i < 2; i++)
        {
            Position newPos;
            if(i % 2 == 0)
                newPos = (new Position(BoardManager.PosCharToInt(piecePosition.xPos) + x, piecePosition.yPos + (1 * sign)));
            else
                newPos = (new Position(BoardManager.PosCharToInt(piecePosition.xPos) - x, piecePosition.yPos + (1 * sign)));
            
            //check if spot is on board
            int xPos = BoardManager.PosCharToInt(newPos.xPos),
                yPos = newPos.yPos;

            if (!(xPos < 1 || xPos > 8 || yPos < 1 || yPos > 8))
            {
                captureMoves.Add(newPos);
                //check if spot is occupied by opponent or else remove it
                BoardPosition boardPosition = BoardManager.GetBoardTile(newPos);

                if (!boardPosition.isOccupied)
                    captureMoves.Remove(newPos);

                else if (boardPosition.occupantColor == pieceColor)
                    captureMoves.Remove(newPos);
            }
        }

        //Other movable spots
        if (moves == 0)// first pawn move
        {
            for(int i = 0; i < 2; i++)
            {
                Position newPos = new Position(piecePosition.xPos, piecePosition.yPos + ((i + 1) * sign));

                //check if spot is on board
                int xPos = BoardManager.PosCharToInt(newPos.xPos),
                    yPos = newPos.yPos;

                if (!(xPos < 1 || xPos > 8 || yPos < 1 || yPos > 8))
                {
                    maxMoves.Add(newPos);
                }

                //check if spot is occupied. if so remove it
                BoardPosition boardPosition = BoardManager.GetBoardTile(newPos);
                if ((boardPosition.isOccupied))
                {
                    maxMoves.Remove(newPos);
                }
            }
        }
        else //normal pawn move
        {
            Position newPos = (new Position(piecePosition.xPos, piecePosition.yPos + (1 * sign)));

            //check if spot is on board
            int xPos = BoardManager.PosCharToInt(newPos.xPos),
                yPos = newPos.yPos;

            if (!(xPos < 1 || xPos > 8 || yPos < 1 || yPos > 8))
            {
                maxMoves.Add(newPos);
            }

            //check if spot is occupied. if so, remove it
            BoardPosition boardPosition = BoardManager.GetBoardTile(newPos);
            if ((boardPosition.isOccupied))
            {
                maxMoves.Remove(newPos);
            }

            //Promote pawn
            if(yPos == 1 || yPos == 8)
            {
                maxMoves.Remove(newPos);
                specialMoves.Add(newPos);
            }

        }

        //EnPassant



    

        Move newMove = new Move(maxMoves.ToArray(), captureMoves.ToArray(), specialMoves.ToArray());
        return newMove;
    }

    private Move rookMove()
    {
        List<Position> maxMoves = new List<Position>();
        List<Position> captureMoves = new List<Position>();
        List<Position> specialMoves = new List<Position>();

        bool upEnd, leftEnd, rightEnd, downEnd;
        upEnd = leftEnd = rightEnd = downEnd = false;

        //Check possible moves

        //Check up movement
        for (int i = 0; i < 8; i++)
        {
            //If a spot in it's way is occupied stop checking for spots beyond
            if (upEnd)
                break;
            Position newPos = new Position(piecePosition.xPos, (piecePosition.yPos + (i + 1)));
            Debug.Log(piecePosition.yPos + " + " + i + " + 1 = " + (piecePosition.yPos + (i + 1)));

            //check if spot is on board
            int xPos = BoardManager.PosCharToInt(newPos.xPos),
                yPos = newPos.yPos;

            if (!(xPos < 1 || xPos > 8 || yPos < 1 || yPos > 8))
            {
                maxMoves.Add(newPos);
                //check if spot is occupied. if so remove it
                BoardPosition boardPosition = BoardManager.GetBoardTile(newPos);
                if ((boardPosition.isOccupied))
                {
                    upEnd = true;
                    maxMoves.Remove(newPos);
                    if (boardPosition.occupantColor != pieceColor)
                        captureMoves.Add(newPos);
                }
            }

            
        }
        //Check down movement
        for (int i = 0; i < 8; i++)
        {
            //If a spot in it's way is occupied stop checking for spots beyond
            if (downEnd)
                break;
            Position newPos = new Position(piecePosition.xPos, (piecePosition.yPos - (i + 1)));

            //check if spot is on board
            int xPos = BoardManager.PosCharToInt(newPos.xPos),
                yPos = newPos.yPos;

            if (!(xPos < 1 || xPos > 8 || yPos < 1 || yPos > 8))
            {
                maxMoves.Add(newPos);
                //check if spot is occupied. if so remove it
                BoardPosition boardPosition = BoardManager.GetBoardTile(newPos);
                if ((boardPosition.isOccupied))
                {
                    downEnd = true;
                    maxMoves.Remove(newPos);
                    if (boardPosition.occupantColor != pieceColor)
                        captureMoves.Add(newPos);
                }
            }

            
        }
        //Check left movement
        for (int i = 0; i < 8; i++)
        {
            //If a spot in it's way is occupied stop checking for spots beyond
            if (leftEnd)
                break;
            Position newPos = new Position((BoardManager.PosCharToInt(piecePosition.xPos) - (i + 1)), piecePosition.yPos);

            //check if spot is on board
            int xPos = BoardManager.PosCharToInt(newPos.xPos),
                yPos = newPos.yPos;

            if (!(xPos < 1 || xPos > 8 || yPos < 1 || yPos > 8))
            {
                maxMoves.Add(newPos);
                //check if spot is occupied. if so remove it
                BoardPosition boardPosition = BoardManager.GetBoardTile(newPos);
                if ((boardPosition.isOccupied))
                {
                    leftEnd = true;
                    maxMoves.Remove(newPos);
                    if (boardPosition.occupantColor != pieceColor)
                        captureMoves.Add(newPos);
                }
            }
        }
        //Check right movement
        for (int i = 0; i < 8; i++)
        {
            //If a spot in it's way is occupied stop checking for spots beyond
            if (rightEnd)
                break;
            Position newPos = new Position((BoardManager.PosCharToInt(piecePosition.xPos) + (i + 1)), piecePosition.yPos);

            //check if spot is on board
            int xPos = BoardManager.PosCharToInt(newPos.xPos),
                yPos = newPos.yPos;

            if (!(xPos < 1 || xPos > 8 || yPos < 1 || yPos > 8))
            {
                maxMoves.Add(newPos);
                //check if spot is occupied. if so remove it
                BoardPosition boardPosition = BoardManager.GetBoardTile(newPos);
                if ((boardPosition.isOccupied))
                {
                    rightEnd = true;
                    maxMoves.Remove(newPos);
                    if (boardPosition.occupantColor != pieceColor)
                        captureMoves.Add(newPos);
                }
            }       
        }


        // Castle

        Move newMove = new Move(maxMoves.ToArray(), captureMoves.ToArray(), specialMoves.ToArray());
        return newMove;
    }

    private Move knightMove()
    {
        List<Position> maxMoves = new List<Position>();
        List<Position> captureMoves = new List<Position>();
        List<Position> specialMoves = new List<Position>();

        //Check possible moves
        //Knight Find moves algorithm
        for(int x = 0, sign = 0; x < 2; x++)
        {
            switch (x)
            {
                case 0:
                    sign = -1;
                    break;
                case 1:
                    sign = 1;
                    break;
                default:
                    break;
            }
            for (int i = -2, j = 0; i < 3; i++)
            {
                if (i == 0)
                    continue;

                switch(Mathf.Abs(i))
                {
                    case 1:
                        j = 2;
                        break;
                    case 2:
                        j = 1;
                        break;
                    default:
                        break;
                }


                Position newPos = new Position((BoardManager.PosCharToInt(piecePosition.xPos) + i), (piecePosition.yPos + (sign * (i/i)) * j));

                //check if spot is on board
                int xPos = BoardManager.PosCharToInt(newPos.xPos),
                    yPos = newPos.yPos;

                if (!(xPos < 1 || xPos > 8 || yPos < 1 || yPos > 8))
                {
                    maxMoves.Add(newPos);
                    //check if spot is occupied. if so remove it
                    BoardPosition boardPosition = BoardManager.GetBoardTile(newPos);
                    if ((boardPosition.isOccupied))
                    {
                        maxMoves.Remove(newPos);
                        if (boardPosition.occupantColor != pieceColor)
                            captureMoves.Add(newPos);
                    }
                }


            }

        }

        Move newMove = new Move(maxMoves.ToArray(), captureMoves.ToArray(), specialMoves.ToArray());
        return newMove;
    }

    private Move bishopMove()
    {
        List<Position> maxMoves = new List<Position>();
        List<Position> captureMoves = new List<Position>();
        List<Position> specialMoves = new List<Position>();

        bool leftUpEnd, rightUpEnd, leftDownEnd, rightDownEnd;
        leftUpEnd = rightUpEnd = leftDownEnd = rightDownEnd = false;

        //Check possible moves

        //Check up movement
        for (int i = 0; i < 8; i++)
        {
            //If a spot in it's way is occupied stop checking for spots beyond
            if (leftUpEnd)
                break;
            Position newPos = new Position((BoardManager.PosCharToInt(piecePosition.xPos) - (i + 1)), (piecePosition.yPos + (i + 1)));
            Debug.Log(piecePosition.yPos + " + " + i + " + 1 = " + (piecePosition.yPos + (i + 1)));

            //check if spot is on board
            int xPos = BoardManager.PosCharToInt(newPos.xPos),
                yPos = newPos.yPos;

            if (!(xPos < 1 || xPos > 8 || yPos < 1 || yPos > 8))
            {
                maxMoves.Add(newPos);
                //check if spot is occupied. if so remove it
                BoardPosition boardPosition = BoardManager.GetBoardTile(newPos);
                if ((boardPosition.isOccupied))
                {
                    leftUpEnd = true;
                    maxMoves.Remove(newPos);
                    if (boardPosition.occupantColor != pieceColor)
                        captureMoves.Add(newPos);
                }
            }


        }
        //right down movement
        for (int i = 0; i < 8; i++)
        {
            //If a spot in it's way is occupied stop checking for spots beyond
            if (rightDownEnd)
                break;
            Position newPos = new Position((BoardManager.PosCharToInt(piecePosition.xPos) + (i + 1)), (piecePosition.yPos - (i + 1)));

            //check if spot is on board
            int xPos = BoardManager.PosCharToInt(newPos.xPos),
                yPos = newPos.yPos;

            if (!(xPos < 1 || xPos > 8 || yPos < 1 || yPos > 8))
            {
                maxMoves.Add(newPos);
                //check if spot is occupied. if so remove it
                BoardPosition boardPosition = BoardManager.GetBoardTile(newPos);
                if ((boardPosition.isOccupied))
                {
                    rightDownEnd = true;
                    maxMoves.Remove(newPos);
                    if (boardPosition.occupantColor != pieceColor)
                        captureMoves.Add(newPos);
                }
            }


        }
        //Check right up movement
        for (int i = 0; i < 8; i++)
        {
            //If a spot in it's way is occupied stop checking for spots beyond
            if (rightUpEnd)
                break;
            Position newPos = new Position((BoardManager.PosCharToInt(piecePosition.xPos) + (i + 1)), (piecePosition.yPos + (i + 1)));

            //check if spot is on board
            int xPos = BoardManager.PosCharToInt(newPos.xPos),
                yPos = newPos.yPos;

            if (!(xPos < 1 || xPos > 8 || yPos < 1 || yPos > 8))
            {
                maxMoves.Add(newPos);
                //check if spot is occupied. if so remove it
                BoardPosition boardPosition = BoardManager.GetBoardTile(newPos);
                if ((boardPosition.isOccupied))
                {
                    rightUpEnd = true;
                    maxMoves.Remove(newPos);
                    if (boardPosition.occupantColor != pieceColor)
                        captureMoves.Add(newPos);
                }
            }
        }
        //Check right movement
        for (int i = 0; i < 8; i++)
        {
            //If a spot in it's way is occupied stop checking for spots beyond
            if (leftDownEnd)
                break;
            Position newPos = new Position((BoardManager.PosCharToInt(piecePosition.xPos) - (i + 1)), (piecePosition.yPos - (i + 1)));

            //check if spot is on board
            int xPos = BoardManager.PosCharToInt(newPos.xPos),
                yPos = newPos.yPos;

            if (!(xPos < 1 || xPos > 8 || yPos < 1 || yPos > 8))
            {
                maxMoves.Add(newPos);
                //check if spot is occupied. if so remove it
                BoardPosition boardPosition = BoardManager.GetBoardTile(newPos);
                if ((boardPosition.isOccupied))
                {
                    leftDownEnd = true;
                    maxMoves.Remove(newPos);
                    if (boardPosition.occupantColor != pieceColor)
                        captureMoves.Add(newPos);
                }
            }
        }

        Move newMove = new Move(maxMoves.ToArray(), captureMoves.ToArray(), specialMoves.ToArray());
        return newMove;
    }

    private Move queenMove()
    {
        List<Position> maxMoves = new List<Position>();
        List<Position> captureMoves = new List<Position>();
        List<Position> specialMoves = new List<Position>();

        bool upEnd, leftEnd, rightEnd, downEnd;
        upEnd = leftEnd = rightEnd = downEnd = false;

        //Check possible moves

        //Check up movement
        for (int i = 0; i < 8; i++)
        {
            //If a spot in it's way is occupied stop checking for spots beyond
            if (upEnd)
                break;
            Position newPos = new Position(piecePosition.xPos, (piecePosition.yPos + (i + 1)));
            Debug.Log(piecePosition.yPos + " + " + i + " + 1 = " + (piecePosition.yPos + (i + 1)));

            //check if spot is on board
            int xPos = BoardManager.PosCharToInt(newPos.xPos),
                yPos = newPos.yPos;

            if (!(xPos < 1 || xPos > 8 || yPos < 1 || yPos > 8))
            {
                maxMoves.Add(newPos);
                //check if spot is occupied. if so remove it
                BoardPosition boardPosition = BoardManager.GetBoardTile(newPos);
                if ((boardPosition.isOccupied))
                {
                    upEnd = true;
                    maxMoves.Remove(newPos);
                    if (boardPosition.occupantColor != pieceColor)
                        captureMoves.Add(newPos);
                }
            }


        }
        //Check down movement
        for (int i = 0; i < 8; i++)
        {
            //If a spot in it's way is occupied stop checking for spots beyond
            if (downEnd)
                break;
            Position newPos = new Position(piecePosition.xPos, (piecePosition.yPos - (i + 1)));

            //check if spot is on board
            int xPos = BoardManager.PosCharToInt(newPos.xPos),
                yPos = newPos.yPos;

            if (!(xPos < 1 || xPos > 8 || yPos < 1 || yPos > 8))
            {
                maxMoves.Add(newPos);
                //check if spot is occupied. if so remove it
                BoardPosition boardPosition = BoardManager.GetBoardTile(newPos);
                if ((boardPosition.isOccupied))
                {
                    downEnd = true;
                    maxMoves.Remove(newPos);
                    if (boardPosition.occupantColor != pieceColor)
                        captureMoves.Add(newPos);
                }
            }


        }
        //Check left movement
        for (int i = 0; i < 8; i++)
        {
            //If a spot in it's way is occupied stop checking for spots beyond
            if (leftEnd)
                break;
            Position newPos = new Position((BoardManager.PosCharToInt(piecePosition.xPos) - (i + 1)), piecePosition.yPos);

            //check if spot is on board
            int xPos = BoardManager.PosCharToInt(newPos.xPos),
                yPos = newPos.yPos;

            if (!(xPos < 1 || xPos > 8 || yPos < 1 || yPos > 8))
            {
                maxMoves.Add(newPos);
                //check if spot is occupied. if so remove it
                BoardPosition boardPosition = BoardManager.GetBoardTile(newPos);
                if ((boardPosition.isOccupied))
                {
                    leftEnd = true;
                    maxMoves.Remove(newPos);
                    if (boardPosition.occupantColor != pieceColor)
                        captureMoves.Add(newPos);
                }
            }
        }
        //Check right movement horizontal and vertical
        for (int i = 0; i < 8; i++)
        {
            //If a spot in it's way is occupied stop checking for spots beyond
            if (rightEnd)
                break;
            Position newPos = new Position((BoardManager.PosCharToInt(piecePosition.xPos) + (i + 1)), piecePosition.yPos);

            //check if spot is on board
            int xPos = BoardManager.PosCharToInt(newPos.xPos),
                yPos = newPos.yPos;

            if (!(xPos < 1 || xPos > 8 || yPos < 1 || yPos > 8))
            {
                maxMoves.Add(newPos);
                //check if spot is occupied. if so remove it
                BoardPosition boardPosition = BoardManager.GetBoardTile(newPos);
                if ((boardPosition.isOccupied))
                {
                    rightEnd = true;
                    maxMoves.Remove(newPos);
                    if (boardPosition.occupantColor != pieceColor)
                        captureMoves.Add(newPos);
                }
            }
        }

        bool leftUpEnd, rightUpEnd, leftDownEnd, rightDownEnd;
        leftUpEnd = rightUpEnd = leftDownEnd = rightDownEnd = false;

        //Check possible moves

        //Check up movement
        for (int i = 0; i < 8; i++)
        {
            //If a spot in it's way is occupied stop checking for spots beyond
            if (leftUpEnd)
                break;
            Position newPos = new Position((BoardManager.PosCharToInt(piecePosition.xPos) - (i + 1)), (piecePosition.yPos + (i + 1)));
            Debug.Log(piecePosition.yPos + " + " + i + " + 1 = " + (piecePosition.yPos + (i + 1)));

            //check if spot is on board
            int xPos = BoardManager.PosCharToInt(newPos.xPos),
                yPos = newPos.yPos;

            if (!(xPos < 1 || xPos > 8 || yPos < 1 || yPos > 8))
            {
                maxMoves.Add(newPos);
                //check if spot is occupied. if so remove it
                BoardPosition boardPosition = BoardManager.GetBoardTile(newPos);
                if ((boardPosition.isOccupied))
                {
                    leftUpEnd = true;
                    maxMoves.Remove(newPos);
                    if (boardPosition.occupantColor != pieceColor)
                        captureMoves.Add(newPos);
                }
            }


        }
        //right down movement
        for (int i = 0; i < 8; i++)
        {
            //If a spot in it's way is occupied stop checking for spots beyond
            if (rightDownEnd)
                break;
            Position newPos = new Position((BoardManager.PosCharToInt(piecePosition.xPos) + (i + 1)), (piecePosition.yPos - (i + 1)));

            //check if spot is on board
            int xPos = BoardManager.PosCharToInt(newPos.xPos),
                yPos = newPos.yPos;

            if (!(xPos < 1 || xPos > 8 || yPos < 1 || yPos > 8))
            {
                maxMoves.Add(newPos);
                //check if spot is occupied. if so remove it
                BoardPosition boardPosition = BoardManager.GetBoardTile(newPos);
                if ((boardPosition.isOccupied))
                {
                    rightDownEnd = true;
                    maxMoves.Remove(newPos);
                    if (boardPosition.occupantColor != pieceColor)
                        captureMoves.Add(newPos);
                }
            }


        }
        //Check right up movement
        for (int i = 0; i < 8; i++)
        {
            //If a spot in it's way is occupied stop checking for spots beyond
            if (rightUpEnd)
                break;
            Position newPos = new Position((BoardManager.PosCharToInt(piecePosition.xPos) + (i + 1)), (piecePosition.yPos + (i + 1)));

            //check if spot is on board
            int xPos = BoardManager.PosCharToInt(newPos.xPos),
                yPos = newPos.yPos;

            if (!(xPos < 1 || xPos > 8 || yPos < 1 || yPos > 8))
            {
                maxMoves.Add(newPos);
                //check if spot is occupied. if so remove it
                BoardPosition boardPosition = BoardManager.GetBoardTile(newPos);
                if ((boardPosition.isOccupied))
                {
                    rightUpEnd = true;
                    maxMoves.Remove(newPos);
                    if (boardPosition.occupantColor != pieceColor)
                        captureMoves.Add(newPos);
                }
            }
        }

        //Check right movement diagonal
        for (int i = 0; i < 8; i++)
        {
            //If a spot in it's way is occupied stop checking for spots beyond
            if (leftDownEnd)
                break;
            Position newPos = new Position((BoardManager.PosCharToInt(piecePosition.xPos) - (i + 1)), (piecePosition.yPos - (i + 1)));

            //check if spot is on board
            int xPos = BoardManager.PosCharToInt(newPos.xPos),
                yPos = newPos.yPos;

            if (!(xPos < 1 || xPos > 8 || yPos < 1 || yPos > 8))
            {
                maxMoves.Add(newPos);
                //check if spot is occupied. if so remove it
                BoardPosition boardPosition = BoardManager.GetBoardTile(newPos);
                if ((boardPosition.isOccupied))
                {
                    leftDownEnd = true;
                    maxMoves.Remove(newPos);
                    if (boardPosition.occupantColor != pieceColor)
                        captureMoves.Add(newPos);
                }
            }
        }

        Move newMove = new Move(maxMoves.ToArray(), captureMoves.ToArray(), specialMoves.ToArray());
        return newMove;
    }

    private Move kingMove()
    {
        List<Position> maxMoves = new List<Position>();
        List<Position> captureMoves = new List<Position>();
        List<Position> specialMoves = new List<Position>();

        bool upEnd, leftEnd, rightEnd, downEnd;
        upEnd = leftEnd = rightEnd = downEnd = false;

        //Check possible moves

        //Check up movement
        for (int i = 0; i < 1; i++)
        {
            //If a spot in it's way is occupied stop checking for spots beyond
            if (upEnd)
                break;
            Position newPos = new Position(piecePosition.xPos, (piecePosition.yPos + (i + 1)));
            Debug.Log(piecePosition.yPos + " + " + i + " + 1 = " + (piecePosition.yPos + (i + 1)));

            //check if spot is on board
            int xPos = BoardManager.PosCharToInt(newPos.xPos),
                yPos = newPos.yPos;

            if (!(xPos < 1 || xPos > 8 || yPos < 1 || yPos > 8))
            {
                maxMoves.Add(newPos);
                //check if spot is occupied. if so remove it
                BoardPosition boardPosition = BoardManager.GetBoardTile(newPos);
                if ((boardPosition.isOccupied))
                {
                    upEnd = true;
                    maxMoves.Remove(newPos);
                    if (boardPosition.occupantColor != pieceColor)
                        captureMoves.Add(newPos);
                }
            }


        }
        //Check down movement
        for (int i = 0; i < 1; i++)
        {
            //If a spot in it's way is occupied stop checking for spots beyond
            if (downEnd)
                break;
            Position newPos = new Position(piecePosition.xPos, (piecePosition.yPos - (i + 1)));

            //check if spot is on board
            int xPos = BoardManager.PosCharToInt(newPos.xPos),
                yPos = newPos.yPos;

            if (!(xPos < 1 || xPos > 8 || yPos < 1 || yPos > 8))
            {
                maxMoves.Add(newPos);
                //check if spot is occupied. if so remove it
                BoardPosition boardPosition = BoardManager.GetBoardTile(newPos);
                if ((boardPosition.isOccupied))
                {
                    downEnd = true;
                    maxMoves.Remove(newPos);
                    if (boardPosition.occupantColor != pieceColor)
                        captureMoves.Add(newPos);
                }
            }


        }
        //Check left movement
        for (int i = 0; i < 1; i++)
        {
            //If a spot in it's way is occupied stop checking for spots beyond
            if (leftEnd)
                break;
            Position newPos = new Position((BoardManager.PosCharToInt(piecePosition.xPos) - (i + 1)), piecePosition.yPos);

            //check if spot is on board
            int xPos = BoardManager.PosCharToInt(newPos.xPos),
                yPos = newPos.yPos;

            if (!(xPos < 1 || xPos > 8 || yPos < 1 || yPos > 8))
            {
                maxMoves.Add(newPos);
                //check if spot is occupied. if so remove it
                BoardPosition boardPosition = BoardManager.GetBoardTile(newPos);
                if ((boardPosition.isOccupied))
                {
                    leftEnd = true;
                    maxMoves.Remove(newPos);
                    if (boardPosition.occupantColor != pieceColor)
                        captureMoves.Add(newPos);
                }
            }
        }
        //Check right movement horizontal and vertical
        for (int i = 0; i < 1; i++)
        {
            //If a spot in it's way is occupied stop checking for spots beyond
            if (rightEnd)
                break;
            Position newPos = new Position((BoardManager.PosCharToInt(piecePosition.xPos) + (i + 1)), piecePosition.yPos);

            //check if spot is on board
            int xPos = BoardManager.PosCharToInt(newPos.xPos),
                yPos = newPos.yPos;

            if (!(xPos < 1 || xPos > 8 || yPos < 1 || yPos > 8))
            {
                maxMoves.Add(newPos);
                //check if spot is occupied. if so remove it
                BoardPosition boardPosition = BoardManager.GetBoardTile(newPos);
                if ((boardPosition.isOccupied))
                {
                    rightEnd = true;
                    maxMoves.Remove(newPos);
                    if (boardPosition.occupantColor != pieceColor)
                        captureMoves.Add(newPos);
                }
            }
        }

        bool leftUpEnd, rightUpEnd, leftDownEnd, rightDownEnd;
        leftUpEnd = rightUpEnd = leftDownEnd = rightDownEnd = false;

        //Check possible moves

        //Check up movement
        for (int i = 0; i < 1; i++)
        {
            //If a spot in it's way is occupied stop checking for spots beyond
            if (leftUpEnd)
                break;
            Position newPos = new Position((BoardManager.PosCharToInt(piecePosition.xPos) - (i + 1)), (piecePosition.yPos + (i + 1)));
            Debug.Log(piecePosition.yPos + " + " + i + " + 1 = " + (piecePosition.yPos + (i + 1)));

            //check if spot is on board
            int xPos = BoardManager.PosCharToInt(newPos.xPos),
                yPos = newPos.yPos;

            if (!(xPos < 1 || xPos > 8 || yPos < 1 || yPos > 8))
            {
                maxMoves.Add(newPos);
                //check if spot is occupied. if so remove it
                BoardPosition boardPosition = BoardManager.GetBoardTile(newPos);
                if ((boardPosition.isOccupied))
                {
                    leftUpEnd = true;
                    maxMoves.Remove(newPos);
                    if (boardPosition.occupantColor != pieceColor)
                        captureMoves.Add(newPos);
                }
            }


        }
        //right down movement
        for (int i = 0; i < 1; i++)
        {
            //If a spot in it's way is occupied stop checking for spots beyond
            if (rightDownEnd)
                break;
            Position newPos = new Position((BoardManager.PosCharToInt(piecePosition.xPos) + (i + 1)), (piecePosition.yPos - (i + 1)));

            //check if spot is on board
            int xPos = BoardManager.PosCharToInt(newPos.xPos),
                yPos = newPos.yPos;

            if (!(xPos < 1 || xPos > 8 || yPos < 1 || yPos > 8))
            {
                maxMoves.Add(newPos);
                //check if spot is occupied. if so remove it
                BoardPosition boardPosition = BoardManager.GetBoardTile(newPos);
                if ((boardPosition.isOccupied))
                {
                    rightDownEnd = true;
                    maxMoves.Remove(newPos);
                    if (boardPosition.occupantColor != pieceColor)
                        captureMoves.Add(newPos);
                }
            }


        }
        //Check right up movement
        for (int i = 0; i < 1; i++)
        {
            //If a spot in it's way is occupied stop checking for spots beyond
            if (rightUpEnd)
                break;
            Position newPos = new Position((BoardManager.PosCharToInt(piecePosition.xPos) + (i + 1)), (piecePosition.yPos + (i + 1)));

            //check if spot is on board
            int xPos = BoardManager.PosCharToInt(newPos.xPos),
                yPos = newPos.yPos;

            if (!(xPos < 1 || xPos > 8 || yPos < 1 || yPos > 8))
            {
                maxMoves.Add(newPos);
                //check if spot is occupied. if so remove it
                BoardPosition boardPosition = BoardManager.GetBoardTile(newPos);
                if ((boardPosition.isOccupied))
                {
                    rightUpEnd = true;
                    maxMoves.Remove(newPos);
                    if (boardPosition.occupantColor != pieceColor)
                        captureMoves.Add(newPos);
                }
            }
        }

        //Check right movement diagonal
        for (int i = 0; i < 1; i++)
        {
            //If a spot in it's way is occupied stop checking for spots beyond
            if (leftDownEnd)
                break;
            Position newPos = new Position((BoardManager.PosCharToInt(piecePosition.xPos) - (i + 1)), (piecePosition.yPos - (i + 1)));

            //check if spot is on board
            int xPos = BoardManager.PosCharToInt(newPos.xPos),
                yPos = newPos.yPos;

            if (!(xPos < 1 || xPos > 8 || yPos < 1 || yPos > 8))
            {
                maxMoves.Add(newPos);
                //check if spot is occupied. if so remove it
                BoardPosition boardPosition = BoardManager.GetBoardTile(newPos);
                if ((boardPosition.isOccupied))
                {
                    leftDownEnd = true;
                    maxMoves.Remove(newPos);
                    if (boardPosition.occupantColor != pieceColor)
                        captureMoves.Add(newPos);
                }
            }
        }

        Move newMove = new Move(maxMoves.ToArray(), captureMoves.ToArray(), specialMoves.ToArray());
        return newMove;
    }


}


public struct Move
{
    public Position[] movablePositions,
               captureMoves,
               specialMoves;
    public Move(Position[] _movablePositions, Position[] _captureMoves, Position[] _specialMoves)
    {
        movablePositions = _movablePositions;
        captureMoves = _captureMoves;
        specialMoves = _specialMoves;
    }

}