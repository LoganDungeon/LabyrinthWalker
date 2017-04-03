using UnityEngine;
using System.Collections;

public abstract class Equipable : Item
{

    // equipable items can not be stacked but equipped in various slots in the character Screen -> //TODO: implemented later

    protected int LifePoints
    {
        get;
        set;
    }

    protected Equipable(string id, string name, string description, int lifePoints) : base(id, name, description)
    {
        this.LifePoints = lifePoints;
    }
}
