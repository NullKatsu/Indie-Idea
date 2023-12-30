using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dish : MonoBehaviour
{
    public GamePiece[,] pieces;
    private List<Ingredient> recipe = new List<Ingredient>();

    public enum DishType
    {
        /*
        FIRE3,
        FIRE2,
        FIRE1,
        WATER3,
        WATER2,
        WATER1,
        EARTH3,
        EARTH2,
        EARTH1,
        */
        ANY,
        COUNT,
    };

    [System.Serializable]
    public struct DishSprite
    {   
        public DishType color;
        public Sprite sprite;
    };

    // public Ingredient primary
    // public Ingredient secondary
    
    // Start is called before the first frame update
    void Awake()
    {
        pieces = new GamePiece[4, 4];
    }
    
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddRecipe(Ingredient ingredient)
    {
        recipe.Add(ingredient);
        /*
        if (recipe.Count == 2 && )
        {
            //Sprite Check
        }
        */
    }

}
