using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings : MonoBehaviour
{
    [SerializeField]
    private StartColor startColor;
    private void Awake()
    {
        switch (startColor)
        {
            case StartColor.White:
                firstMoveColor = PieceColor.White;
                break;
            case StartColor.Black:
                firstMoveColor = PieceColor.Black;
                break;
            case StartColor.Random:
                float rand = Random.Range(0.0f, 1.0f);
                if(rand > 0.5f)
                    firstMoveColor = PieceColor.Black;
                else
                    firstMoveColor = PieceColor.White;
                break;
        }
    }
    public PieceColor firstMoveColor { get; private set; } = PieceColor.Black;
}

public enum StartColor { White, Black, Random}
