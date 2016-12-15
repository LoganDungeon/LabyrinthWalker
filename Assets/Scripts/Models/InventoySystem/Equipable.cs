using UnityEngine;
using System.Collections;

public abstract class Equipable : Item {

    protected int lifePoints { get; set; }

    public Equipable( string id, string name, string description, int lifePoints ) : base(id, name, description) {
        this.lifePoints = lifePoints;
    }
}
