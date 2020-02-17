using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MenuButtonController : MonoBehaviour
{
    public Text errorMessage;
    private GameController gc;
    private BattleController bc;

    public Dropdown difficulty;
    // Start is called before the first frame update
    void Start()
    {
        gc = GameObject.FindObjectsOfType<GameController>()[0];
        bc = GameObject.FindObjectsOfType<BattleController>()[0];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void randomStart()
    {
        setDifficulty();

        GlobalVars.Instance.playerCurrentHealth = GlobalVars.Instance.playerMaxHealth;
        GlobalVars.Instance.enemyCurrentHealth = GlobalVars.Instance.enemyMaxHealth;

        GlobalVars.Instance.selectedEnemy = (Enemy)Random.Range(1, 6);
        bc.selectedSkillIds = GlobalSkills.Instance.getRandomSkills().Select(x => x.skillId).ToList();

        bc.updateSkills();

        gc.LoadMenu(GlobalVars.Menu.Battle);
    }

    private void setDifficulty()
    {
        GlobalEffects.Instance.clearEffects();

        switch (difficulty.options[difficulty.value].text)
        {
            case "Easy":
                GlobalEffects.Effect easy = new GlobalEffects.Effect
                {
                    effectType = GlobalEffects.EffectType.CooldownMultiplier,
                    effectTarget = GlobalEffects.EffectTarget.Enemy,                    
                    ongoing = true,
                    effect = 2,                    
                    duration = 600,
                    sourceName = "Easy Mode",
                };

                GlobalEffects.Instance.addEffect(easy);
                break;
            case "Medium":
                break;
            case "Hard":
                GlobalEffects.Effect hard = new GlobalEffects.Effect
                {
                    effectType = GlobalEffects.EffectType.CooldownMultiplier,
                    effectTarget = GlobalEffects.EffectTarget.Enemy,
                    ongoing = true,
                    effect = .5f,
                    duration = 600,
                    sourceName = "Easy Mode",
                };

                GlobalEffects.Instance.addEffect(hard);
                break;
            default:
                break;
        }

        Debug.Log("Difficulty set to - " + difficulty.options[difficulty.value].text);
    }

    public void nextMenu()
    {
        if (errorMessage)
        {
            errorMessage.text = "";
        }
        
        switch (GlobalVars.Instance.currentMenu)
        {
            case GlobalVars.Menu.Start:                
                gc.LoadMenu(GlobalVars.Menu.Character);
                GlobalVars.Instance.playerCurrentHealth = GlobalVars.Instance.playerMaxHealth;
                GlobalVars.Instance.enemyCurrentHealth = GlobalVars.Instance.enemyMaxHealth;


                setDifficulty();
                Debug.Log("Character Menu");
                break;
            case GlobalVars.Menu.Character:
                if (GlobalVars.Instance.selectedSkills < 9)
                {
                    errorMessage.text = "Please select 9 skills";
                    return;
                }
                applySkills();
                gc.LoadMenu(GlobalVars.Menu.Enemy);
                Debug.Log("Enemy Menu");
                break;
            case GlobalVars.Menu.Enemy:
                if (GlobalVars.Instance.selectedEnemy == 0)
                {
                    errorMessage.text = "Please select an enemy";
                    return;
                }
                gc.LoadMenu(GlobalVars.Menu.Battle);
                Debug.Log("Battle Scene");
                break;
            case GlobalVars.Menu.Battle:
                break;
            case GlobalVars.Menu.GameOver:
                gc.LoadMenu(GlobalVars.Menu.Start);
                break;
            default:
                break;
        }
    }

    public void applySkills()
    {
        BattleController bc = GameObject.Find("Controllers").GetComponent<BattleController>();

        SkillPaneController[] spcs = FindObjectsOfType<SkillPaneController>();

        bc.selectedSkillIds = spcs.Where(s => s.outline.enabled == true).OrderBy(k => GlobalSkills.Instance.getSkillById(k.skillId).orderBy).Take(9).Select(x => x.skillId).ToList();

        bc.updateSkills();
    }


    public void previousMenu()
    {
        GameController gc = GameObject.FindObjectsOfType<GameController>()[0];

        if (errorMessage)
        {
            errorMessage.text = "";
        }

        switch (GlobalVars.Instance.currentMenu)
        {
            case GlobalVars.Menu.Start:
                break;
            case GlobalVars.Menu.Character:
                gc.LoadMenu(GlobalVars.Menu.Start);
                break;
            case GlobalVars.Menu.Enemy:
                gc.LoadMenu(GlobalVars.Menu.Character);
                break;
            case GlobalVars.Menu.Battle:
                gc.LoadMenu(GlobalVars.Menu.Enemy);
                break;
            case GlobalVars.Menu.GameOver:
                break;
            default:
                break;
        }
    }
}
