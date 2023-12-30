using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorPiece : MonoBehaviour
{
    public enum ColorType
    {
        FIRE3,
        FIRE2,
        FIRE1,
        WATER3,
        WATER2,
        WATER1,
        EARTH3,
        EARTH2,
        EARTH1,
        ANY,
        COUNT
    };

    [System.Serializable]
    public struct ColorSprite
    {   
        public ColorType color;
        public Sprite sprite;
    };

    public ColorSprite[] colorSprites;

    private ColorType color;
    public ColorType Color
    {
        get {return color;}
        set {SetColor(value);}
    }

    public int NumColors
    {
        get {return colorSprites.Length;}
    }
    private SpriteRenderer sprite;

    private Dictionary<ColorType, Sprite> colorSpriteDict;

    void Awake() 
    {
        sprite = transform.Find("piece").GetComponent<SpriteRenderer>();

        colorSpriteDict = new Dictionary<ColorType, Sprite>();
        for (int i = 0; i < colorSprites.Length; i++) 
        {
            if (!colorSpriteDict.ContainsKey(colorSprites[i].color))
            {
                colorSpriteDict.Add(colorSprites[i].color, colorSprites[i].sprite);
            }
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetColor(ColorType newColor)
    {
        color = newColor;
        if (colorSpriteDict.ContainsKey(newColor))
        {
            sprite.sprite = colorSpriteDict[newColor];
        }
    }

    public static ColorType Promote(ColorType initColor)
    {
        if (initColor == ColorType.FIRE1)
        {
            return ColorType.FIRE2;
        }
        if (initColor == ColorType.FIRE2)
        {
            return ColorType.FIRE3;
        }
        if (initColor == ColorType.WATER1)
        {
            return ColorType.WATER2;
        }
        if (initColor == ColorType.WATER2)
        {
            return ColorType.WATER3;
        }
        if (initColor == ColorType.EARTH1)
        {
            return ColorType.EARTH2;
        }
        if (initColor == ColorType.EARTH2)
        {
            return ColorType.EARTH3;
        }
        else
        {
            return initColor;
        }
    }
}
