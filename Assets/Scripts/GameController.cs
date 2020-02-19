using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

public class GameController : MonoBehaviour
{
    public Text displayText;
    public Canvas startUI;
    public Canvas characterUI;
    public Canvas enemyUI;
    public Canvas battleUI;
    public Canvas gameOverUI;
    private BattleController bc;

    // Start is called before the first frame update
    void Start()
    {
        startUI = startUI.GetComponent<Canvas>();
        characterUI = characterUI.GetComponent<Canvas>();
        enemyUI = enemyUI.GetComponent<Canvas>();
        battleUI = battleUI.GetComponent<Canvas>();
        gameOverUI = gameOverUI.GetComponent<Canvas>();

        bc = GetComponentInParent<BattleController>();

        LoadMenu(GlobalVars.Menu.Start);

        Debug.Log("Version - 1.0");

    }
    private void Awake()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (GlobalEffects.Instance.getCurrentFreezeDuration(GlobalEffects.EffectTarget.Friendly) <= 0)
        {
            if (Input.anyKeyDown)
            {
                string strDown = Input.inputString;

                Regex r = new Regex("^[a-zA-Z0-9]*$");
                if (r.IsMatch(strDown) || strDown == " ")
                {
                    updateCurrentString(GlobalVars.Instance.CurrentTypedString += strDown);
                }
            }
            if (Input.GetKeyDown(KeyCode.Return))
            {
                updateCurrentString("");
            }
            if (Input.GetKeyDown(KeyCode.Backspace))
            {
                if (GlobalVars.Instance.CurrentTypedString.Length > 0)
                {
                    updateCurrentString(GlobalVars.Instance.CurrentTypedString.Substring(0, GlobalVars.Instance.CurrentTypedString.Length - 1));
                }                
            }
        }
        
        
        if (GlobalVars.Instance.CurrentTypedString != displayText.text)
        {
            updateCurrentString(GlobalVars.Instance.CurrentTypedString);
        }
    }

    public void LoadMenu(GlobalVars.Menu menu)
    {
        startUI.enabled = false;
        characterUI.enabled = false;
        enemyUI.enabled = false;
        battleUI.enabled = false;
        gameOverUI.enabled = false;

        GlobalVars.Instance.currentMenu = menu;

        Time.timeScale = 0;

        switch (menu)
        {
            case GlobalVars.Menu.Start:
                startUI.enabled = true;
                break;
            case GlobalVars.Menu.Character:
                characterUI.enabled = true;
                break;
            case GlobalVars.Menu.Enemy:
                enemyUI.enabled = true;
                break;
            case GlobalVars.Menu.Battle:
                battleUI.enabled = true;
                bc.StartBattle();
                break;
            case GlobalVars.Menu.GameOver:
                gameOverUI.enabled = true;
                break;
            default:
                break;
        }
    }
    
    void updateCurrentString(string c) {
        GlobalVars.Instance.CurrentTypedString = c;
        displayText.text = GlobalVars.Instance.CurrentTypedString;
    }
}
