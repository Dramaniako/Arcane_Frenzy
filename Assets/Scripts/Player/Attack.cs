using UnityEngine;

public class Spell
{
    public int spellID;
    public float magnitude;
    public Spell next;
    public Spell prev;

    public Spell(int id, float mag)
    {
        spellID = id;
        magnitude = mag;
    }
}


public class Attack : MonoBehaviour
{
    private Spell currentSpell;

    void Start()
    {
        // Create spells
        Spell fire = new Spell(1, 10f);
        Spell ice = new Spell(2, 8f);
        Spell lightning = new Spell(3, 12f);

        // Link them in a circular doubly linked list
        fire.next = ice;
        ice.next = lightning;
        lightning.next = fire;

        fire.prev = lightning;
        ice.prev = fire;
        lightning.prev = ice;

        // Set starting spell
        currentSpell = fire;
    }

    void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll > 0f)
        {
            currentSpell = currentSpell.next;
            Debug.Log("Next Spell: " + currentSpell.spellID);
        }
        else if (scroll < 0f)
        {
            currentSpell = currentSpell.prev;
            Debug.Log("Previous Spell: " + currentSpell.spellID);
        }
    }
}
