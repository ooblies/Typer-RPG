using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GlobalSkills : Singleton<GlobalSkills>
{
    private List<Skill> skills;

    private void Awake()
    {
        skills = new List<Skill>();
        
        GenerateWarriorSkills();
        GenerateMageSkills();
        GenerateRangerSkills();
        GenerateClericSkills();
        GenerateBardSkills();

        Debug.Log("Generated Skills");
    }

    public Skill getSkillById(string skillId)
    {
        return skills.Where(s => s.skillId == skillId).FirstOrDefault();
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
            skillId = "warriorBlock",
            job = Job.Warrior,
            skillType = SkillType.Block,
            skillName = "Shield Block",
            description = "Block the next attack with your shield",

            wordCostSize = 5,
            wordsBlocked = 2,
            cooldown = 15,
        };
        Skill Prepare = new Skill
        {
            skillId = "warriorPrepare",
            job = Job.Warrior,
            skillType = SkillType.Buff,
            skillName = "Prepare",
            description = "Prepare for your next attack, doubling it",

            wordCostSize = 5,
            warriorWordDamageSizeBuff = 2.0f,
            cooldown = 20,
        };
        Skill Enrage = new Skill
        {
            skillId = "warriorEnrage",
            job = Job.Warrior,
            skillType = SkillType.Buff,
            skillName = "Enrage",
            description = "Get mad, increasing the cost and damage of your skills",

            wordCostSize = 4,
            ongoingWarriorWordCostSizeBuff = 1.2f,
            applyOngoingOnCast = true,
            duration = 10,
            cooldown = 12,
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
            skillId = "mageFireRain",
            job = Job.Mage,
            skillType = SkillType.Attack,
            skillName = "Fire Rain",
            description = "Rain fire on your enemies, dealing damage over time",

            wordCostSize = 6,
            applyOngoingOnCast = true,
            ongoingDamageSize = 1f,
            duration = 10,
            cooldown = 15,
        };
        Skill ForceField = new Skill
        {
            skillId = "mageForceField",
            job = Job.Mage,
            skillType = SkillType.Block,
            skillName = "Force Field",
            description = "Surround yourself in a magical field, blocking the next 10 letters",

            wordCostSize = 5,
            lettersBlocked = 10,
            cooldown = 15
        };
        Skill IceStorm = new Skill
        {
            skillId = "mageIceStorm",
            job = Job.Mage,
            skillType = SkillType.Buff,
            skillName = "Ice Storm",
            description = "Cover the battlefield in Ice, slowing down emeny attacks",

            wordCostSize = 10,
            applyOngoingOnCast = true,
            ongoingEnemyWordSpeedBuff = 0.75f,
            cooldown = 15,
            duration = 15,
        };
        Skill FireBall = new Skill
        {
            skillId = "mageFireball",
            job = Job.Mage,
            skillType = SkillType.Attack,
            skillName = "Fireball",
            description = "Throw a ball of fire at your enemies, dealing damage upfront and over time",

            numProjectiles = 1,
            wordCostSize = 8,
            applyOngoingOnHit = transform,
            ongoingDamageSize = 1,
            wordDamageSize = 5,
            duration = 10,
            cooldown = 10,
        };

        Skill Enchant = new Skill
        {
            skillId = "mageEnchant",
            job = Job.Mage,
            skillType = SkillType.Buff,
            skillName = "Enchant",
            description = "Enchant your party member's weapons with fire, adding damage to their next attack",

            wordCostSize = 8,
            warriorWordDamageSizeBuff = 1.25f,
            mageWordCostSizeBuff = 1.25f,
            rangerWordCostSizeBuff = 1.25f,
            clericWordCostSizeBuff = 1.25f,
            bardWordCostSizeBuff = 1.25f,
            cooldown = 20,
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
            skillId = "rangerShoot",
            job = Job.Ranger,
            skillType = SkillType.Attack,
            skillName = "Shoot",
            description = "Fire an arrow at the enemy",

            wordCostSize = 5,
            wordDamageSize = 4,
            cooldown = 3,
            moveSpeedBuff = 1.25f,
            numProjectiles = 1,
        };
        Skill DoubleShot = new Skill
        {
            skillId = "rangerDoubleShot",
            job = Job.Ranger,
            skillType = SkillType.Attack,
            skillName = "Double Shot",
            description = "Fire 2 arrows in rapid succession",

            wordCostSize = 5,
            wordDamageSize = 3,
            numProjectiles = 2,
            moveSpeedBuff = 1.25f,
            cooldown = 6,
        };
        Skill Snipe = new Skill
        {
            skillId = "rangerSnipe",
            job = Job.Ranger,
            skillType = SkillType.Attack,
            skillName = "Snipe",
            description = "Take aim and fire a fast, high damage shot",

            wordCostSize = 10,
            wordDamageSize = 7,
            numProjectiles = 1,
            moveSpeedBuff = 3,
            cooldown = 15,

        };
        Skill Barrage = new Skill
        {
            skillId = "rangerBarrage",
            job = Job.Ranger,
            skillType = SkillType.Attack,
            skillName = "Barrage",
            description = "Fire multiple arrows at once",

            wordCostSize = 10,
            wordDamageSize = 3,
            numProjectiles = 5,
            moveSpeedBuff = 1.25f,
            cooldown = 20,
        };
        Skill Snare = new Skill
        {
            skillId = "rangerSnare",
            job = Job.Ranger,
            skillType = SkillType.Buff,
            skillName = "Snare",
            description = "Catch the enemy in a snare, freezing their attacks",

            enemyTypingFreezeDuration = 5,
            wordCostSize = 6,
            cooldown = 15,
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
            skillId = "clericShield",
            job = Job.Cleric,
            skillType = SkillType.Block,
            skillName = "Shield",
            description = "Block the next 3 attacks",

            wordCostSize = 6,
            wordsBlocked = 3,
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
            skillId = "bardHasteningMelody",
            job = Job.Bard,
            skillType = SkillType.Buff,
            skillName = "Hastening Melody",
            description = "Reduce cooldowns for your party-members abilities",

            ongoingCooldownMultiplierBuff = .75f,
            applyOngoingOnCast = true,
            wordCostSize = 10,
            duration = 10,
            cooldown = 10,
            shareCooldownCode = "bardSongs",
        };
        Skill EmbiggeningTune = new Skill
        {
            skillId = "bardEmbiggeningTune",
            job = Job.Bard,
            skillType = SkillType.Buff,
            skillName = "Embiggening Tune",
            description = "Add damage to your party-members attack",

            ongoingGlobalWordDamageSizeBuff = 1.25f,
            applyOngoingOnCast = true,
            wordCostSize = 10,
            duration = 10,
            cooldown = 10,
            shareCooldownCode = "bardSongs",
        };
        Skill NimbleingDiddy = new Skill
        {
            skillId = "bardNimbleingDiddy",
            job = Job.Bard,
            skillType = SkillType.Buff,
            skillName = "Nimbleing Diddy",
            description = "Add speed to your party-members attacks",

            ongoingFriendlyWordSpeedBuff = 1.25f,
            applyOngoingOnCast = true,
            wordCostSize = 10,
            duration = 10,
            cooldown = 10,
            shareCooldownCode = "bardSongs",
        };
        Skill RestoringSonata = new Skill
        {
            skillId = "bardRestoringSonata",
            job = Job.Bard,
            skillType = SkillType.Buff,
            skillName = "Restoring Sonata",
            description = "Restore a small amout of health per second",

            ongoingHealSize = 1f,
            applyOngoingOnCast = true,
            wordCostSize = 10,
            duration = 10,
            cooldown = 10,
            shareCooldownCode = "bardSongs",
        };
        Skill SharpeningJig = new Skill
        {
            skillId = "bardSharpeningJig",
            job = Job.Bard,
            skillType = SkillType.Buff,
            skillName = "Sharpening Jig",
            description = "Reduce cost requirements for your party-members skills",

            ongoingGlobalWordCostSizeBuff = 0.75f,
            applyOngoingOnCast = true,
            wordCostSize = 10,
            duration = 10,
            cooldown = 10,
            shareCooldownCode = "bardSongs",
        };

        skills.Add(HasteningMelody);
        skills.Add(EmbiggeningTune);
        skills.Add(NimbleingDiddy);
        skills.Add(RestoringSonata);
        skills.Add(SharpeningJig);
    }
}
