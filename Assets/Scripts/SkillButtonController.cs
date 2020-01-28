using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillButtonController : MonoBehaviour
{
    public string skillId;
    public Skill skill;
    private Button btn;
    private float cooldown;
    private BattleController bc;
    private Text txt;

    public Image icon;

    // Start is called before the first frame update
    void Start()
    {   
        btn = this.GetComponentInParent<Button>();
        txt = this.GetComponentInParent<Text>();
        bc = GameObject.Find("Controllers").GetComponent<BattleController>();
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
        float cooldownMultiplier = 1f;
        cooldownMultiplier *= GlobalBuffs.Instance.ongoingCooldownMultiplierBuff;

        cooldown = skill.cooldown * cooldownMultiplier;
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
        //on cooldown
        if (cooldown > 0)
        {
            cooldown -= Time.deltaTime;
            txt.enabled = false;
        }
        //first off cooldown
        else if (cooldown <= 0 && btn.interactable == false)
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
            //take in to account cost buffs when creating trigger word
        }
        //off cooldown
        else if (cooldown <= 0 && btn.interactable == true)
        {
            if (txt.text.ToLower().StartsWith(GlobalVars.Instance.CurrentTypedString.ToLower()) && GlobalVars.Instance.CurrentTypedString.Length > 0)
            {
                txt.color = Color.red;
            }
            else
            {
                txt.color = Color.black;
            }

            if (txt.text.ToLower() == GlobalVars.Instance.CurrentTypedString.ToLower())
            {
                CastSkill();
                GlobalVars.Instance.CurrentTypedString = "";
            }
        }
    }

    public void updateCost()
    {
        float wordCost = skill.wordCostSize;

        float costMultiplier = GlobalBuffs.Instance.getWordCostMultiplier(skill.job);
        wordCost *= costMultiplier;
        GlobalBuffs.Instance.resetCostMultiplier(skill.job);

        string randomWord = GlobalWords.Instance.getWordByLength((int)wordCost);

        txt.text = randomWord;
    }

    public void CastSkill()
    {
        skill = GlobalSkills.Instance.getSkillById(skillId);

        float cooldownMultiplier = 1f;
        cooldownMultiplier *= GlobalBuffs.Instance.ongoingCooldownMultiplierBuff;

        cooldown += skill.cooldown * cooldownMultiplier;
        btn.interactable = false;
        skill.onCast(txt.text, bc.projectileStartingX, bc.projectileStartingYMin, bc.projectileStartingYMax, bc.projectilePrefab, skill); 

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
    }
}
