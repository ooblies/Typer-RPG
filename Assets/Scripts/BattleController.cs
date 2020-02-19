using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


public class BattleController : MonoBehaviour
{
    public Text playerHealth;
    public Text enemyHealth;

    public float playerProjectileStartingX = -7f;
    public float playerProjectileHitAtX = 6f;
    public float enemyProjectileStartingX = 6f;
    public float enemyProjectileHitAtX = -7f;

    public float projectileStartingYMax = 3f;
    public float projectileStartingYMin = -4f;
    

    public GameObject projectilePrefab;
    public float projectileMoveSpeed = 1f;

    private EnemyController ec;
    private GameController gc;

    public List<string> selectedSkillIds = new List<string>();

    private float startTime;

    public Text endTitle;
    public Text endEnemy;
    public Text endTime;
    public Text endWPM;
    
    // Start is called before the first frame update
    void Start()
    {
        ec = FindObjectOfType<EnemyController>();
        gc = FindObjectOfType<GameController>();
    }

    public void StartBattle()
    {
        Time.timeScale = 1;
        GlobalVars.Instance.CurrentTypedString = "";
        GlobalVars.Instance.wordsTyped = 0;
        startTime = Time.time;

        ec.LoadEnemy();

        ////Debug.Log("Battle Started");
        //Debug.Log("Battle start - " + string.Join(",",selectedSkillIds.ToArray()));
    }

    public void EndBattle(bool win)
    {

        Time.timeScale = 0;
        GlobalVars.Instance.CurrentTypedString = "";
        float duration = Time.time - startTime;

        foreach (GameObject go in GameObject.FindGameObjectsWithTag("Projectile"))
        {
            Destroy(go);
        }
        

        endTitle.text = (win ? "You won~" : "You lost :(");
        endEnemy.text = "Enemy : " + GlobalVars.Instance.selectedEnemy.ToString();

        string minutes = Mathf.FloorToInt(duration / 60).ToString();
        string seconds = "00" + ((int)(duration % 60)).ToString();
        endTime.text =  minutes + "m" + seconds.Substring(seconds.Length - 2) + "s";

        endWPM.text = ((int)(GlobalVars.Instance.wordsTyped / (duration / 60))).ToString() + " wpm";

        gc.LoadMenu(GlobalVars.Menu.GameOver);

        ////Debug.Log("Battle Ended");
    }

    public void updateSkills()
    {
        int i = 0;

        GameObject[] skillButtons = GameObject.FindGameObjectsWithTag("SkillButton");
        foreach (GameObject go in skillButtons.OrderByDescending(b => b.transform.position.y))
        {
            SkillButtonController sbc = go.GetComponent<SkillButtonController>();
            
            sbc.skillId = selectedSkillIds[i];
            sbc.updateSkill();
            i++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale == 0) return;

        if (playerHealth.text != GlobalVars.Instance.playerCurrentHealth.ToString()) {
            playerHealth.text = GlobalVars.Instance.playerCurrentHealth.ToString();
        }
        if (enemyHealth.text != GlobalVars.Instance.enemyCurrentHealth.ToString()) {
            enemyHealth.text = GlobalVars.Instance.enemyCurrentHealth.ToString();
        }

        foreach (GameObject go in GameObject.FindGameObjectsWithTag("Projectile"))
        {            
            Skill skill = go.GetComponent<Skill>();

            if (skill.job > 0) //Friendly 
            {

                MeshRenderer mr = go.GetComponent<MeshRenderer>();
                Material[] mats = new Material[] { mr.material };

                mr.materials = mats;

                float multiplier = GlobalEffects.Instance.applyEffect(GlobalEffects.EffectTarget.Friendly, GlobalEffects.EffectType.SpeedMultiplier, skill.job);

                go.transform.Translate(Vector3.right * projectileMoveSpeed * Time.deltaTime * multiplier * skill.moveSpeed);

                if (go.transform.position.x > playerProjectileHitAtX)
                {
                    go.SendMessage("onHit");
                    Destroy(go);
                }
            } 
            
            if (skill.enemy > 0)
            {
                float multiplier = GlobalEffects.Instance.applyEffect(GlobalEffects.EffectTarget.Enemy, GlobalEffects.EffectType.SpeedMultiplier, Job.None);

                go.transform.Translate(Vector3.left * projectileMoveSpeed * Time.deltaTime * multiplier);

                if (go.transform.position.x < enemyProjectileHitAtX)
                {
                    go.SendMessage("onHit");
                    Destroy(go);
                }

                TextMesh tm = go.GetComponent<TextMesh>();
                MeshRenderer mr = go.GetComponent<MeshRenderer>();
                Material[] mats = new Material[] { mr.material };
                string curr = GlobalVars.Instance.CurrentTypedString;


                if (skill.originalText.StartsWith(curr) && curr.Length > 0)
                {
                    //hidden words are untypable
                    if (go.transform.position.x <= enemyProjectileHitAtX + (Mathf.Abs(enemyProjectileHitAtX) + Mathf.Abs(enemyProjectileStartingX)) * skill.projectileVisiblePercentage)
                    {
                        //remove material

                        mr.materials = mats;
                        tm.text = "<color=red>" + curr + "</color>" + skill.originalText.Substring(curr.Length);
                    }                    
                } 
                else
                {
                    if (go.transform.position.x > enemyProjectileHitAtX + (Mathf.Abs(enemyProjectileHitAtX) + Mathf.Abs(enemyProjectileStartingX)) * skill.projectileVisiblePercentage)
                    {
                        tm.text = "<material=1>" + skill.originalText + "</material>";
                    }
                    else
                    {
                        //remove material
                        mr.materials = mats;
                        tm.text = skill.originalText;
                    }
                    
                }

                if (skill.damageMultiplier > 1)
                {
                    tm.text += " x" + skill.damageMultiplier;
                } 

                if (skill.originalText == curr)
                {
                    Destroy(go);
                    GlobalVars.Instance.CurrentTypedString = "";
                    GlobalVars.Instance.wordsTyped++;
                }
            }            
        }

        if (GlobalVars.Instance.playerCurrentHealth <= 0)
        {
            EndBattle(false);
        }
        if (GlobalVars.Instance.enemyCurrentHealth <= 0)
        {
            EndBattle(true);
        }
    }
}
