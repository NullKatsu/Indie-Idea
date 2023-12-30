using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cursor : MonoBehaviour
{
    private Grid grid;
    private GameObject dish;
    private Dish Dish;
    public static bool isPlayer; //temp
    public bool fixedRotation;
    public KeyCode KeyUp;
    public KeyCode KeyDown;
    public KeyCode KeyLeft;
    public KeyCode KeyRight;
    public KeyCode KeyRRight;
    public KeyCode KeyRLeft;
    public KeyCode KeyDebug;
    public KeyCode KeyInventory;
    
    private int xpos = 2; 
    private int ypos = 2; 

    private List<GamePiece> cursorPieces = new List<GamePiece>();

    void Awake() 
    {
        grid = GetComponent<Grid>();
        Dish = grid.transform.GetComponentInChildren<Dish>();
    }

    // Start is called before the first frame update
    void Start()
    {
       foreach (GamePiece piece in grid.cursorGrid)
       {
           if (piece != null && piece.IsMovable())
           {
               cursorPieces.Add(piece);
           }
       }
       /*
       [0,2]
       [1,3]
       */
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlayer)
        {
            if (Input.GetKeyDown(KeyUp) && xpos > grid.cursorDim)
            {
                xpos--;
                foreach (GamePiece piece in cursorPieces)
                {
                    piece.MovableComponent.Move(piece.X, piece.Y - 1, grid.fillTime);
                }
            }
            if (Input.GetKeyDown(KeyDown) && xpos < grid.xDim)
            {
                xpos++;
                foreach (GamePiece piece in cursorPieces)
                {
                    piece.MovableComponent.Move(piece.X, piece.Y + 1, grid.fillTime);
                }
            }
            if (Input.GetKeyDown(KeyLeft) && ypos > grid.cursorDim)
            {
                ypos--;
                foreach (GamePiece piece in cursorPieces)
                {
                    piece.MovableComponent.Move(piece.X - 1, piece.Y, grid.fillTime);
                }
            }
            if (Input.GetKeyDown(KeyRight) && ypos < grid.yDim)
            {
                ypos++;
                foreach (GamePiece piece in cursorPieces)
                {
                    piece.MovableComponent.Move(piece.X + 1, piece.Y, grid.fillTime);
                }
            }
            if (Input.GetKeyDown(KeyRRight) && !fixedRotation)
            {
                SwapPieces(cursorPieces[0], cursorPieces[2], cursorPieces[3], cursorPieces[1]);
                grid.GetMatch(3);
            }
            if (Input.GetKeyDown(KeyRLeft) && !fixedRotation)
            {
                SwapPieces(cursorPieces[0], cursorPieces[1], cursorPieces[3], cursorPieces[2]);
                grid.GetMatch(3);
            }
            if (Input.GetKeyDown(KeyInventory))
            {
                /*
                //isPlayer = false;
                foreach (GamePiece piece in cursorPieces)
                {
                    SpriteRenderer sprite = piece.GetComponentInChildren<SpriteRenderer>();
                    sprite.enabled = false;
                }
                */
            }
            if (Input.GetKeyDown(KeyDebug))
            {
                
            }
        }
    }
    
    private GamePiece GetPiece(GamePiece cursorPiece) //rename
    {
        GamePiece gridPiece = Dish.pieces[cursorPiece.X, cursorPiece.Y];
        grid.PromoteCheck(gridPiece);
        return gridPiece;
    }

    public void SwapPieces(GamePiece cursor1, GamePiece cursor2, GamePiece cursor3, GamePiece cursor4) // order in clockwise rotation
    {
        GamePiece piece1 = GetPiece(cursor1);
        GamePiece piece2 = GetPiece(cursor2);
        GamePiece piece3 = GetPiece(cursor3);
        GamePiece piece4 = GetPiece(cursor4);
        Dish.pieces[piece2.X, piece2.Y] = piece1;
        Dish.pieces[piece3.X, piece3.Y] = piece2;
        Dish.pieces[piece4.X, piece4.Y] = piece3;
        Dish.pieces[piece1.X, piece1.Y] = piece4;

        int piece1X = piece1.X;
        int piece1Y = piece1.Y;

        piece1.MovableComponent.Move(piece2.X, piece2.Y, grid.fillTime);
        piece2.MovableComponent.Move(piece3.X, piece3.Y, grid.fillTime);
        piece3.MovableComponent.Move(piece4.X, piece4.Y, grid.fillTime);
        piece4.MovableComponent.Move(piece1X, piece1Y, grid.fillTime);
    }
}
