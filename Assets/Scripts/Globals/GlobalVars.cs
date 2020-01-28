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
    public int playerWordBlock = 0;
    public int playerLetterBlock = 0;


    private void Awake()
    {

    }
    private void Start()
    {

    }
}
