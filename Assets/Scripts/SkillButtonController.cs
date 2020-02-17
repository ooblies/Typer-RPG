using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SkillButtonController : MonoBehaviour
{
    public string skillId;
    public Skill skill;
    public Image cooldownImage;
    private Button btn;
    public float cooldown;
    private BattleController bc;
    public Text txt;
    private string originalText;
    

    public Image icon;

    // Start is called before the first frame update
    void Start()
    {   
    }
    private void Awake()
    {

        btn = this.GetComponentInParent<Button>();
        txt = this.GetComponentInParent<Text>();
        bc = GameObject.Find("Controllers").GetComponent<BattleController>();

        txt.supportRichText = true;
        originalText = txt.text;
    }

    public void updateSkill()
    {
        if (skillId.Length > 0)
        {
            skill = GlobalSkills.Instance.getSkillById(skillId);
            string path = "Icons/" + skill.skillId;

            Texture2D text = Resources.Load<Texture2D>(path);

            if (text == null)
            {
                //missing icon
                text = Resources.Load<Texture2D>("Icons/temp");
            }

            Rect rect = new Rect(0.0f, 0.0f, text.width, text.height);
            Vector2 vec2 = new Vector2(0.5f, 0.5f);

            icon.sprite = Sprite.Create(text, rect, vec2, 32);
        }        
    }

    public void ActivateCooldown()
    {
        if (skill.job > 0)
        {
            float cooldownMultiplier = GlobalEffects.Instance.applyEffect(GlobalEffects.EffectTarget.Friendly, GlobalEffects.EffectType.CooldownMultiplier, skill.job);
            cooldown = skill.cooldown * cooldownMultiplier;
        }
        if (skill.enemy > 0)
        {
            float cooldownMultiplier = GlobalEffects.Instance.applyEffect(GlobalEffects.EffectTarget.Enemy, GlobalEffects.EffectType.CooldownMultiplier, Job.None);
            cooldown = skill.cooldown * cooldownMultiplier;
        }                
    }
    public void ClearCooldown()
    {
        cooldown = 0;
        btn.interactable = true;
        txt.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale == 0) return;

        //on cooldown
        if (cooldown > 0)
        {
            if (!GlobalEffects.Instance.isFrozen(skill.job > 0 ? GlobalEffects.EffectTarget.Friendly : GlobalEffects.EffectTarget.Enemy))
            {
                float cooldownMultiplier = 1f;
                if (skill.job > 0)
                {
                    cooldownMultiplier = GlobalEffects.Instance.applyEffect(GlobalEffects.EffectTarget.Friendly, GlobalEffects.EffectType.CooldownMultiplier, skill.job);                    
                }
                if (skill.enemy > 0)
                {
                    cooldownMultiplier = GlobalEffects.Instance.applyEffect(GlobalEffects.EffectTarget.Enemy, GlobalEffects.EffectType.CooldownMultiplier, Job.None);                    
                }

                cooldown -= Time.deltaTime;
                txt.enabled = false;
                cooldownImage.fillAmount = cooldown / (skill.cooldown * cooldownMultiplier);
            }            
        }

        if (skill.job > 0)
        {
            //first off cooldown
            if (cooldown <= 0 && btn.interactable == false)
            {
                updateCost();

                if (skill.shareCooldownCode.Length == 0)
                {
                    ClearCooldown();
                }
                else
                {
                    GameObject[] objects = GameObject.FindGameObjectsWithTag("SkillButton");

                    foreach (GameObject go in objects)
                    {
                        SkillButtonController sbc = go.GetComponent<SkillButtonController>();
                        if (sbc.skill.shareCooldownCode == skill.shareCooldownCode)
                        {
                            sbc.updateCost();
                            sbc.ClearCooldown();
                        }
                    }
                }
            }
            //off cooldown
            else if (cooldown <= 0 && btn.interactable == true)
            {
                if (originalText.StartsWith(GlobalVars.Instance.CurrentTypedString.ToLower()) && GlobalVars.Instance.CurrentTypedString.Length > 0)
                {
                    string matchedText = GlobalVars.Instance.CurrentTypedString.ToLower();
                    string unmatchedText = originalText.Substring(matchedText.Length, originalText.Length - matchedText.Length).ToLower();
                    txt.text = "<color=red>" + matchedText + "</color>" + unmatchedText;
                }
                else
                {
                    txt.text = originalText;
                }

                if (originalText.ToLower() == GlobalVars.Instance.CurrentTypedString.ToLower())
                {
                    CastSkill();
                    GlobalVars.Instance.CurrentTypedString = "";
                    GlobalVars.Instance.wordsTyped++;
                }
            }
        }
        if (skill.enemy > 0)
        {
            if (cooldown <= 0)
            {
                if (skill.shareCooldownCode.Length > 0)
                {
                    //randomly cast a shared skill
                    List<SkillButtonController> witchSkills = GameObject.FindGameObjectsWithTag("EnemyButton").Select(x => x.GetComponent<SkillButtonController>()).Where(x => x.skill.shareCooldownCode == skill.shareCooldownCode).ToList();

                    int randomIndex = Random.Range(0, witchSkills.Count);

                    witchSkills[randomIndex].CastSkill();                                            
                }
                else
                {
                    CastSkill();
                }
            }            
        }        
    }

    public void updateCost()
    {
        float wordCost = skill.wordCostSize;

        float costMultiplier = GlobalEffects.Instance.checkEffect(GlobalEffects.EffectTarget.Friendly, GlobalEffects.EffectType.CostMultiplier, skill.job);

        wordCost *= costMultiplier;

        string randomWord = GlobalWords.Instance.getWordByLength((int)wordCost);

        txt.text = randomWord;
        originalText = txt.text;
    }

    public void CastSkill()
    {
        if (skill.job > 0)
        {
            btn.interactable = false;
            skill.onCast(originalText, bc.playerProjectileStartingX, bc.projectileStartingYMin, bc.projectileStartingYMax, bc.projectilePrefab);

            if (skill.shareCooldownCode.Length > 0)
            {
                GameObject[] objects = GameObject.FindGameObjectsWithTag("SkillButton");

                foreach (GameObject go in objects)
                {
                    SkillButtonController sbc = go.GetComponent<SkillButtonController>();
                    if (sbc.skill.shareCooldownCode == skill.shareCooldownCode)
                    {
                        sbc.ActivateCooldown();
                    }
                }
            }
            else
            {
                ActivateCooldown();
            }
        }
        if (skill.enemy > 0)
        {
            skill.onCast(GlobalWords.Instance.getWordByLength(Mathf.Clamp(skill.wordDamageSize,1,1000)), bc.enemyProjectileStartingX, bc.projectileStartingYMin, bc.projectileStartingYMax, bc.projectilePrefab);

            if (skill.shareCooldownCode.Length > 0)
            {
                GameObject[] objects = GameObject.FindGameObjectsWithTag("EnemyButton");

                foreach (GameObject go in objects)
                {
                    SkillButtonController sbc = go.GetComponent<SkillButtonController>();
                    if (sbc.skill.shareCooldownCode == skill.shareCooldownCode)
                    {
                        sbc.ActivateCooldown();
                    }
                }
            }
            else
            {
                ActivateCooldown();
            }
        }
    }
}
