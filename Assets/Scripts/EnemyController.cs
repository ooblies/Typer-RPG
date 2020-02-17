using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    public Image enemyImage;
    public Text enemyTitle;
    public Enemy enemy;

    public GameObject skill1;
    public GameObject skill2;
    public GameObject skill3;

    private SkillButtonController sbc1;
    private SkillButtonController sbc2;
    private SkillButtonController sbc3;

    public Text skill1Title;
    public Text skill2Title;
    public Text skill3Title;

    private float cooldown1;
    private float cooldown2;
    private float cooldown3;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void Awake()
    {
        sbc1 = skill1.GetComponentInChildren<SkillButtonController>();
        sbc2 = skill2.GetComponentInChildren<SkillButtonController>();
        sbc3 = skill3.GetComponentInChildren<SkillButtonController>();

        cooldown1 = Random.Range(0f, 1f);
        cooldown2 = Random.Range(0f, 1f);
        cooldown3 = Random.Range(0f, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadEnemy()
    {
        enemy = GlobalVars.Instance.selectedEnemy;

        string path = "Enemies/" + enemy.ToString();

        Texture2D texture = Resources.Load<Texture2D>(path);

        if (texture == null)
        {
            //missing icon
            texture = Resources.Load<Texture2D>("Characters/blank");
        }

        Rect rect = new Rect(0.0f, 0.0f, texture.width, texture.height);
        Vector2 vec2 = new Vector2(0.5f, 0.5f);

        enemyImage.sprite = Sprite.Create(texture, rect, vec2, texture.width);

        enemyTitle.text = enemy.ToString();

        List<Skill> skills = GlobalSkills.Instance.getSkillsByEnemy(enemy);

        updateSkill(sbc1, skills[0]);
        skill1Title.text = skills[0].skillName;        

        updateSkill(sbc2, skills[1]);
        skill2Title.text = skills[1].skillName;        

        updateSkill(sbc3, skills[2]);
        skill3Title.text = skills[2].skillName;

        //randomly set cooldowns (exxcept for witch)
        float cooldownMultiplier = GlobalEffects.Instance.applyEffect(GlobalEffects.EffectTarget.Enemy, GlobalEffects.EffectType.CooldownMultiplier, Job.None);        

        if (enemy != Enemy.Witch)
        {
            sbc1.cooldown = cooldown1 * skills[0].cooldown * cooldownMultiplier;
            sbc2.cooldown = cooldown2 * skills[1].cooldown * cooldownMultiplier;
            sbc3.cooldown = cooldown3 * skills[2].cooldown * cooldownMultiplier;
        }
        if (enemy == Enemy.Witch)
        {
            sbc1.cooldown = skills[0].cooldown * cooldownMultiplier;
            sbc2.cooldown = skills[1].cooldown * cooldownMultiplier;
            sbc3.cooldown = skills[2].cooldown * cooldownMultiplier;
        }
    }

    private void updateSkill(SkillButtonController sbc, Skill skill)
    {
        sbc.skillId = skill.skillId;
        sbc.skill = skill;

        string iconPath = "Icons/" + sbc.skillId;

        Texture2D iconTexture = Resources.Load<Texture2D>(iconPath);

        if (iconTexture == null)
        {
            //missing icon
            iconTexture = Resources.Load<Texture2D>("Icons/temp");
        }

        Rect iconRect = new Rect(0.0f, 0.0f, iconTexture.width, iconTexture.height);
        Vector2 iconVec2 = new Vector2(0.5f, 0.5f);

        sbc.icon.sprite = Sprite.Create(iconTexture, iconRect, iconVec2, iconTexture.width);
        sbc.txt.text = skill.skillName;
    }
}

