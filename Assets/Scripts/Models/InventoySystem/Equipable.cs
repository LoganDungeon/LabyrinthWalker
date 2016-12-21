using UnityEngine;
using System.Collections;

public abstract class Equipable : Item {

    // equipable items can not be stacked but equipped in various slots in the character Screen -> //TODO: implemented later

    protected int lifePoints { get; set; }

    public Equipable( string id, string name, string description, int lifePoints ) : base(id, name, description) {
        this.lifePoints = lifePoints;
    }
}
