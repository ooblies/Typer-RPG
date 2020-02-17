using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Skill : MonoBehaviour {      
    public string skillId;
    public int orderBy;
    public Job job;
    public Enemy enemy;
    public SkillType skillType;
    public string skillName;
    public string description;
    public int wordCostSize = 0;
    public int wordDamageSize = 0;
    public int wordHealSize = 0;
    public int cooldown = 0;

    public int numProjectiles = 0;

    public float moveSpeed = 1f;


    public GlobalEffects.EffectType effectType;
    public GlobalEffects.EffectTarget effectTarget;
    public Job effectTargetJob;
    public float effect = 1f;
    public int effectCount = 0;
    public float effectDuration = 0f;
    public bool ongoingEffect = false;


    public int duration = 0;
    public bool applyEffectOnHit = false;
    public bool applyEffectOnCast = false;

    public double projectileVisiblePercentage = 1;


    public string shareCooldownCode = "";
    public string originalText = "";

    #region Multis
    public float damageMultiplier = 1f;
    #endregion

    public void onCast(string word, float startingX, float startingYMin, float startingYMax, GameObject prefab) {
        //Debug.Log("Skill - Cast - " + skillName);
        //foreach num projectiles, create object with size etc
        if (wordHealSize > 0) {
            //Debug.Log("Skill - Cast - " + skillName + " - Heal " + wordHealSize + " hp");
            GlobalVars.Instance.playerCurrentHealth += wordHealSize;
            if (GlobalVars.Instance.playerCurrentHealth > GlobalVars.Instance.playerMaxHealth) {
                GlobalVars.Instance.playerCurrentHealth = GlobalVars.Instance.playerMaxHealth;
            }
        }

        List<GameObject> createdProjectiles = new List<GameObject>();

        for (int i = 0; i < numProjectiles; i++)
        {
            float y = 0f;

            bool overlap = true;            

            while (overlap)
            {
                overlap = false;
                y = Random.Range(startingYMin, startingYMax);

                foreach (GameObject ep in createdProjectiles)
                {
                    if (Mathf.Abs(Mathf.Abs(ep.transform.position.y) - Mathf.Abs(y)) < 0.25f)
                    {
                        overlap = true;
                    }
                }
            }


            Vector3 startingPosition = new Vector3(startingX, y, 0);
            GameObject projectile = Instantiate(prefab, startingPosition, Quaternion.identity);
            projectile.name = skillName + "Projectile";
            TextMesh theText = projectile.GetComponent<TextMesh>();
            Skill newSkill = projectile.GetComponent<Skill>();
            originalText = word;
            copySkill(newSkill);
            newSkill.damageMultiplier = GlobalEffects.Instance.applyEffect(job > 0 ? GlobalEffects.EffectTarget.Friendly : GlobalEffects.EffectTarget.Enemy, GlobalEffects.EffectType.DamageMultiplier, job) * damageMultiplier;
            string damageWord = (wordCostSize == wordDamageSize ? word : GlobalWords.Instance.getWordByLength(wordDamageSize));
            theText.text = damageWord;
            if (enemy > 0)
            {
                theText.text = GlobalWords.Instance.getWordByLength(damageWord.Length);
                newSkill.originalText = theText.text;
            }
            if (newSkill.damageMultiplier > 1)
            {
                theText.text += " - x" + newSkill.damageMultiplier;
            }

            createdProjectiles.Add(projectile);
        }        

        //use cost effect
        float temp = GlobalEffects.Instance.applyEffect(GlobalEffects.EffectTarget.Friendly, GlobalEffects.EffectType.CostMultiplier, effectTargetJob);

        if (effectType == GlobalEffects.EffectType.RemoveWords)
        {
            GameObject[] projs = GameObject.FindGameObjectsWithTag("Projectile");
            foreach (GameObject proj in projs) {
                Skill projSkill = proj.GetComponent<Skill>();
                if ((projSkill.job > 0 && enemy > 0) || (job > 0 && projSkill.enemy > 0))
                {
                    Destroy(proj);
                }
            }
        }
       

        if (applyEffectOnCast)
        {
            applyEffect();           
        }
        
        //update costs
        if (skillType == SkillType.Effect && effectType == GlobalEffects.EffectType.CostMultiplier)
        {
            GlobalEffects.Instance.updateCosts(effectTargetJob);
        }

    }


    private void applyEffect()
    {
        if (skillType == SkillType.Effect)
        {
            GlobalEffects.Effect eff = new GlobalEffects.Effect
            {
                effectType = effectType,
                effectTarget = effectTarget,
                affectedJob = effectTargetJob,
                ongoing = ongoingEffect,
                effect = effect,
                effectCount = effectCount,
                duration = effectDuration,
                sourceName = skillName,
            };

            GlobalEffects.Instance.addEffect(eff);
        }

    }

    public void onHit()
    {
        float damage = 0.0f;
        int size = wordDamageSize;

        if (GlobalEffects.Instance.applyWordBlock(job > 0 ? GlobalEffects.EffectTarget.Enemy : GlobalEffects.EffectTarget.Friendly))
        {
            Debug.Log("Skill - Block - " + skillName);
            return;
        }

        int lBlock = GlobalEffects.Instance.applyLettersBlocked(job > 0 ? GlobalEffects.EffectTarget.Enemy : GlobalEffects.EffectTarget.Friendly, size);


        if (lBlock > 0)
        {
            Debug.Log("Skill - Block - " + skillName + " - Blocked " + lBlock.ToString() + " of " + size.ToString() + " letters");
        }

        if (lBlock < size)
        {
            damage = ((size - lBlock) * damageMultiplier);
            damage *= GlobalEffects.Instance.applyEffect(effectTarget, effectType, job);

            if (job > 0)
            {
                GlobalVars.Instance.enemyCurrentHealth -= (int)damage;

                Debug.Log("Skill - Hit - " + skillName + " - Deal " + ((int)damage).ToString() + " damage");

                if (applyEffectOnHit)
                {
                    applyEffect();
                }
            }
            if (enemy > 0)
            {
                GlobalVars.Instance.playerCurrentHealth -= (int)damage;

                Debug.Log("Skill - Hit - " + skillName + " - Deal " + ((int)damage).ToString() + " damage");

                if (applyEffectOnHit)
                {
                    applyEffect();
                }
            }
        }
    }

    void copySkill(Skill newSkill)
    {
        newSkill.shareCooldownCode = shareCooldownCode;

        newSkill.job = job;
        newSkill.enemy = enemy;
        newSkill.skillId = skillId;
        newSkill.orderBy = orderBy;
        newSkill.skillType = skillType;
        newSkill.skillName = skillName;
        newSkill.description = description;
        newSkill.wordCostSize = wordCostSize;
        newSkill.wordDamageSize = wordDamageSize;
        newSkill.wordHealSize = wordHealSize;
        newSkill.cooldown = cooldown;
        newSkill.numProjectiles = numProjectiles;

        newSkill.duration = duration;
        newSkill.applyEffectOnHit = applyEffectOnHit;
        newSkill.applyEffectOnCast = applyEffectOnCast;

        newSkill.moveSpeed = moveSpeed;

        newSkill.projectileVisiblePercentage = projectileVisiblePercentage;

        newSkill.effectType = effectType;
        newSkill.effectTarget = effectTarget;
        newSkill.effectTargetJob = effectTargetJob;
        newSkill.effect = effect;
        newSkill.effectCount = effectCount;
        newSkill.effectDuration = effectDuration;
        newSkill.ongoingEffect = ongoingEffect;

        newSkill.originalText = originalText;
    }

}

public enum Job 
{
    None = 0,
    Warrior = 1,
    Mage = 2,
    Ranger = 3,
    Cleric = 4,
    Bard = 5,
    All = 6,
}

public enum SkillType
{
    None = 0,
    Attack = 1,
    Heal = 2,
    Effect = 3,
    Hybrid = 4,
}

public enum Enemy
{
    None = 0,
    Rats = 1,
    Giant = 2,
    Knight = 3,
    Witch = 4,
    Ninja = 5,
}