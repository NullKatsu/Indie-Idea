using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public enum PieceType
    {
        EMPTY,
        NORMAL,
        SELECT,
        CIRCLE,
        COUNT,
    };

    [System.Serializable]
    public struct PiecePrefab 
    {
        public PieceType type;
        public GameObject prefab;
    };

    public int xDim = 4;
    public int yDim = 4;
    public int cursorDim = 2;
    public int matchNum = 3;
    public float fillTime;
    public PiecePrefab[] piecePrefabs;
    public GameObject dishPrefab;
    
    public GamePiece[,] cursorGrid;

    private Dish Dish;
    private GameObject dish;
    private List<GamePiece> matchingPieces = new List<GamePiece>();
    private List<Coroutine> animationList = new List<Coroutine>();
    private Dictionary<PieceType,GameObject> piecePrefabDict;
    
    void Awake()
    {
        dish = (GameObject)Instantiate(dishPrefab, transform.position, Quaternion.identity, transform);
        Dish = dish.GetComponent<Dish>();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log(Dish1 is null ? "NULL" : "NOT NULL");
        piecePrefabDict = new Dictionary<PieceType, GameObject> ();

        for (int i = 0; i < piecePrefabs.Length; i++) 
        {
            if (!piecePrefabDict.ContainsKey(piecePrefabs[i].type))
            {
                piecePrefabDict.Add(piecePrefabs[i].type, piecePrefabs[i].prefab);
            }
        }
        //cursor pieces
        cursorGrid = new GamePiece[xDim, yDim];
        for (int x = 0; x < cursorDim; x++)
        {
            for (int y = 0; y < cursorDim; y++)
            {
                SpawnNewPiece(x, y, PieceType.SELECT, cursorGrid, transform);
            }
        }  
        //pieces
        Dish.pieces = new GamePiece[xDim, yDim];
        for (int x = 0; x < xDim; x++) 
        {
            for (int y = 0; y < yDim; y++)
            {
                SpawnNewPiece(x, y, PieceType.EMPTY, Dish.pieces, dish.transform);
                /*
                [0,0] [1,0] [2,0] [3,0]
                [0,1] [1,1] [2,1] [3,1]
                [0,2] [1,2] [2,2] [3,2]
                [0,3] [1,3] [2,3] [3,3]
                */
            }
        }
        StartCoroutine(Fill());
    }

    // Update is called once per frame
    void Update()
    {

    }

    public IEnumerator Fill()
    {
        //yield return new WaitForSeconds(3.0f);
        while(FillStep()) 
        {
            yield return new WaitForSeconds(fillTime);
        }
    }
    public bool FillStep()
    {
        bool movedPiece = false;
        for (int y = yDim - 2; y >= 0; y--)
        {
            for (int x = 0; x < xDim; x++)
            {
                //Debug.Log("Cords: "+ x + ", " + y); // (0,2)
                //Debug.Log(Dish is null ? "NULL" : "NOT NULL");
                //GamePiece piece = Dish.GetPieceAt(x,y); //Null Reference
                GamePiece piece = Dish.pieces[x,y];
                if (piece.IsMovable())
                {
                    GamePiece pieceBelow = Dish.pieces[x, y + 1];
                    if (pieceBelow.Type == PieceType.EMPTY)
                    {
                        Destroy(pieceBelow.gameObject);
                        piece.MovableComponent.Move(x, y + 1, fillTime);
                        Dish.pieces[x, y + 1] = piece;
                        SpawnNewPiece(x, y, PieceType.EMPTY, Dish.pieces, dish.transform);
                        movedPiece = true;
                    }
                }
            }
        }
        
        for (int x = 0; x < xDim; x++)
        {
            GamePiece pieceBelow = Dish.pieces[x,0];
            if (pieceBelow.Type == PieceType.EMPTY)
            {
                Destroy(pieceBelow.gameObject);
                //SpawnNewPiece(x, -1, PieceType.NORMAL, pieces, transform);
                GameObject newPiece = (GameObject)Instantiate(piecePrefabDict[PieceType.NORMAL], GetWorldPosition(x, -1), Quaternion.identity);
                newPiece.transform.parent = dish.transform;

                Dish.pieces[x,0] = newPiece.GetComponent<GamePiece>();
                Dish.pieces[x,0].Init(x, -1, this, PieceType.NORMAL);
                Dish.pieces[x,0].MovableComponent.Move(x,0, fillTime);
                Dish.pieces[x,0].ColorComponent.SetColor((ColorPiece.ColorType)Random.Range(0, Dish.pieces[x,0].ColorComponent.NumColors));
                movedPiece = true;
            }
        }
        return movedPiece;
    }
    
    public List<GamePiece> FindHorizontalMatch(int col, int row, int matchNum)
    {
        GamePiece piece = Dish.pieces[col, row];
        List<GamePiece> horizontalPieces = new List<GamePiece>();
        if (piece.IsColored())
        {
            horizontalPieces.Add(piece);
            for (int i = col + 1; i < col + matchNum; i++)
            {
                GamePiece nextColumn = Dish.pieces[i, row];
                if (nextColumn.IsColored() && nextColumn.ColorComponent.Color == piece.ColorComponent.Color)
                {
                    horizontalPieces.Add(nextColumn);
                } else {
                    break;
                }
            }
        }
        return horizontalPieces;
    }

    public List<GamePiece> FindVerticalMatch(int col, int row, int matchNum)
    {
        GamePiece piece = Dish.pieces[col, row];
        List<GamePiece> verticalPieces = new List<GamePiece>();
        if (piece.IsColored())
        {
            verticalPieces.Add(piece);
            for (int i = row + 1; i < row + matchNum; i++)
            {
                GamePiece nextRow = Dish.pieces[col, i];
                if (nextRow.IsColored() && nextRow.ColorComponent.Color == piece.ColorComponent.Color)
                {
                    verticalPieces.Add(nextRow);
                } else {
                    break;
                }
            }
        }
        return verticalPieces;
    }

    public void GetMatch(int matchNum) // only works for a uniform grid (ex: 2x2, 4x4)
    {   
        //List<GamePiece> matchingPieces = new List<GamePiece>();
        for (int x = 0; x <= xDim - matchNum; x++) 
        {
            for (int y = 0; y < yDim; y++)
            {
                List<GamePiece> horizontalPieces = FindHorizontalMatch(x, y, matchNum);
                if (horizontalPieces.Count == matchNum && matchingPieces.Count < matchNum)
                {
                    for (int i = 0; i < horizontalPieces.Count; i++)
                    {
                        if (ColorPiece.Promote(horizontalPieces[i].ColorComponent.Color) != horizontalPieces[i].ColorComponent.Color && !horizontalPieces[i].ClearableComponent.IsBeingCleared)
                        {
                            horizontalPieces[i].ClearableComponent.Clear();
                            if ((i - 1) % matchNum == 0)
                            {
                                horizontalPieces[i].ClearableComponent.IsPreserved = true;
                            }
                            matchingPieces.Add(horizontalPieces[i]);
                            animationList.Add(StartCoroutine(PromoteAction(horizontalPieces[i])));
                        } 
                    }
                }

                List<GamePiece> verticalPieces = FindVerticalMatch(y, x, matchNum);
                if (verticalPieces.Count == matchNum && matchingPieces.Count < matchNum)
                {
                    for (int i = 0; i < verticalPieces.Count; i++)
                    {
                        if (ColorPiece.Promote(verticalPieces[i].ColorComponent.Color) != verticalPieces[i].ColorComponent.Color && !verticalPieces[i].ClearableComponent.IsBeingCleared)
                        {
                            verticalPieces[i].ClearableComponent.Clear();
                            if ((i - 1) % matchNum == 0)
                            {
                                verticalPieces[i].ClearableComponent.IsPreserved = true;
                            }
                            matchingPieces.Add(verticalPieces[i]);
                            animationList.Add(StartCoroutine(PromoteAction(verticalPieces[i])));
                        } 
                    }
                }
            }
        }
    }

    private IEnumerator PromoteAction(GamePiece piece)
    {
        yield return new WaitForSeconds(3.0f);
        if (piece.ClearableComponent.IsPreserved)
        {
            ColorPiece.ColorType upgradePiece = ColorPiece.Promote(piece.ColorComponent.Color);
            Dish.pieces[piece.X,piece.Y].ColorComponent.SetColor(upgradePiece);
            Dish.pieces[piece.X,piece.Y].ClearableComponent.IsPreserved = false; //temp
        } else {
            Destroy(Dish.pieces[piece.X,piece.Y].gameObject);
            SpawnNewPiece(piece.X, piece.Y, PieceType.EMPTY, Dish.pieces, dish.transform);
        }
        matchingPieces.Clear();
        StartCoroutine(Fill());
    }

    public void PromoteCheck(GamePiece piece) //stops existing match
    {
        if (piece.ClearableComponent.IsBeingCleared)
        {
            foreach (Coroutine co in animationList)
            {
                StopCoroutine(co);
            }
            animationList.Clear();
            foreach (GamePiece matchingPiece in matchingPieces)
            {
                //StopCoroutine(matchingPiece.ClearableComponent.co);
                matchingPiece.ClearableComponent.EndCoroutine();
            }
            matchingPieces.Clear();
        }
    }

    public Vector2 GetWorldPosition(int x, int y)
    {
        return new Vector2(xDim/8.0f + transform.position.x - xDim/2.0f + x, - yDim/8.0f + transform.position.y + yDim/2.0f - y);
    }

    public GamePiece SpawnNewPiece(int x, int y, PieceType type, GamePiece[,] group, Transform parentTrans)
    {
        GameObject newPiece = (GameObject)Instantiate(piecePrefabDict[type], GetWorldPosition(x,y), Quaternion.identity, parentTrans);

        group[x,y] = newPiece.GetComponent<GamePiece>();
        group[x,y].Init(x, y, this, type);
        return group[x,y];
    }
}