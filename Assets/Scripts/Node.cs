using UnityEngine;

[System.Serializable]
public class Node
{
    [SerializeField] private int xPosition;
    [SerializeField] private int yPosition;
        
    public Node(int x, int y, TerrainType type)
    {
        xPosition = x;
        yPosition = y;
        TerrainType = type;
    }

    public int X
    {
        get { return xPosition; }
    }

    public int Y
    {
        get { return yPosition; }
    }

    private TerrainType terrainType;

    public TerrainType TerrainType
    {
        get { return terrainType; }
        set
        {
            terrainType = value;

            Cost = Tiles.GetCost(terrainType);
        }
    }

    public int Cost
    {
        get;
        set;
    }

    #region AStar specific
    private Node parent;

    public Node Parent
    {
        get { return parent; }
        set { parent = value; }
    }
        
    public int GCost { get; set; }
    public int HCost { get; set; }

    public int FCost { get { return GCost + HCost; } }
    #endregion
}
