using UnityEngine;

public class Spell
{
    public int spellID;
    public float magnitude;
    public float speed;
    public bool explode;
    public Spell next;
    public Spell prev;

    public Spell(int id, float mag, float spd, bool expl)
    {
        spellID = id;
        magnitude = mag;
        speed = spd;
        explode = expl;
    }
}


public class Attack : MonoBehaviour
{
    public Spell currentSpell;
    public int spell;

    void Start()
    {
        // Create spells
        Spell magicBullet = new Spell(1, 10f, 20f, true);
        Spell ice = new Spell(2, 8f, 60f, false);
        Spell lightning = new Spell(3, 12f, 10f, false);

        // Link them in a circular doubly linked list
        magicBullet.next = ice;
        ice.next = lightning;
        lightning.next = magicBullet;

        magicBullet.prev = lightning;
        ice.prev = magicBullet;
        lightning.prev = ice;

        // Set starting spell
        currentSpell = magicBullet;
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
