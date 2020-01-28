using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalBuffs : Singleton<GlobalBuffs>
{
    public List<Buff> buffs = new List<Buff>();

    public class Buff
    {
        public Job affectedJob;
        public BuffType buffType;
        public BuffTarget buffTarget;
        
        public string sourceName;
        
        public bool ongoing;
        public float duration;
        
        public int effectCount;
        
        public float effectMultiplier;
    }

    public string printBuffList()
    {
        string list = "";

        foreach (Buff buff in buffs)
        {
            string printBuff = "";

            if (buff.ongoing)
            {
                printBuff = buff.sourceName + " - " + buff.buffTarget.ToString() + " " + buff.buffType.ToString() + " x" + buff.effectMultiplier + " - " + buff.duration + "s\n";
            }
            else
            {
                printBuff = buff.sourceName + " - " + buff.buffTarget.ToString() + " " + buff.buffType.ToString() + " x" + buff.effectMultiplier + " - " + buff.effectCount + " times\n";
            }

            list += printBuff;            
        }

        return list;
    }

    public enum BuffType
    {
        Damage = 0,
        Cost = 1,
        Cooldown = 2,
        Speed = 3,    
        Freeze = 4,
    }

    public enum BuffTarget
    {
        Friendly = 0,
        Enemy = 1,
    }

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

    public float friendlyTypingFreezeDuration = 0f;
    public float enemyTypingFreezeDuration = 0f;
    #endregion

    #region One-time Buffs
    public float globalWordDamageSizeBuff = 1f;
    public float globalWordCostSizeBuff = 1f;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
    
    }


    public void resetDamageMultiplier(Job job)
    {
        globalWordDamageSizeBuff = 1f;
        switch (job)
        {
            case Job.Warrior:
                warriorWordDamageSizeBuff = 1f;
                break;
            case Job.Mage:
                mageWordDamageSizeBuff = 1f;
                break;
            case Job.Ranger:
                rangerWordDamageSizeBuff = 1f;
                break;
            case Job.Cleric:
                clericWordDamageSizeBuff = 1f;
                break;
            case Job.Bard:
                bardWordDamageSizeBuff = 1f;
                break;
            default:
                break;
        }
    }

    public void resetCostMultiplier(Job job)
    {
        globalWordCostSizeBuff = 1f;
        switch (job)
        {
            case Job.Warrior:
                warriorWordCostSizeBuff = 1f;
                break;
            case Job.Mage:
                mageWordCostSizeBuff = 1f;
                break;
            case Job.Ranger:
                rangerWordCostSizeBuff = 1f;
                break;
            case Job.Cleric:
                clericWordCostSizeBuff = 1f;
                break;
            case Job.Bard:
                bardWordCostSizeBuff = 1f;
                break;
            default:
                break;
        }

    }

    public float getWordCostMultiplier(Job job)
    {
        float multi = 1f;

        multi = multi * globalWordCostSizeBuff * ongoingGlobalWordCostSizeBuff * 
            (job == Job.Warrior ? ongoingWarriorWordCostSizeBuff * warriorWordCostSizeBuff : 
             job == Job.Mage ? ongoingMageWordCostSizeBuff * mageWordCostSizeBuff : 
             job == Job.Ranger ? ongoingRangerWordCostSizeBuff * rangerWordCostSizeBuff : 
             job == Job.Cleric ? ongoingClericWordCostSizeBuff * clericWordCostSizeBuff : 
             job == Job.Bard ? ongoingBardWordCostSizeBuff * bardWordCostSizeBuff : 1);

        return multi;
    }

    public float getWordDamageSizeMultiplier(Job job)
    {
        float multi = 1f;
        
        multi = multi * globalWordDamageSizeBuff * ongoingGlobalWordDamageSizeBuff *
            (job == Job.Warrior ? ongoingWarriorWordDamageSizeBuff * warriorWordDamageSizeBuff :
             job == Job.Mage ? ongoingMageWordDamageSizeBuff * mageWordDamageSizeBuff :
             job == Job.Ranger ? ongoingRangerWordDamageSizeBuff * rangerWordDamageSizeBuff :
             job == Job.Cleric ? ongoingClericWordDamageSizeBuff * clericWordDamageSizeBuff :
             job == Job.Bard ? ongoingBardWordDamageSizeBuff * bardWordDamageSizeBuff : 1);

        return multi;
    }

    // Update is called once per frame
    void Update()
    {
        if (ongoingWarriorWordDamageSizeBuffDuration > 0)
        {
            ongoingWarriorWordDamageSizeBuffDuration -= Time.deltaTime;
        } else if (ongoingWarriorWordDamageSizeBuff != 1f)
        {
            ongoingWarriorWordDamageSizeBuff = 1f;
        }

        if (ongoingWarriorWordCostSizeBuffDuration > 0)
        {
            ongoingWarriorWordCostSizeBuffDuration -= Time.deltaTime;
        }
        else if (ongoingWarriorWordCostSizeBuff != 1f)
        {
            ongoingWarriorWordCostSizeBuff = 1f;
        }

        if (ongoingGlobalWordDamageSizeBuffDuration > 0)
        {
            ongoingGlobalWordDamageSizeBuffDuration -= Time.deltaTime;
        }
        else if (ongoingGlobalWordDamageSizeBuff != 1f)
        {
            ongoingGlobalWordDamageSizeBuff = 1f;
        }

        if (ongoingGlobalWordCostSizeBuffDuration > 0)
        {
            ongoingGlobalWordCostSizeBuffDuration -= Time.deltaTime;
        }
        else if (ongoingGlobalWordCostSizeBuff != 1f)
        {
            ongoingGlobalWordCostSizeBuff = 1f;
        }

        if (ongoingFriendlyWordSpeedBuffDuration > 0)
        {
            ongoingFriendlyWordSpeedBuffDuration -= Time.deltaTime;
        }
        else if (ongoingFriendlyWordSpeedBuff != 1f)
        {
            ongoingFriendlyWordSpeedBuff = 1f;
        }

        if (ongoingEnemyWordSpeedBuffDuration > 0)
        {
            ongoingEnemyWordSpeedBuffDuration -= Time.deltaTime;
        }
        else if (ongoingEnemyWordSpeedBuff != 1f)
        {
            ongoingEnemyWordSpeedBuff = 1f;
        }

        if (ongoingCooldownMultiplierBuffDuration > 0)
        {
            ongoingCooldownMultiplierBuffDuration -= Time.deltaTime;
        }
        else if (ongoingCooldownMultiplierBuff != 1f)
        {
            ongoingCooldownMultiplierBuff = 1f;
        }

        if (friendlyTypingFreezeDuration > 0)
        {
            friendlyTypingFreezeDuration -= Time.deltaTime;
        }

        if (enemyTypingFreezeDuration > 0)
        {
            enemyTypingFreezeDuration -= Time.deltaTime;
        }
    }
}
