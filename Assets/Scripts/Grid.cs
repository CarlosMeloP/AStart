using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Grid
{
    [SerializeField] private int sizeX;
    [SerializeField] private int sizeY;
    [SerializeField] private List<Node> nodes;

    public Grid(int gridSizeX, int gridSizeY)
    {
        sizeX = gridSizeX;
        sizeY = gridSizeY;

        nodes = new List<Node>();

        for (int y = 0; y < sizeY; y++)
        {
            for (int x = 0; x < sizeX; x++)
            {
                Node newNode = new Node(x, y, TerrainType.Grass);

                nodes.Add(newNode);
            }
        }
    }

    public List<Node> GetNodes()
    {
        return nodes;
    }

    public List<Node> GetNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();

        int xPosition;
        int yPosition;

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (!( x == 0 && y == 0))
                {
                    xPosition = node.X + x;
                    yPosition = node.Y + y;

                    if (AssetWithinBorders(xPosition, yPosition))
                    {
                        neighbours.Add(GetNodeFromPosition(xPosition, yPosition));
                    }
                }
            }
        }

        return neighbours;
    }

    private bool AssetWithinBorders(int x, int y)
    {
        return x >= 0 && x < sizeX && y >= 0 && y < sizeY;
    }

    public Node GetNodeFromPosition(int x, int y)
    {
        return nodes[x + y * sizeX];
    }
}
