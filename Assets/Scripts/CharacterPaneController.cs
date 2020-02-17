using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CharacterPaneController : MonoBehaviour
{
    // Start is called before the first frame update

    public Job job;
    public Enemy enemy;
    public Image img;
    public Text textName;
    public Text txtDescription;

    public Outline outline;

    public GameObject skill1;
    public GameObject skill2;
    public GameObject skill3;
    public GameObject skill4;
    public GameObject skill5;

    public int selectedSkills = 0;


    void Start()
    {
        updateCharacter();
        outline = GetComponentInChildren<Outline>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void click()
    {
        if (job > 0)
        {
            if (GlobalVars.Instance.selectedCharacters < 3 || outline.enabled)
            {
                outline.enabled = !outline.enabled;
            }

            if (!outline.enabled)
            {
                foreach (SkillPaneController spc in GetComponentsInChildren<SkillPaneController>())
                {
                    spc.outline.enabled = false;
                    selectedSkills = 0;
                }
            }

            GlobalVars.Instance.selectedCharacters = GameObject.FindGameObjectsWithTag("CharacterPane").Where(c => c.GetComponent<Outline>().enabled && c.transform.parent.gameObject.GetComponent<CharacterPaneController>().job > 0).Count();
            GlobalVars.Instance.selectedSkills = GameObject.FindGameObjectsWithTag("SkillPane").Where(c => c.GetComponent<Outline>().enabled).Count();
        }

        if (enemy > 0)
        {
            List<CharacterPaneController> panes = GameObject.FindObjectsOfType<CharacterPaneController>().Where(o => o.enemy > 0).ToList();
            foreach(CharacterPaneController pane in panes)
            {
                pane.outline.enabled = false;
            }
            outline.enabled = true;
            GlobalVars.Instance.selectedEnemy = enemy;
        }
    }

    public void updateCharacter()
    {
        string path = (job > 0 ? "Characters/" + job.ToString() : "Enemies/" + enemy.ToString());

        Texture2D texture = Resources.Load<Texture2D>(path);

        if (texture == null)
        {
            //missing icon
            texture = Resources.Load<Texture2D>("Characters/blank");
        }

        Rect rect = new Rect(0.0f, 0.0f, texture.width, texture.height);
        Vector2 vec2 = new Vector2(0.5f, 0.5f);

        img.sprite = Sprite.Create(texture, rect, vec2, texture.width);

        List<Skill> skills = new List<Skill>();

        if (job > 0) //player character
        {
            switch (job)
            {
                case Job.Warrior:
                    txtDescription.text = "A scary warrior";
                    break;
                case Job.Mage:
                    txtDescription.text = "Pew pew.";
                    break;
                case Job.Ranger:
                    txtDescription.text = "Rooty-tooty point-and-shooty";
                    break;
                case Job.Cleric:
                    txtDescription.text = "Heals";
                    break;
                case Job.Bard:
                    txtDescription.text = "Baby shark do-doo";
                    break;
                default:
                    break;
            }

            textName.text = job.ToString();
            skills = GlobalSkills.Instance.getSkillsByJob(job);

            updateSkill(skill1, skills[0]);
            updateSkill(skill2, skills[1]);
            updateSkill(skill3, skills[2]);
            updateSkill(skill4, skills[3]);
            updateSkill(skill5, skills[4]);
        }
        if (enemy > 0) //enemy
        {
            switch (enemy)
            {
                case Enemy.Rats:
                    txtDescription.text = "Lots of small attacks, carries diseases";
                    break;
                case Enemy.Giant:
                    txtDescription.text = "Huge attacks and stuns";
                    break;
                case Enemy.Knight:
                    txtDescription.text = "High blocking power, low damage";
                    break;
                case Enemy.Witch:
                    txtDescription.text = "Unblockable spells";
                    break;
                case Enemy.Ninja:
                    txtDescription.text = "Hard to block attacks";
                    break;
                default:
                    break;
            }

            textName.text = enemy.ToString();
            skills = GlobalSkills.Instance.getSkillsByEnemy(enemy);

            updateSkill(skill1, skills[0]);
            updateSkill(skill2, skills[1]);
            updateSkill(skill3, skills[2]);
        }

    }

    void updateSkill(GameObject skillPanel, Skill skill)
    {
        string path = "Icons/" + skill.skillId;

        Texture2D text = Resources.Load<Texture2D>(path);

        if (text == null)
        {
            //missing icon
            text = Resources.Load<Texture2D>("Icons/temp");
        }

        Rect rect = new Rect(0.0f, 0.0f, text.width, text.height);
        Vector2 vec2 = new Vector2(0.5f, 0.5f);
        Sprite sprite = Sprite.Create(text, rect, vec2, text.width);
        
        foreach (Image img in skillPanel.GetComponentsInChildren<Image>())
        {
            if (img.name == "Icon")
            {
                img.sprite = sprite;
            }
        }

        foreach (Text txt in skillPanel.GetComponentsInChildren<Text>())
        {
            if (txt.name == "Name")
            {
                txt.text = skill.skillName;
            }
            if (txt.name == "Description")
            {
                txt.text = skill.description;
            }
        }

        SkillPaneController spc = skillPanel.GetComponent<SkillPaneController>();
        spc.skillId = skill.skillId;
        spc.orderBy = skill.orderBy;

    }
}
