using UnityEngine;
using System.Collections;

public class Consumable : Item {

    public int quantity {
        get;
        protected set;
    }

    public int maxQuantity { get; protected set; }

    public Consumable( string id, string name, string description, int initialQuantity, int maxQuantity ) : base(id, name, description) {
        this.quantity = initialQuantity;
        this.maxQuantity = maxQuantity;
    }
}
