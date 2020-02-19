using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GlobalSkills : Singleton<GlobalSkills>
{
    private List<Skill> skills;

    private void Awake()
    {
        GenerateSkills();

        //Debug.Log("Generated Skills");
    }

    private void GenerateSkills()
    {
        skills = new List<Skill>();

        GenerateWarriorSkills();
        GenerateMageSkills();
        GenerateRangerSkills();
        GenerateClericSkills();
        GenerateBardSkills();

        GenerateEnemySkills();
    }

    public List<Skill> getRandomSkills()
    {
        //GenerateSkills();

        List<Skill> randomSkills = new List<Skill>();

        int randomJob1 = UnityEngine.Random.Range(1, 6);
        int randomJob2 = randomJob1;
        int randomJob3 = randomJob1;

        while (randomJob2 == randomJob1)
        {
            randomJob2 = UnityEngine.Random.Range(1, 6);
        }

        while(randomJob3 == randomJob1 || randomJob3 == randomJob2)
        {
            randomJob3 = UnityEngine.Random.Range(1, 6);
        }

        randomSkills.AddRange(getSkillsByJob((Job)randomJob1).OrderBy(x => Guid.NewGuid()).Take(3));
        randomSkills.AddRange(getSkillsByJob((Job)randomJob2).OrderBy(x => Guid.NewGuid()).Take(3));
        randomSkills.AddRange(getSkillsByJob((Job)randomJob3).OrderBy(x => Guid.NewGuid()).Take(3));

        return randomSkills.OrderBy(r => r.orderBy).ToList();
    }    

    public List<Skill> getSkillsByEnemy(Enemy enemy)
    {
        return skills.Where(s => s.enemy == enemy).ToList(); ;
    }

    public Skill getSkillById(string skillId)
    {
        //GenerateSkills();
        try
        {            
            Skill skill = skills.Where(s => s.skillId == skillId).First();
            return skill;
        }
        catch (Exception ex)
        {
            throw ex;
        }        
    }

    public List<Skill> getSkillsByJob(Job job)
    {
        return skills.Where(s => s.job == job).ToList();
    }

    private void GenerateEnemySkills()
    {
        Skill ratBite = new Skill
        {
            orderBy = 1,
            skillId = "ratBite",
            enemy = Enemy.Rats,
            skillType = SkillType.Attack,
            skillName = "Bite",
            description = "Chomp chomp chomp",

            wordDamageSize = 3,
            numProjectiles = 1,
            cooldown = 2,
        };
        Skill ratSwarm = new Skill
        {
            orderBy = 2, 
            skillId = "ratSwarm",
            enemy = Enemy.Rats,
            skillType = SkillType.Attack,
            skillName = "Swarm",
            description = "A swarm of rats",

            wordDamageSize = 3,
            numProjectiles = 10,
            cooldown = 20,
        };

        Skill ratDisease = new Skill
        {
            orderBy = 3,
            skillId = "ratDisease",
            enemy = Enemy.Rats,
            skillType = SkillType.Attack,
            effectType = GlobalEffects.EffectType.DamageOverTime,
            skillName = "Disease",
            description = "Rats are yucky",

            wordDamageSize = 5,
            numProjectiles = 1,
            cooldown = 10,

            applyEffectOnHit = true,
            effect = 1,
            effectDuration = 15,
            duration = 10,
        };

        skills.Add(ratBite);
        skills.Add(ratSwarm);
        skills.Add(ratDisease);

        Skill giantSmash = new Skill
        {
            orderBy = 1,
            skillId = "giantSmash",
            enemy = Enemy.Giant,
            skillType = SkillType.Attack,
            skillName = "Smash",
            description = "A big ol' smash",

            wordDamageSize = 25,
            numProjectiles = 1,
            cooldown = 12,
        };
        Skill giantDoubleSmash = new Skill
        {
            orderBy = 2,
            skillId = "giantDoubleSmash",
            enemy = Enemy.Giant,
            skillType = SkillType.Attack,
            skillName = "Double Smash",
            description = "A big ol' double smash",

            wordDamageSize = 15,
            numProjectiles = 2,
            cooldown = 18,
        };
        Skill giantEarthquake = new Skill
        {
            orderBy = 3,
            skillId = "giantEarthquake",
            enemy = Enemy.Giant,
            skillType = SkillType.Effect,
            skillName = "Earthquake",
            description = "Stomp on the ground, stunning enemies",
                        
            cooldown = 30,
            effectType = GlobalEffects.EffectType.Freeze,
            effectTarget = GlobalEffects.EffectTarget.Friendly,
            effectTargetJob = Job.All,
            effectDuration = 5,      
            ongoingEffect = true,
            applyEffectOnCast = true,
        };

        skills.Add(giantSmash);
        skills.Add(giantDoubleSmash);
        skills.Add(giantEarthquake);

        Skill knightBlock = new Skill
        {
            orderBy = 1,
            skillId = "knightBlock",
            enemy = Enemy.Knight,
            skillType = SkillType.Effect,
            skillName = "Heavy Armor",
            description = "Continually block a portion of all attacks",

            cooldown = 10,
            effectDuration = 10,
            effectTarget = GlobalEffects.EffectTarget.Enemy,
            effectType = GlobalEffects.EffectType.BlockLetters,
            effect = 3,
            ongoingEffect = true,
            applyEffectOnCast = true,
        };

        Skill knightShieldBash = new Skill
        {
            orderBy = 2,
            skillId = "knightShieldBash",
            enemy = Enemy.Knight,
            skillType = SkillType.Attack,
            skillName = "Shield Bash",
            description = "Bash with shield, stunning enemy",

            wordDamageSize = 10,
            numProjectiles = 1,
            cooldown = 25,
            applyEffectOnHit = true,
            effectDuration = 5,
            effectTarget = GlobalEffects.EffectTarget.Friendly,
            ongoingEffect = true,
            effectTargetJob = Job.All,
            effectType = GlobalEffects.EffectType.Freeze,            
        };

        Skill knightChop = new Skill
        {
            orderBy = 3,
            skillId = "knightChop",
            enemy = Enemy.Knight,
            skillType = SkillType.Attack,
            skillName = "Sword Chop",
            description = "Chop with sword",

            wordDamageSize = 15,
            numProjectiles = 1,
            cooldown = 10,            
        };

        skills.Add(knightBlock);
        skills.Add(knightShieldBash);
        skills.Add(knightChop);

        Skill witchPurge = new Skill
        {
            orderBy = 1,
            skillId = "witchPurge",
            enemy = Enemy.Witch,
            skillType = SkillType.Effect,
            skillName = "Purge",
            description = "Remove all enemy words on the battlefield",
            
            cooldown = 20,
            effectTarget = GlobalEffects.EffectTarget.Friendly,
            effectType = GlobalEffects.EffectType.RemoveWords,
        };

        Skill witchPoison = new Skill
        {
            orderBy = 2,
            skillId = "witchPoison",
            enemy = Enemy.Witch,
            skillType = SkillType.Effect,
            skillName = "Poison Gas",
            description = "Slowly damage your enemies",

            effectDuration = 10,
            cooldown = 10,
            effectTarget = GlobalEffects.EffectTarget.Friendly,
            effectType = GlobalEffects.EffectType.DamageOverTime,
            applyEffectOnCast = true,
            effect = 3,
            ongoingEffect = true,
            shareCooldownCode = "witch",
        };

        Skill witchHeal = new Skill
        {
            orderBy = 3,
            skillId = "witchHeal",
            enemy = Enemy.Witch,
            skillType = SkillType.Effect,
            skillName = "Healing Aura",
            description = "Heal over time",

            effectDuration = 10,
            cooldown = 10,
            effectTarget = GlobalEffects.EffectTarget.Enemy,
            effectType = GlobalEffects.EffectType.HealOverTime,
            applyEffectOnCast = true,
            effect = 3,
            ongoingEffect = true,
            shareCooldownCode = "witch",
        };

        skills.Add(witchPurge);
        skills.Add(witchPoison);
        skills.Add(witchHeal);

        Skill ninjaShurikan = new Skill
        {
            orderBy = 1,
            skillId = "ninjaShurikan",
            enemy = Enemy.Ninja,
            skillType = SkillType.Attack,
            skillName = "Shurikan",
            description = "Spinny death",

            wordDamageSize = 5,
            numProjectiles = 2,
            cooldown = 6,
            projectileVisiblePercentage = 0.5f,
        };

        Skill ninjaBackstab = new Skill
        {
            orderBy = 2,
            skillId = "ninjaBackstab",
            enemy = Enemy.Ninja,
            skillType = SkillType.Attack,
            skillName = "Backstab",
            description = "A difficult to stop attack",

            wordDamageSize = 4,
            numProjectiles = 1,
            cooldown = 10,
            projectileVisiblePercentage = 0.25f,
            damageMultiplier = 2.5f,

        };

        Skill ninjaShadowstep = new Skill
        {
            orderBy = 3,
            skillId = "ninjaShadowStep",
            enemy = Enemy.Ninja,
            skillType = SkillType.Effect,
            effectType = GlobalEffects.EffectType.BlockWords,
            skillName = "Shadowstep",
            description = "Step into the shadows, dodging attacks",

            
            effectCount = 2,
            ongoingEffect = false,
            applyEffectOnCast = true,
            effectTarget = GlobalEffects.EffectTarget.Enemy,                        
            cooldown = 15,            
        };

        skills.Add(ninjaShurikan);
        skills.Add(ninjaBackstab);
        skills.Add(ninjaShadowstep);
    }

    public List<Skill> getSkillByCooldownCode(string code)
    {
        List<Skill> slist = new List<Skill>();

        if (code.Length > 0)
        {
            foreach (Skill skill in skills)
            {
                if (skill.shareCooldownCode == code)
                {
                    slist.Add(skill);
                }
            }
        }

        return slist;       
    }

    private void GenerateWarriorSkills()
    {
        //Debug.Log("Generating Warrior Skills");

        Skill TwoHandedAttack = new Skill
        {
            orderBy = 1,
            skillId = "warrior2h",            
            job = Job.Warrior,
            skillType = SkillType.Attack,
            skillName = "Two-Handed Attack",
            description = "A big ol' smack",

            wordCostSize = 8,
            wordDamageSize = 8,
            cooldown = 8,
            numProjectiles = 1,
        };
        Skill OneHandedAttack = new Skill
        {
            orderBy = 2,
            skillId = "warrior1h",
            job = Job.Warrior,
            skillType = SkillType.Attack,
            skillName = "One-Handed Attack",
            description = "A regular smack",

            wordCostSize = 4,
            wordDamageSize = 4,
            cooldown = 4,
            numProjectiles = 1,
        };
        Skill Block = new Skill
        {
            orderBy = 3,
            skillId = "warriorBlock",
            job = Job.Warrior,
            skillType = SkillType.Effect,
            skillName = "Shield Block",
            description = "Block the next attack with your shield",

            
            effectTarget = GlobalEffects.EffectTarget.Friendly,
            effectType = GlobalEffects.EffectType.BlockWords,
            applyEffectOnCast = true,
            effectCount = 1,

            wordCostSize = 5,
            cooldown = 15,
        };
        Skill Prepare = new Skill
        {
            orderBy = 4,
            skillId = "warriorPrepare",
            job = Job.Warrior,
            skillType = SkillType.Effect,
            skillName = "Prepare",
            description = "Prepare for your next attack, doubling it",

            wordCostSize = 5,
            cooldown = 20,
            effectType = GlobalEffects.EffectType.DamageMultiplier,
            effectTarget = GlobalEffects.EffectTarget.Friendly,
            effectTargetJob = Job.Warrior,
            effect = 2.0f,
            effectCount = 1,
            applyEffectOnCast = true,
        };
        Skill Enrage = new Skill
        {
            orderBy = 5,
            skillId = "warriorEnrage",
            job = Job.Warrior,
            skillType = SkillType.Effect,            
            skillName = "Enrage",
            description = "Get mad, increasing the cost and damage of your skills",

            wordCostSize = 4,
            cooldown = 12,
            effectType = GlobalEffects.EffectType.CostMultiplier,
            effectTarget = GlobalEffects.EffectTarget.Friendly,
            effectTargetJob = Job.Warrior,
            effect = 1.2f,
            effectDuration = 10,
            ongoingEffect = true,
            applyEffectOnCast = true,
            
        };

        skills.Add(TwoHandedAttack);
        skills.Add(OneHandedAttack);
        skills.Add(Block);
        skills.Add(Prepare);
        skills.Add(Enrage);
    }
    private void GenerateMageSkills()
    {
        //Debug.Log("Generating Mage Skills");

        Skill FireRain = new Skill
        {
            orderBy = 6,
            skillId = "mageFireRain",
            job = Job.Mage,
            skillType = SkillType.Effect,
            skillName = "Fire Rain",
            description = "Rain fire on your enemies, dealing damage over time",

            wordCostSize = 6,
            applyEffectOnCast = true,
            cooldown = 15,
            effectType = GlobalEffects.EffectType.DamageOverTime,
            effectTarget = GlobalEffects.EffectTarget.Enemy,
            effect = 1,
            effectDuration = 10,
            ongoingEffect = true,
        };
        Skill ForceField = new Skill
        {
            orderBy = 7,
            skillId = "mageForceField",
            job = Job.Mage,
            skillType = SkillType.Effect,
            skillName = "Force Field",
            description = "Surround yourself in a magical field, blocking the next 10 letters",


            effectTarget = GlobalEffects.EffectTarget.Friendly,
            effectType = GlobalEffects.EffectType.BlockLetters,
            applyEffectOnCast = true,
            effectCount = 10,

            wordCostSize = 5,
            cooldown = 15
        };
        Skill IceStorm = new Skill
        {
            orderBy = 8,
            skillId = "mageIceStorm",
            job = Job.Mage,
            skillType = SkillType.Effect,
            skillName = "Ice Storm",
            description = "Cover the battlefield in Ice, slowing down emeny attacks",

            wordCostSize = 10,
            applyEffectOnCast = true,
            cooldown = 15,
            effectType = GlobalEffects.EffectType.SpeedMultiplier,
            effectTarget = GlobalEffects.EffectTarget.Enemy,            
            effect = 0.75f,
            effectDuration = 15,
            ongoingEffect = true,

        };
        Skill FireBall = new Skill
        {
            orderBy = 9,
            skillId = "mageFireball",
            job = Job.Mage,
            skillType = SkillType.Effect,
            skillName = "Fireball",
            description = "Throw a ball of fire at your enemies, dealing damage upfront and over time",

            numProjectiles = 1,
            wordCostSize = 8,
            applyEffectOnHit = true,
            cooldown = 15,
            effectType = GlobalEffects.EffectType.DamageOverTime,
            effectTarget = GlobalEffects.EffectTarget.Enemy,
            effect = 1,
            effectDuration = 10,
            ongoingEffect = true,            
            wordDamageSize = 5,            
        };

        Skill Enchant = new Skill
        {
            orderBy = 10,
            skillId = "mageEnchant",
            job = Job.Mage,
            skillType = SkillType.Effect,
            skillName = "Enchant",
            description = "Enchant your party member's weapons with lightning, adding damage to the 5 attacks next attack",

            wordCostSize = 8,
            cooldown = 20,
            effectType = GlobalEffects.EffectType.DamageMultiplier,
            effectTarget = GlobalEffects.EffectTarget.Friendly,
            effectTargetJob = Job.All,
            effect = 1.25f,
            ongoingEffect = false,
            effectCount = 5,
            applyEffectOnCast = true,
        };

        skills.Add(FireRain);
        skills.Add(ForceField);
        skills.Add(IceStorm);
        skills.Add(FireBall);
        skills.Add(Enchant);
    }
    private void GenerateRangerSkills()
    {
        //Debug.Log("Generating Ranger Skills");
        Skill Shoot = new Skill
        {
            orderBy = 11,
            skillId = "rangerShoot",
            job = Job.Ranger,
            skillType = SkillType.Attack,
            skillName = "Shoot",
            description = "Fire an arrow at the enemy",

            wordCostSize = 5,
            wordDamageSize = 4,
            cooldown = 3,
            moveSpeed = 1.25f,
            numProjectiles = 1,
        };
        Skill DoubleShot = new Skill
        {
            orderBy = 12,
            skillId = "rangerDoubleShot",
            job = Job.Ranger,
            skillType = SkillType.Attack,
            skillName = "Double Shot",
            description = "Fire 2 arrows in rapid succession",

            wordCostSize = 5,
            wordDamageSize = 3,
            numProjectiles = 2,
            moveSpeed = 1.25f,
            cooldown = 6,
        };
        Skill Snipe = new Skill
        {
            orderBy = 13,
            skillId = "rangerSnipe",
            job = Job.Ranger,
            skillType = SkillType.Attack,
            skillName = "Snipe",
            description = "Take aim and fire a fast, high damage shot",

            wordCostSize = 10,
            wordDamageSize = 7,
            numProjectiles = 1,
            moveSpeed = 3,
            cooldown = 15,

        };
        Skill Barrage = new Skill
        {
            orderBy = 14,
            skillId = "rangerBarrage",
            job = Job.Ranger,
            skillType = SkillType.Attack,
            skillName = "Barrage",
            description = "Fire multiple arrows at once",

            wordCostSize = 10,
            wordDamageSize = 3,
            numProjectiles = 5,
            moveSpeed = 1.25f,
            cooldown = 20,
        };
        Skill Snare = new Skill
        {
            orderBy = 15,
            skillId = "rangerSnare",
            job = Job.Ranger,
            skillType = SkillType.Effect,
            skillName = "Snare",
            description = "Catch the enemy in a snare, stunning them temporarily",
           
            wordCostSize = 6,
            cooldown = 15,
            effectType = GlobalEffects.EffectType.Freeze,
            effectTarget = GlobalEffects.EffectTarget.Enemy,                        
            effectDuration = 5,
            ongoingEffect = true,
            applyEffectOnCast = true,
        };

        skills.Add(Shoot);
        skills.Add(DoubleShot);
        skills.Add(Snipe);
        skills.Add(Barrage);
        skills.Add(Snare);
    }
    private void GenerateClericSkills()
    {
        //Debug.Log("Generating Cleric Skills");
        Skill BigHeal = new Skill
        {
            orderBy = 16,
            skillId = "clericBigHeal",
            job = Job.Cleric,
            skillType = SkillType.Heal,
            skillName = "Big Heal",
            description = "Restore a large amount of hit points",

            wordCostSize = 8,
            wordHealSize = 8,
            cooldown = 8,            
        };
        Skill SmallHeal = new Skill
        {
            orderBy = 17,
            skillId = "clericSmallHeal",
            job = Job.Cleric,
            skillType = SkillType.Heal,
            skillName = "Small Heal",
            description = "Restore a small amount of hit points",

            wordCostSize = 4,
            wordHealSize = 4,
            cooldown = 4,
        };
        Skill EmergencyHeal = new Skill
        {
            orderBy =18 ,
            skillId = "clericEmergencyHeal",
            job = Job.Cleric,
            skillType = SkillType.Heal,
            skillName = "Emerygency Heal",
            description = "Restore a huge amount of hit points",

            wordCostSize = 3,
            wordHealSize = 15,
            cooldown = 60,
        };
        Skill Mace = new Skill
        {
            orderBy = 19,
            skillId = "clericMace",
            job = Job.Cleric,
            skillType = SkillType.Attack,
            skillName = "Mace",
            description = "Smash someone with your mace",

            wordCostSize = 6,
            wordDamageSize = 4,
            numProjectiles = 1,
            cooldown = 8,
        };
        Skill Shield = new Skill
        {
            orderBy = 20,
            skillId = "clericShield",
            job = Job.Cleric,
            skillType = SkillType.Effect,
            skillName = "Shield",
            description = "Block the next 3 attacks",

            wordCostSize = 6,
            effectTarget = GlobalEffects.EffectTarget.Friendly,
            effectType = GlobalEffects.EffectType.BlockWords,
            applyEffectOnCast = true,
            effectCount = 3,
            cooldown = 15,
        };

        skills.Add(BigHeal);
        skills.Add(SmallHeal);
        skills.Add(EmergencyHeal);
        skills.Add(Mace);
        skills.Add(Shield);

    }
    private void GenerateBardSkills()
    {
        //Debug.Log("Generating Bard Skills");
        Skill HasteningMelody = new Skill
        {
            orderBy = 21,
            skillId = "bardHasteningMelody",
            job = Job.Bard,
            skillType = SkillType.Effect,
            skillName = "Hastening Melody",
            description = "Reduce cooldowns for your party-members abilities",
            
            applyEffectOnCast = true,
            wordCostSize = 10,            
            cooldown = 10,
            shareCooldownCode = "bardSongs",

            effectType = GlobalEffects.EffectType.CooldownMultiplier,
            effectTarget = GlobalEffects.EffectTarget.Friendly,
            effectTargetJob = Job.All,
            effect = 0.75f,
            effectDuration = 10f,
            ongoingEffect = true,            
        };
        Skill EmbiggeningTune = new Skill
        {
            orderBy = 22,
            skillId = "bardEmbiggeningTune",
            job = Job.Bard,
            skillType = SkillType.Effect,
            skillName = "Embiggening Tune",
            description = "Add damage to your party-members attack",

            applyEffectOnCast = true,
            wordCostSize = 10,
            duration = 10,
            cooldown = 10,
            shareCooldownCode = "bardSongs",

            effectType = GlobalEffects.EffectType.DamageMultiplier,
            effectTarget = GlobalEffects.EffectTarget.Friendly,
            effectTargetJob = Job.All,
            effect = 1.25f,
            effectDuration = 10f,
            ongoingEffect = true,
        };
        Skill NimbleingDiddy = new Skill
        {
            orderBy = 23,
            skillId = "bardNimbleingDiddy",
            job = Job.Bard,
            skillType = SkillType.Effect,
            skillName = "Nimbleing Diddy",
            description = "Add speed to your party-members attacks",

            applyEffectOnCast = true,
            wordCostSize = 10,
            duration = 10,
            cooldown = 10,
            shareCooldownCode = "bardSongs",

            effectType = GlobalEffects.EffectType.SpeedMultiplier,
            effectTarget = GlobalEffects.EffectTarget.Friendly,
            effectTargetJob = Job.All,
            effect = 1.5f,
            effectDuration = 10f,
            ongoingEffect = true,
        };
        Skill RestoringSonata = new Skill
        {
            orderBy = 24,
            skillId = "bardRestoringSonata",
            job = Job.Bard,
            skillType = SkillType.Effect,
            skillName = "Restoring Sonata",
            description = "Restore a small amout of health per second",

            
            applyEffectOnCast = true,
            wordCostSize = 10,
            cooldown = 10,
            shareCooldownCode = "bardSongs",

            effectType = GlobalEffects.EffectType.HealOverTime,
            effectTarget = GlobalEffects.EffectTarget.Friendly,
            effectTargetJob = Job.All,
            effect = 1,
            effectDuration = 10,
            ongoingEffect = true,
        };
        Skill SharpeningJig = new Skill
        {
            orderBy = 25,
            skillId = "bardSharpeningJig",
            job = Job.Bard,
            skillType = SkillType.Effect,
            skillName = "Sharpening Jig",
            description = "Reduce cost requirements for your party-members skills",
            
            applyEffectOnCast = true,
            wordCostSize = 10,
            duration = 10,
            cooldown = 10,
            shareCooldownCode = "bardSongs",

            effectType = GlobalEffects.EffectType.CostMultiplier,
            effectTarget = GlobalEffects.EffectTarget.Friendly,
            effectTargetJob = Job.All,
            effect = 0.75f,
            effectDuration = 10f,
            ongoingEffect = true,
        };

        skills.Add(HasteningMelody);
        skills.Add(EmbiggeningTune);
        skills.Add(NimbleingDiddy);
        skills.Add(RestoringSonata);
        skills.Add(SharpeningJig);
    }
}
