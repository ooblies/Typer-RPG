using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SkillPaneController : MonoBehaviour
{
    // Start is called before the first frame update
    private CharacterPaneController cpc;
    public Outline outline;
    public string skillId;
    public int orderBy;

    void Start()
    {
        cpc = gameObject.GetComponentInParent<CharacterPaneController>();
        outline = GetComponentInChildren<Outline>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void click()
    {
        if (cpc.job > 0)
        {
            if ((cpc.selectedSkills < 3) || outline.enabled)
            {
                if (GlobalVars.Instance.selectedCharacters < 3 || cpc.outline.enabled)
                {
                    outline.enabled = !outline.enabled;

                    if (outline.enabled)
                    {
                        cpc.selectedSkills++;
                        cpc.outline.enabled = true;
                    }
                    else
                    {
                        cpc.selectedSkills--;
                    }
                }
            }

            List<GameObject> panes = GameObject.FindGameObjectsWithTag("CharacterPane").ToList();
            GlobalVars.Instance.selectedCharacters = panes.Where(c => c.GetComponent<Outline>().enabled).Count();
            GlobalVars.Instance.selectedSkills = GameObject.FindGameObjectsWithTag("SkillPane").Where(c => c.GetComponent<Outline>().enabled).Count();
        }    
        
        if (cpc.enemy > 0)
        {
            cpc.click();
        }
    }
}
