using UnityEngine;
using System.Collections;

public class Consumable : Item {

    protected int quantity { get; set; }

    public Consumable( string id, string name, string description, int initialQuantity ) : base(id, name, description) {
        this.quantity = initialQuantity;
    }
}
