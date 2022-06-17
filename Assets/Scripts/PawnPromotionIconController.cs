using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnPromotionIconController : MonoBehaviour
{
    [SerializeField]
    private PiecesType pieceType;

    private PiecesManager pieceManager;
    private PawnPromotionMenuScript pawnPromotionMenuScript;

    private void Awake()
    {
        pieceManager = FindObjectOfType<PiecesManager>();
        pawnPromotionMenuScript = FindObjectOfType<PawnPromotionMenuScript>();
    }

    private void OnMouseDown()
    {
        pieceManager.PromotePawn(BoardManager.GetBoardTile(pawnPromotionMenuScript.promotionSpot), pieceType);
        pawnPromotionMenuScript.HideMenu();
    }
}
