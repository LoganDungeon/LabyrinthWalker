using UnityEngine;
using System.Collections;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

public class Tile : IXmlSerializable
{

    // this class is a pure data class
    // it contains various informations about 1 Tile, or "cell" of the 
    // planned Labyrinth

    // declares, if there will be generated a wall on this tile
    public bool IsWall
    {
        get;
        set;
    }

    // declares if the tile was already visited by the depth first search algorithm
    public bool Visited
    {
        get;
        set;
    }

    public bool IsRoomWall
    {
        get;
        set;
    }

    // Coordinates;
    public int X
    {
        get;
        set;
    }

    public int Z
    {
        get;
        set;
    }

    // Emtpy Constructor just for Serialization
    public Tile()
    {
    }

    public Tile(int x, int z)
    {
        this.X = x;
        this.Z = z;
        this.Visited = false;
        this.IsWall = true;
        this.IsRoomWall = false;
    }

    public XmlSchema GetSchema()
    {
        return null;
    }

    public void WriteXml(XmlWriter writer)
    {
        writer.WriteAttributeString("X", this.X.ToString());
        writer.WriteAttributeString("Z", this.Z.ToString());
        writer.WriteAttributeString("IsWall", this.IsWall.ToString().ToLower());
        writer.WriteAttributeString("IsRoomWall", this.IsRoomWall.ToString().ToLower());
    }

    public void ReadXml(XmlReader reader)
    {
        reader.MoveToAttribute("X");
        this.X = reader.ReadContentAsInt();
        reader.MoveToAttribute("Z");
        this.Z = reader.ReadContentAsInt();
        reader.MoveToAttribute("IsWall");
        this.IsWall = reader.ReadContentAsBoolean();
        reader.MoveToAttribute("IsRoomWall");
        this.IsRoomWall = reader.ReadContentAsBoolean();
    }
}
