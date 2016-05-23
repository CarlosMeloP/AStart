using UnityEngine;
using System.Collections.Generic;

public class GridManager : MonoBehaviour 
{
    private List<NodeVisual> visuals;

    [SerializeField] private int gridSizeX;
    [SerializeField] private int gridSizeY;

    [SerializeField] private float size = 1f;

    private readonly string path =  "NodeVisual";

    private static GridManager instance;

    private Grid grid;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
    }

    private void Start()
    {
        GenerateGrid();
    }

    private void GenerateGrid()
    {
        visuals = new List<NodeVisual>(gridSizeX * gridSizeY);

        grid = new Grid(gridSizeX, gridSizeY);

        List<Node> gridNodes = grid.GetNodes();

        GameObject nodeVisual = Resources.Load<GameObject>(path);

        NodeVisual visual;

        for (int y = 0; y < gridSizeY; y++)
        {
            for (int x = 0; x < gridSizeX; x++)
            {
                GameObject newNodeVisual = Instantiate(nodeVisual, new Vector3(x * size, 0f, y * size), Quaternion.identity) as GameObject;

                visual = newNodeVisual.GetComponent<NodeVisual>();
                visuals.Add(visual);
                visual.Initialize(gridNodes[x + y * gridSizeX]);
            }
        }
    }

    public Grid InternalGetGrid()
    {
        return grid;
    }

    public NodeVisual InternalGetVisual(Node node)
    {
        return InternalGetVisual(node.X, node.Y);
    }
       
    public NodeVisual InternalGetVisual(int x, int y)
    {
        return visuals[x + y * gridSizeX];
    }

    public Node InternalGetClosestNodeToWorldPosition(Vector3 position)
    {
        int x = Mathf.RoundToInt((position.x + size) / size) - 1;
        int y = Mathf.RoundToInt((position.z + size) / size) - 1;

        x = Mathf.Clamp(x, 0, gridSizeX - 1);
        y = Mathf.Clamp(y, 0, gridSizeY - 1);

        return grid.GetNodeFromPosition(x, y);
    }
        
    public Vector3 InternalGetWorldPosition(Node node)
    {
        return new Vector3(node.X * size, 0f, node.Y * size);
    }

    public static Grid GetGrid()
    {
        return instance.InternalGetGrid();
    }

    public static NodeVisual GetNodeVisual(Node node)
    {
        return instance.InternalGetVisual(node);
    }

    public static NodeVisual GetNodeVisual(int x, int y)
    {
        return instance.InternalGetVisual(x, y);
    }

    public static Node GetClosestNodeToWorldPosition(Vector3 position)
    {
        return instance.InternalGetClosestNodeToWorldPosition(position);
    }

    public static int GetDistance(Node nodeA, Node nodeB) 
    {
        int dstX = Mathf.Abs(nodeA.X - nodeB.X);
        int dstY = Mathf.Abs(nodeA.Y - nodeB.Y);

        if(dstX == 0 || dstY == 0)
        {
            return (dstX + dstY) * 10;
        }
        else if (dstX > dstY)
            return 14*dstY + 10* (dstX-dstY);
        return 14*dstX + 10 * (dstY-dstX);
    }

    public static Vector3 GetWorldPosition(Node node)
    {
        return instance.InternalGetWorldPosition(node);
    }
}
