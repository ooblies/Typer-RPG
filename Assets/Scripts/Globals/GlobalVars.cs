using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalVars : Singleton<GlobalVars>
{
    public string CurrentTypedString = "";    

    public float playerCurrentHealth = 100;
    public int playerMaxHealth = 100;
    public float enemyCurrentHealth = 100;
    public int enemyMaxHealth =100;

    public Difficulty difficultySetting;

    public Menu currentMenu = Menu.Start;

    public Enemy selectedEnemy;

    public int selectedCharacters = 0;
    public int selectedSkills = 0;

    public int wordsTyped;

    public enum Menu
    {
        Start = 0,
        Character = 1,
        Enemy = 2,
        Battle = 3,
        GameOver = 4,
    }

    public enum Difficulty
    { 
        Easy = 0,
        Medium = 1,
        Hard = 2,    
    }


    private void Awake()
    {

    }
    private void Start()
    {

    }
}
