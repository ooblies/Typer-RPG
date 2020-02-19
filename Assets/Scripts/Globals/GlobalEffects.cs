using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GlobalEffects : Singleton<GlobalEffects>
{
    public List<Effect> effects = new List<Effect>();
    public Text friendlyEffectText;
    public Text enemyEffectText;
    
    private float nextActionTime = 0.0f;
    public float period = 1f;

    public class Effect
    {
        public Job affectedJob;
        public EffectType effectType;
        public EffectTarget effectTarget;
        
        public string sourceName;

        public bool ongoing = false;
        public float duration = 0f;
        
        public int effectCount = 0;
        
        public float effect = 1;


    }

    public string printEffectList(EffectTarget target)
    {
        string list = "";

        foreach (Effect eff in effects)
        {
            if (eff.effectTarget == target)
            {
            string printEffect = "";

            if (eff.ongoing)
            {
                printEffect = eff.sourceName + " - " + eff.effectTarget.ToString() + " " + eff.affectedJob.ToString() + " " + eff.effectType.ToString() + " x" + eff.effect + " - " + (int)eff.duration + "s\n";
            }
            else
            {
                printEffect = eff.sourceName + " - " + eff.effectTarget.ToString() + " " + eff.affectedJob.ToString() + " " + eff.effectType.ToString() + " x" + eff.effect + " - " + eff.effectCount + " times\n";
            }

            list += printEffect;

            }
        }

        return list;
    }

    public void addEffect(Effect eff)
    {
        if (effects.Where(e => e.sourceName == eff.sourceName && e.effectCount > 0).Count() > 0)
        {
            effects.Where(e => e.sourceName == eff.sourceName && e.effectCount > 0).First().effectCount += eff.effectCount;
        }
        else
        {
            effects.Add(eff);
        }
    }

    public void clearEffects()
    {
        effects = new List<Effect>();
    }

    public enum EffectType
    {
        None = 0,
        DamageMultiplier = 1,
        CostMultiplier = 2,
        CooldownMultiplier = 3,
        SpeedMultiplier = 4,    
        Freeze = 5,
        BlockLetters = 6,
        BlockWords = 7,
        DamageOverTime = 8,
        HealOverTime = 9,
        RemoveWords = 10,
    }

    public enum EffectTarget
    {
        Friendly = 0,
        Enemy = 1,
    }


    // Start is called before the first frame update
    void Start()
    {
    
    }

    public bool isFrozen(EffectTarget target)
    {
        bool isFroze = false;
        if (effects.Where(e => e.effectTarget == target && e.effectType == EffectType.Freeze).Count() > 0)
        {
            isFroze = true;
        }

        return isFroze;
    }

    public int applyLettersBlocked(EffectTarget target, int maxLetters)
    {
        int blocked = 0;

        //check ongoing effects before single-use
        foreach (Effect eff in effects.OrderByDescending(b => b.ongoing))
        {
            if (eff.effectTarget == target && eff.effectType == EffectType.BlockLetters)
            {
                if (blocked < maxLetters)
                {
                    blocked += (int)eff.effect;

                    if (!eff.ongoing)
                    {
                        if (blocked > maxLetters)
                        {
                            eff.effectCount = blocked - maxLetters;
                        } 
                        else
                        {
                            eff.effectCount = 0;
                        }
                    }

                    if (blocked > maxLetters)
                    {
                        blocked = maxLetters;
                    }
                }
            }
        }

        effects = effects.Where(b => b.ongoing == true || b.effectCount > 0).ToList();

        return blocked;
    }

    public bool applyWordBlock(EffectTarget target)
    {
        bool block = false;

        //check ongoing effects before single-use
        foreach (Effect eff in effects.OrderByDescending(b => b.ongoing))
        {
            if (eff.effectTarget == target && eff.effectType == EffectType.BlockWords)
            {
                if (!block)
                {
                    block = true;
                    if (!eff.ongoing)
                    {
                        eff.effectCount--;
                    }
                }
            }
        }

        effects = effects.Where(b => b.ongoing == true || b.effectCount > 0).ToList();

        return block;
    }

    public float checkEffect(EffectTarget target, EffectType type, Job job)
    {
        float multi = 1f;

        foreach (Effect eff in effects)
        {
            if ((eff.affectedJob == job || eff.affectedJob == Job.All) && (eff.effectType == type) && (eff.effectTarget == target))
            {
                multi *= eff.effect;
            }

        }

        return multi;
    }

    public float applyEffect(EffectTarget target, EffectType type, Job job)
    {
        float multi = 1f;

        foreach (Effect eff in effects)
        {
            //decrease single use effects
            if ((eff.affectedJob == job || eff.affectedJob == Job.All) && (eff.effectType == type) && (eff.effectTarget == target))
            {
                if (!eff.ongoing)
                {
                    eff.effectCount--;
                }

                multi *= eff.effect;
            }

        }

        effects = effects.Where(b => b.ongoing == true || b.effectCount > 0).ToList();

        return multi;
    }

    public float getCurrentFreezeDuration(EffectTarget target)
    {
        float freezeDuration = 0f;

        if (effects.Where(b => b.effectType == EffectType.Freeze && b.effectTarget == target).Count() > 0)
        {
            freezeDuration = effects.Where(b => b.effectType == EffectType.Freeze && b.effectTarget == target).Max(b => b.duration);
        }
        
        return freezeDuration;
    }


    public void updateCosts(Job job)
    {
        GameObject[] btns = GameObject.FindGameObjectsWithTag("SkillButton");

        foreach (GameObject btn in btns)
        {
            SkillButtonController sbc = btn.GetComponent<SkillButtonController>();

            if (sbc.skill.job == job || job == Job.All)
            {
                sbc.updateCost();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        friendlyEffectText.text = printEffectList(EffectTarget.Friendly);
        enemyEffectText.text = printEffectList(EffectTarget.Enemy);

        if (Time.time > nextActionTime)
        {
            nextActionTime += period;

            foreach (Effect eff in effects)
            {
                eff.duration -= period;

                if (eff.effectType == EffectType.DamageOverTime)
                {
                    if (eff.effectTarget == EffectTarget.Enemy)
                    {
                        GlobalVars.Instance.enemyCurrentHealth -= eff.effect;
                    }
                    if (eff.effectTarget == EffectTarget.Friendly)
                    {
                        GlobalVars.Instance.playerCurrentHealth -= eff.effect;
                    }
                    

                    ////Debug.Log("Ongoing Effect - " + eff.sourceName + " - Deal " + eff.effect.ToString() + " damage");
                }
                if (eff.effectType == EffectType.HealOverTime)
                {
                    if (eff.effectTarget == EffectTarget.Friendly)
                    {
                        GlobalVars.Instance.playerCurrentHealth = Mathf.Clamp(GlobalVars.Instance.playerCurrentHealth + eff.effect, 0, GlobalVars.Instance.playerMaxHealth);
                        
                    }
                    if (eff.effectTarget == EffectTarget.Enemy)
                    {
                        GlobalVars.Instance.enemyCurrentHealth = Mathf.Clamp(GlobalVars.Instance.enemyCurrentHealth + eff.effect, 0, GlobalVars.Instance.enemyMaxHealth);                        
                    }
                    ////Debug.Log("Ongoing Effect - " + eff.sourceName + " - Heal " + eff.effect.ToString() + " hp");
                }
            }
        }
        effects = effects.Where(b => b.duration > 0 || b.effectCount > 0).ToList();
    }
}
