using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAssets : MonoBehaviour
{
    private static GameAssets instance;

    public static GameAssets Instance
    {
        get
        {
            if (instance == null) instance = (Instantiate(Resources.Load("GameAssets")) as GameObject).GetComponent<GameAssets>();
            return instance;
        }
    }





    [field: SerializeField]
    public UnitSO AICivilianUnitSO
    {
        get;
        private set;
    }
    [field: SerializeField]
    public UnitSO AISwordsManUnitSO
    {
        get;
        private set;
    }
    [field: SerializeField]
    public UnitSO AIArcherUnitSO
    {
        get;
        private set;
    }

    [field: SerializeField]
    public Sprite SinglePixelSprite
    {
        get;
        private set;
    }



}
