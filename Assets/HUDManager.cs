using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class SpellMapping
{
   public Sprite icon;
   public Utils.SpellName spellName;
}

public class HUDManager : MonoBehaviour
{
    public List<SpellMapping> spellMappings;

    private Image spellDisplay;

    // Start is called before the first frame update
    void Start()
    {
        spellDisplay = transform.Find("Spell").GetComponent<Image>();
    }

    public void SetSpell(Utils.SpellName spellName) {
        foreach(SpellMapping spell in spellMappings) {
            if (spell.spellName == spellName) {
                spellDisplay.sprite = spell.icon;
                spellDisplay.color = Color.white;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
