using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class BattleController : MonoBehaviour
{
    public Text playerHealth;
    public Text enemyHealth;
    public float projectileStartingX = -7f;
    public float projectileStartingYMax = 3f;
    public float projectileStartingYMin = -4f;
    public GameObject projectilePrefab;
    public float projectileMoveSpeed = 1f;
    public float projectileHitAtX = 6f;

    public string skill1Id = "";
    public string skill2Id = "";
    public string skill3Id = "";
    public string skill4Id = "";
    public string skill5Id = "";
    public string skill6Id = "";
    public string skill7Id = "";
    public string skill8Id = "";
    public string skill9Id = "";
    

    // Start is called before the first frame update
    void Start()
    {
        string[] skillIds = new string[] { skill1Id, skill2Id, skill3Id, skill4Id, skill5Id, skill6Id, skill7Id, skill8Id, skill9Id };
        
        int i = 0;
        
        GameObject[] skillButtons = GameObject.FindGameObjectsWithTag("SkillButton");
        foreach (GameObject go in skillButtons)
        {
            SkillButtonController sbc = go.GetComponent<SkillButtonController>();

            sbc.skillId = skillIds[i];
            sbc.updateSkill();
            i++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (playerHealth.text != GlobalVars.Instance.playerCurrentHealth.ToString()) {
            playerHealth.text = GlobalVars.Instance.playerCurrentHealth.ToString();
        }
        if (enemyHealth.text != GlobalVars.Instance.enemyCurrentHealth.ToString()) {
            enemyHealth.text = GlobalVars.Instance.enemyCurrentHealth.ToString();
        }

        foreach (GameObject go in GameObject.FindGameObjectsWithTag("PlayerProjectile"))
        {
            Skill skill = go.GetComponent<Skill>();

            float multiplier = 1f;
            multiplier *= GlobalBuffs.Instance.ongoingFriendlyWordSpeedBuff;
            multiplier *= skill.moveSpeedBuff;

            go.transform.Translate(Vector3.right * projectileMoveSpeed * Time.deltaTime * multiplier);

            if (go.transform.position.x > projectileHitAtX)
            {
                go.SendMessage("onHit");
                Destroy(go);
            }
        }

        //typeable
        //enemyprojectiles

        //check player health
        //check enemy health
    }

    //public void SkillButton1() {
    //    GlobalVars.Instance.warriorSkills.skills[0].onCast(startingX, startingYMin, startingYMax, prefab, GlobalVars.Instance.warriorSkills.skills[0]);
    //    button1Cooldown += GlobalVars.Instance.warriorSkills.skills[0].cooldown;
    //    skillButton1.interactable = false;
        
    //}
    //public void SkillButton2()
    //{
    //    GlobalVars.Instance.warriorSkills.skills[1].onCast(startingX, startingYMin, startingYMax, prefab,GlobalVars.Instance.warriorSkills.skills[1]);
    //    button1Cooldown += GlobalVars.Instance.warriorSkills.skills[0].cooldown;
    //    skillButton1.interactable = false;
    //}
    //public void SkillButton3()
    //{
    //    GlobalVars.Instance.warriorSkills.skills[2].onCast(startingX, startingYMin, startingYMax, prefab, GlobalVars.Instance.warriorSkills.skills[2]);
    //    button1Cooldown += GlobalVars.Instance.warriorSkills.skills[0].cooldown;
    //    skillButton1.interactable = false;
    //}
    //public void SkillButton4()
    //{
    //    GlobalVars.Instance.warriorSkills.skills[3].onCast(startingX, startingYMin, startingYMax, prefab, GlobalVars.Instance.warriorSkills.skills[3]);
    //    button1Cooldown += GlobalVars.Instance.warriorSkills.skills[0].cooldown;
    //    skillButton1.interactable = false;
    //}
    //public void SkillButton5()
    //{
    //    GlobalVars.Instance.warriorSkills.skills[4].onCast(startingX, startingYMin, startingYMax, prefab, GlobalVars.Instance.warriorSkills.skills[4]);
    //    button1Cooldown += GlobalVars.Instance.warriorSkills.skills[0].cooldown;
    //    skillButton1.interactable = false;
    //}
}
