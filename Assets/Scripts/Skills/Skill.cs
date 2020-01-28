using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Skill : MonoBehaviour {  
    public string skillId;
    public Job job;
    public SkillType skillType;
    public string skillName;
    public string description;
    public int wordCostSize = 0;
    public int wordDamageSize = 0;
    public int wordHealSize = 0;
    public int cooldown = 0;

    public int numProjectiles = 0;

    public int wordsBlocked = 0; 
    public int lettersBlocked = 0;

    public float moveSpeedBuff = 1f;

    #region Buffs
    #region WarriorBuffs
    public float warriorWordDamageSizeBuff = 1f;
    public float warriorWordCostSizeBuff = 1f;

    public float ongoingWarriorWordDamageSizeBuff = 1f;
    public float ongoingWarriorWordDamageSizeBuffDuration = 0f;

    public float ongoingWarriorWordCostSizeBuff = 1f;
    public float ongoingWarriorWordCostSizeBuffDuration = 0f;

    #endregion

    #region MageBuffs
    public float mageWordDamageSizeBuff = 1f;
    public float mageWordCostSizeBuff = 1f;

    public float ongoingMageWordDamageSizeBuff = 1f;
    public float ongoingMageWordDamageSizeBuffDuration = 0f;

    public float ongoingMageWordCostSizeBuff = 1f;
    public float ongoingMageWordCostSizeBuffDuration = 0f;

    #endregion

    #region RangerBuffs
    public float rangerWordDamageSizeBuff = 1f;
    public float rangerWordCostSizeBuff = 1f;

    public float ongoingRangerWordDamageSizeBuff = 1f;
    public float ongoingRangerWordDamageSizeBuffDuration = 0f;

    public float ongoingRangerWordCostSizeBuff = 1f;
    public float ongoingRangerWordCostSizeBuffDuration = 0f;

    #endregion

    #region ClericBuffs
    public float clericWordDamageSizeBuff = 1f;
    public float clericWordCostSizeBuff = 1f;

    public float ongoingClericWordDamageSizeBuff = 1f;
    public float ongoingClericWordDamageSizeBuffDuration = 0f;

    public float ongoingClericWordCostSizeBuff = 1f;
    public float ongoingClericWordCostSizeBuffDuration = 0f;

    #endregion

    #region BardBuffs
    public float bardWordDamageSizeBuff = 1f;
    public float bardWordCostSizeBuff = 1f;

    public float ongoingBardWordDamageSizeBuff = 1f;
    public float ongoingBardWordDamageSizeBuffDuration = 0f;

    public float ongoingBardWordCostSizeBuff = 1f;
    public float ongoingBardWordCostSizeBuffDuration = 0f;

    #endregion

    #region OngoingBuffs
    public float ongoingGlobalWordDamageSizeBuff = 1f;
    public float ongoingGlobalWordDamageSizeBuffDuration = 0f;

    public float ongoingGlobalWordCostSizeBuff = 1f;
    public float ongoingGlobalWordCostSizeBuffDuration = 0f;

    public float ongoingFriendlyWordSpeedBuff = 1f;
    public float ongoingFriendlyWordSpeedBuffDuration = 0f;

    public float ongoingEnemyWordSpeedBuff = 1f;
    public float ongoingEnemyWordSpeedBuffDuration = 0f;

    public float ongoingCooldownMultiplierBuff = 1f;
    public float ongoingCooldownMultiplierBuffDuration = 0f;
    #endregion

    #region One-time Buffs
    public float globalWordDamageSizeBuff = 1f;
    public float globalWordCostSizeBuff = 1f;

    #endregion
    #endregion

    public int duration = 0;
    public bool applyOngoingOnHit = false;
    public bool applyOngoingOnCast = false;
    
    public float ongoingHealSize = 0f;
    public float ongoingDamageSize = 0f;
    //public float ongoingCostSizeBuff = 0f;

    public double projectileVisiblePercentage = 0;

    public int friendlyTypingFreezeDuration = 0;
    public int enemyTypingFreezeDuration = 0;

    public string shareCooldownCode = "";

    #region Multis
    public float damageMultiplier = 1f;
    #endregion

    public void onCast(string word, float startingX, float startingYMin, float startingYMax, GameObject prefab, Skill skill) {
        Debug.Log("Skill - Cast - " + skillName);
        //foreach num projectiles, create object with size etc
        if (wordHealSize > 0) {
            Debug.Log("Skill - Cast - " + skillName + " - Heal " + wordHealSize + " hp");
            GlobalVars.Instance.playerCurrentHealth += wordHealSize;
            if (GlobalVars.Instance.playerCurrentHealth > GlobalVars.Instance.playerMaxHealth) {
                GlobalVars.Instance.playerCurrentHealth = GlobalVars.Instance.playerMaxHealth;
            }
        }
        if (wordsBlocked > 0)
        {
            Debug.Log("Skill - Cast - " + skillName + " - Block " + wordsBlocked + " words");
            GlobalVars.Instance.playerWordBlock += wordsBlocked;
            Text t = GameObject.Find("PlayerWordBlock").GetComponent<Text>();
            t.text = "W:" + GlobalVars.Instance.playerWordBlock.ToString();

        }
        if (lettersBlocked > 0)
        {
            Debug.Log("Skill - Cast - " + skillName + " - Block " + lettersBlocked + " letters");
            GlobalVars.Instance.playerLetterBlock += lettersBlocked;
            Text t = GameObject.Find("PlayerLetterBlock").GetComponent<Text>();
            t.text = "L:" + GlobalVars.Instance.playerLetterBlock.ToString();
        }

        for (int i = 0; i < numProjectiles; i++)
        {
            Vector3 startingPosition = new Vector3(startingX, Random.Range(startingYMin, startingYMax), 0);

            GameObject projectile = Instantiate(prefab, startingPosition, Quaternion.identity);
            projectile.name = skillName + "Projectile";
            TextMesh theText = projectile.GetComponent<TextMesh>();
            Skill newSkill = projectile.GetComponent<Skill>();
            copySkill(skill, newSkill);
            newSkill.damageMultiplier = GlobalBuffs.Instance.getWordDamageSizeMultiplier(job);
            string damageWord = (skill.wordCostSize == skill.wordDamageSize ? word : GlobalWords.Instance.getWordByLength(skill.wordDamageSize));
            theText.text = damageWord + " - x" + newSkill.damageMultiplier;
            GlobalBuffs.Instance.resetDamageMultiplier(job);

            //add damage buffs when calculating real projectile
        }

        if (warriorWordDamageSizeBuff != 1f)
        {
            GlobalBuffs.Instance.warriorWordDamageSizeBuff *= warriorWordDamageSizeBuff;
        }
        if (warriorWordCostSizeBuff != 1f)
        {
            GlobalBuffs.Instance.warriorWordCostSizeBuff *= warriorWordCostSizeBuff;
        }
        if (mageWordDamageSizeBuff != 1f)
        {
            GlobalBuffs.Instance.mageWordDamageSizeBuff *= mageWordDamageSizeBuff;
        }
        if (mageWordCostSizeBuff != 1f)
        {
            GlobalBuffs.Instance.mageWordCostSizeBuff *= mageWordCostSizeBuff;
        }
        if (rangerWordDamageSizeBuff != 1f)
        {
            GlobalBuffs.Instance.rangerWordDamageSizeBuff *= rangerWordDamageSizeBuff;
        }
        if (rangerWordCostSizeBuff != 1f)
        {
            GlobalBuffs.Instance.rangerWordCostSizeBuff *= rangerWordCostSizeBuff;
        }
        if (clericWordDamageSizeBuff != 1f)
        {
            GlobalBuffs.Instance.clericWordDamageSizeBuff *= clericWordDamageSizeBuff;
        }
        if (clericWordCostSizeBuff != 1f)
        {
            GlobalBuffs.Instance.clericWordCostSizeBuff *= clericWordCostSizeBuff;
        }
        if (bardWordDamageSizeBuff != 1f)
        {
            GlobalBuffs.Instance.bardWordDamageSizeBuff *= bardWordDamageSizeBuff;
        }
        if (bardWordCostSizeBuff != 1f)
        {
            GlobalBuffs.Instance.bardWordCostSizeBuff *= bardWordCostSizeBuff;
        }
        if (globalWordDamageSizeBuff != 1f) {
            GlobalBuffs.Instance.globalWordDamageSizeBuff *= globalWordDamageSizeBuff;
        }
        if (globalWordCostSizeBuff != 1f)        {
            GlobalBuffs.Instance.globalWordCostSizeBuff *= globalWordCostSizeBuff;
        }
        if (friendlyTypingFreezeDuration > 0)
        {
            GlobalBuffs.Instance.friendlyTypingFreezeDuration = friendlyTypingFreezeDuration;
        }
        if (enemyTypingFreezeDuration > 0)
        {
            GlobalBuffs.Instance.enemyTypingFreezeDuration = enemyTypingFreezeDuration;
        }

        if (applyOngoingOnCast)
        {
            if (ongoingWarriorWordDamageSizeBuff != 1f)
            {
                GlobalBuffs.Instance.ongoingWarriorWordDamageSizeBuff *= ongoingWarriorWordDamageSizeBuff;
                GlobalBuffs.Instance.ongoingWarriorWordDamageSizeBuffDuration += duration;
            }
            if (ongoingWarriorWordCostSizeBuff != 1f)
            {
                GlobalBuffs.Instance.ongoingWarriorWordCostSizeBuff *= ongoingWarriorWordCostSizeBuff;
                GlobalBuffs.Instance.ongoingWarriorWordCostSizeBuffDuration += duration;
            }
            if (ongoingMageWordDamageSizeBuff != 1f)
            {
                GlobalBuffs.Instance.ongoingMageWordDamageSizeBuff *= ongoingMageWordDamageSizeBuff;
                GlobalBuffs.Instance.ongoingMageWordDamageSizeBuffDuration += duration;
            }
            if (ongoingMageWordCostSizeBuff != 1f)
            {
                GlobalBuffs.Instance.ongoingMageWordCostSizeBuff *= ongoingMageWordCostSizeBuff;
                GlobalBuffs.Instance.ongoingMageWordCostSizeBuffDuration += duration;
            }
            if (ongoingRangerWordDamageSizeBuff != 1f)
            {
                GlobalBuffs.Instance.ongoingRangerWordDamageSizeBuff *= ongoingRangerWordDamageSizeBuff;
                GlobalBuffs.Instance.ongoingRangerWordDamageSizeBuffDuration += duration;
            }
            if (ongoingRangerWordCostSizeBuff != 1f)
            {
                GlobalBuffs.Instance.ongoingRangerWordCostSizeBuff *= ongoingRangerWordCostSizeBuff;
                GlobalBuffs.Instance.ongoingRangerWordCostSizeBuffDuration += duration;
            }
            if (ongoingClericWordDamageSizeBuff != 1f)
            {
                GlobalBuffs.Instance.ongoingClericWordDamageSizeBuff *= ongoingClericWordDamageSizeBuff;
                GlobalBuffs.Instance.ongoingClericWordDamageSizeBuffDuration += duration;
            }
            if (ongoingClericWordCostSizeBuff != 1f)
            {
                GlobalBuffs.Instance.ongoingClericWordCostSizeBuff *= ongoingClericWordCostSizeBuff;
                GlobalBuffs.Instance.ongoingClericWordCostSizeBuffDuration += duration;
            }
            if (ongoingBardWordDamageSizeBuff != 1f)
            {
                GlobalBuffs.Instance.ongoingBardWordDamageSizeBuff *= ongoingBardWordDamageSizeBuff;
                GlobalBuffs.Instance.ongoingBardWordDamageSizeBuffDuration += duration;
            }
            if (ongoingBardWordCostSizeBuff != 1f)
            {
                GlobalBuffs.Instance.ongoingBardWordCostSizeBuff *= ongoingBardWordCostSizeBuff;
                GlobalBuffs.Instance.ongoingBardWordCostSizeBuffDuration += duration;
            }

            if (ongoingGlobalWordDamageSizeBuff != 1f)
            {
                GlobalBuffs.Instance.ongoingGlobalWordDamageSizeBuff *= ongoingGlobalWordDamageSizeBuff;
                GlobalBuffs.Instance.ongoingGlobalWordDamageSizeBuffDuration += duration;
            }
            if (ongoingGlobalWordCostSizeBuff != 1f)
            {
                GlobalBuffs.Instance.ongoingGlobalWordCostSizeBuff *= ongoingGlobalWordCostSizeBuff;
                GlobalBuffs.Instance.ongoingGlobalWordCostSizeBuffDuration += duration;
            }
            if (ongoingFriendlyWordSpeedBuff != 1f)
            {
                GlobalBuffs.Instance.ongoingFriendlyWordSpeedBuff *= ongoingFriendlyWordSpeedBuff;
                GlobalBuffs.Instance.ongoingFriendlyWordSpeedBuffDuration += duration;
            }
            if (ongoingEnemyWordSpeedBuff != 1f)
            {
                GlobalBuffs.Instance.ongoingEnemyWordSpeedBuff *= ongoingEnemyWordSpeedBuff;
                GlobalBuffs.Instance.ongoingEnemyWordSpeedBuffDuration += duration;
            }
            if (ongoingCooldownMultiplierBuff != 1f)
            {
                GlobalBuffs.Instance.ongoingCooldownMultiplierBuff *= ongoingCooldownMultiplierBuff;
                GlobalBuffs.Instance.ongoingCooldownMultiplierBuffDuration += duration;
            }

            applyEffect();
        }
        
        //update costs
        if (bardWordCostSizeBuff != 1f || clericWordCostSizeBuff != 1f || globalWordCostSizeBuff != 1f || mageWordCostSizeBuff != 1f || ongoingBardWordCostSizeBuff != 1f || ongoingClericWordCostSizeBuff != 1f ||
            ongoingGlobalWordCostSizeBuff != 1f || ongoingMageWordCostSizeBuff != 1f || ongoingRangerWordCostSizeBuff != 1f || ongoingWarriorWordCostSizeBuff != 1f || 
            rangerWordCostSizeBuff != 1f || warriorWordCostSizeBuff != 1f)
        {
            updateCosts();
        }

    }

    private void updateCosts()
    {
        GameObject[] btns = GameObject.FindGameObjectsWithTag("SkillButton");

        foreach (GameObject btn in btns)
        {
            SkillButtonController sbc = btn.GetComponent<SkillButtonController>();

            switch (sbc.skill.job)
            {
                case Job.Warrior:
                    if (ongoingWarriorWordCostSizeBuff != 1f || warriorWordCostSizeBuff != 1f) {
                        sbc.updateCost();
                    }
                    break;
                case Job.Mage:
                    if (ongoingMageWordCostSizeBuff != 1f || mageWordCostSizeBuff != 1f)
                    {
                        sbc.updateCost();
                    }
                    break;
                case Job.Ranger:
                    if (ongoingRangerWordCostSizeBuff != 1f || rangerWordCostSizeBuff != 1f)
                    {
                        sbc.updateCost();
                    }
                    break;
                case Job.Cleric:
                    if (ongoingClericWordCostSizeBuff != 1f || clericWordCostSizeBuff != 1f)
                    {
                        sbc.updateCost();
                    }
                    break;
                case Job.Bard:
                    if (ongoingBardWordCostSizeBuff != 1f || bardWordCostSizeBuff != 1f)
                    {
                        sbc.updateCost();
                    }
                    break;
                default:
                    break;
            }         
            
            if (globalWordCostSizeBuff != 1f || ongoingGlobalWordCostSizeBuff != 1f)
            {
                sbc.updateCost();
            }
        }
    }

    private void applyEffect()
    {
        if (ongoingDamageSize > 0f || ongoingHealSize > 0f)
        {
            GlobalEffects.Effect effect = new GlobalEffects.Effect();
            effect.name = skillName;
            effect.damage = ongoingDamageSize;
            effect.healing = ongoingHealSize;
            effect.duration = duration;

            GlobalEffects.Instance.effects.Add(effect);
        }
    }

    public void onHit()
    {
        GlobalVars.Instance.enemyCurrentHealth -= (int)(wordDamageSize * damageMultiplier);

        Debug.Log("Skill - Hit - " + skillName + " - Deal " + ((int)(wordDamageSize * damageMultiplier)).ToString() + " damage");

        if (applyOngoingOnHit)
        {
            applyEffect();
        }

    }

    void copySkill(Skill from, Skill to)
    {
        to.job = from.job;
        to.skillType = from.skillType;
        to.skillName = from.skillName;
        to.description = from.description;
        to.wordCostSize = from.wordCostSize;
        to.wordDamageSize = from.wordDamageSize;
        to.wordHealSize = from.wordHealSize;
        to.cooldown = from.cooldown;
        to.numProjectiles = from.numProjectiles;
        to.wordsBlocked = from.wordsBlocked;
        to.lettersBlocked = from.lettersBlocked;

        to.duration = from.duration;
        to.applyOngoingOnHit = from.applyOngoingOnHit;
        to.applyOngoingOnCast = from.applyOngoingOnCast;

        to.warriorWordDamageSizeBuff = from.warriorWordDamageSizeBuff;
        to.warriorWordCostSizeBuff = from.warriorWordCostSizeBuff;

        to.mageWordDamageSizeBuff = from.mageWordDamageSizeBuff;
        to.mageWordCostSizeBuff = from.mageWordCostSizeBuff;
        
        to.rangerWordDamageSizeBuff = from.rangerWordDamageSizeBuff;
        to.rangerWordCostSizeBuff = from.rangerWordCostSizeBuff;
        
        to.clericWordDamageSizeBuff = from.clericWordDamageSizeBuff;
        to.clericWordCostSizeBuff = from.clericWordCostSizeBuff;
        
        to.bardWordDamageSizeBuff = from.bardWordDamageSizeBuff;
        to.bardWordCostSizeBuff = from.bardWordCostSizeBuff;
        
        to.globalWordDamageSizeBuff = from.globalWordDamageSizeBuff;
        to.globalWordCostSizeBuff = from.globalWordCostSizeBuff;

        to.globalWordDamageSizeBuff = from.globalWordDamageSizeBuff;
        to.globalWordCostSizeBuff = from.globalWordCostSizeBuff;

        to.ongoingFriendlyWordSpeedBuff = from.ongoingFriendlyWordSpeedBuff;
        to.ongoingEnemyWordSpeedBuff = from.ongoingEnemyWordSpeedBuff;
        to.ongoingCooldownMultiplierBuff = from.ongoingCooldownMultiplierBuff;

        to.moveSpeedBuff = from.moveSpeedBuff;

        to.ongoingWarriorWordDamageSizeBuff = from.ongoingWarriorWordDamageSizeBuff;
        to.ongoingWarriorWordCostSizeBuff = from.ongoingWarriorWordCostSizeBuff;
        to.ongoingMageWordDamageSizeBuff = from.ongoingMageWordDamageSizeBuff;
        to.ongoingMageWordCostSizeBuff = from.ongoingMageWordCostSizeBuff;
        to.ongoingRangerWordDamageSizeBuff = from.ongoingRangerWordDamageSizeBuff;
        to.ongoingRangerWordCostSizeBuff = from.ongoingRangerWordCostSizeBuff;
        to.ongoingClericWordDamageSizeBuff = from.ongoingClericWordDamageSizeBuff;
        to.ongoingClericWordCostSizeBuff = from.ongoingClericWordCostSizeBuff;
        to.ongoingBardWordDamageSizeBuff = from.ongoingBardWordDamageSizeBuff;
        to.ongoingBardWordCostSizeBuff = from.ongoingBardWordCostSizeBuff;


        to.ongoingGlobalWordDamageSizeBuff = from.ongoingGlobalWordDamageSizeBuff;
        to.ongoingGlobalWordCostSizeBuff = from.ongoingGlobalWordCostSizeBuff;

        to.ongoingDamageSize = from.ongoingDamageSize;
        to.ongoingHealSize = from.ongoingHealSize;

        to.projectileVisiblePercentage = from.projectileVisiblePercentage;
        to.friendlyTypingFreezeDuration = from.friendlyTypingFreezeDuration;
        to.enemyTypingFreezeDuration = from.enemyTypingFreezeDuration;
}

}

public enum Job 
{
    All = 0,
    Warrior = 1,
    Mage = 2,
    Ranger = 3,
    Cleric = 4,
    Bard = 5,
}

public enum SkillType
{
    Attack = 0,
    Block = 1,
    Heal = 2,
    Buff = 3,
    Hybrid = 4,
}