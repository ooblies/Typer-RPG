using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalEffects : Singleton<GlobalEffects>
{
    public class Effect
    {
        public string name;
        public float duration;
        public float damage;
        public float healing;
    }

    public List<Effect> effects;
    private List<Effect> effectsToDelete;

    private void Awake()
    {
        effects = new List<Effect>();
        effectsToDelete = new List<Effect>();
    }

    private float nextActionTime = 0.0f;
    public float period = 1f;
    private void Update()
    {
        if (effectsToDelete != null)
        {
            foreach (Effect eff in effectsToDelete)
            {
                effects.Remove(eff);

                Debug.Log("Ongoing Effect - " + eff.name + " - Ended");
            }            
        }
        effectsToDelete = new List<Effect>();

        if (Time.time > nextActionTime)
        {
            nextActionTime += period;
            
            if (effects != null)
            {
                foreach (Effect effect in effects)
                {
                    if (effect.duration > 0)
                    {
                        effect.duration -= period;

                        if (effect.healing > 0)
                        {
                            GlobalVars.Instance.playerCurrentHealth += effect.healing;
                            if (GlobalVars.Instance.playerCurrentHealth > GlobalVars.Instance.playerMaxHealth)
                            {
                                GlobalVars.Instance.playerCurrentHealth = GlobalVars.Instance.playerMaxHealth;
                            }

                            Debug.Log("Ongoing Effect - " + effect.name + " - Heal " + effect.healing.ToString() + " hp");
                        }

                        if (effect.damage > 0)
                        {
                            GlobalVars.Instance.enemyCurrentHealth -= effect.damage;

                            Debug.Log("Ongoing Effect - " + effect.name + " - Deal " + effect.damage.ToString() + " damage");
                        }
                    }
                    else
                    {
                        effectsToDelete.Add(effect);
                    }
                }
            }
        }           
    }
}
