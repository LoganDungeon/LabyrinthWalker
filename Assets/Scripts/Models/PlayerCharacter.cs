using System;
using System.Collections;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Object = UnityEngine.Object;
using UnityEngine;

public class PlayerCharacter : Character, IXmlSerializable
{

    // Inventory of the Player
    private Inventory _playerInventory;

    // saturation will go down with the time and stamina will be used to sprint
    public int Saturation
    {
        get;
        protected set;
    }

    public int Stamina
    {
        get;
        protected set;
    }

    // visuals of the Character
    public PlayerCharacterGOController PcgoC;

    // creates the Character for the Player. tile - position to spawn at, health, stamina, 
    // saturation - stats of the player, inventorySlots - Number of Spaces in the Inventory
    public PlayerCharacter(Tile tile, int health, int saturation, int stamina, int inventorySlots) : base(tile, health)
    {
        PcgoC = Object.FindObjectOfType<PlayerCharacterGOController>();
        PcgoC.CreateCharacterGameObject(tile);

        this.Saturation = saturation;
        this.Stamina = stamina;

        _playerInventory = new Inventory(inventorySlots);
    }

    public int GetInventorySpaces()
    {
        return _playerInventory.Slots.Length;
    }

    public XmlSchema GetSchema()
    {
        return null;
    }

    public void WriteXml(XmlWriter writer)
    {
        writer.WriteStartElement("PlayerStats");
        writer.WriteAttributeString("Health", this.Health.ToString());
        writer.WriteAttributeString("Stamina", this.Stamina.ToString());
        writer.WriteAttributeString("Saturation", this.Saturation.ToString());
        writer.WriteAttributeString("InventorySlots", this.GetInventorySpaces().ToString());
        writer.WriteEndElement();
    }

    public void ReadXml(XmlReader reader)
    {
        reader.ReadToDescendant("PlayerStats");
        /*reader.MoveToAttribute("Health");
        this.Health = reader.ReadContentAsInt();
        reader.MoveToAttribute("Stamina");
        this.Stamina = reader.ReadContentAsInt();
        reader.MoveToAttribute("Saturation");
        this.Saturation = reader.ReadContentAsInt();
        reader.MoveToAttribute("InventorySlots");
        this._playerInventory = new Inventory(reader.ReadContentAsInt());*/
        this.Health = int.Parse(reader.GetAttribute("Health") ?? "1");
        this.Stamina = int.Parse(reader.GetAttribute("Stamina") ?? "1");
        this.Saturation = int.Parse(reader.GetAttribute("Saturation") ?? "1");
        this._playerInventory = new Inventory(int.Parse(reader.GetAttribute("InventorySlots") ?? "1"));
    }
}
