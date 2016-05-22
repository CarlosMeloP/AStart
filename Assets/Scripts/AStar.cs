using UnityEngine;
using System.Collections.Generic;

public class AStar : MonoBehaviour 
{
    [SerializeField] private bool showCostVisuals = false;

    [SerializeField] private Color gizmoColor;

	private Grid grid;

    private int sizeX, sizeY;

    [SerializeField] private Transform start;
    [SerializeField] private Transform target;

    private void OnDrawGizmos()
    {
        if (path != null && path.Count > 0)
        {
            Vector3 offset = Vector3.up;

            for (int i = 0; i < path.Count; i++)
            {
                Gizmos.color = gizmoColor;
                Gizmos.DrawWireCube(GridManager.GetWorldPosition(path[i]), Vector3.one); 
            }
        }
    }

    public List<Node> FindPath(Node startNode, Node targetNode) 
    {
        grid = GridManager.GetGrid();

        List<Node> openSet = new List<Node>();
        openSet.Add(startNode);

        List<Node> closedSet = new List<Node>();

        while (openSet.Count > 0) 
        {
            Node currentNode = openSet[0];
            for (int i = 1; i < openSet.Count; i ++) 
            {
                if (openSet[i].FCost < currentNode.FCost 
                    || (openSet[i].FCost == currentNode.FCost 
                        && openSet[i].HCost < currentNode.HCost))
                {
                    currentNode = openSet[i];
                }
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            if (currentNode == targetNode)
            {
                return RetracePath(startNode, targetNode);
            }
                
            foreach (Node neighbour in grid.GetNeighbours(currentNode)) 
            {
                if (neighbour.Cost < 10 && !closedSet.Contains(neighbour))
                {
                    int costToNeighbour = currentNode.GCost + GetDistance(currentNode, neighbour) + neighbour.Cost;

                    if (costToNeighbour < neighbour.GCost || !openSet.Contains(neighbour)) 
                    {
                        neighbour.GCost = costToNeighbour;
                        neighbour.HCost = GetDistance(neighbour, targetNode);
                        neighbour.Parent = currentNode;

                        if (showCostVisuals)
                        {
                            GridManager.GetNodeVisual(neighbour).EnableCostVisuals(true);
                        }

                        if (!openSet.Contains(neighbour))
                        {
                            openSet.Add(neighbour);
                        }
                    }
                }
            }
        }

        return null;
	}

    private List<Node> path;

    private List<Node> RetracePath(Node startNode, Node endNode) 
    {
        if (path != null)
        {
            path.Clear();
        }
		path = new List<Node>();

		Node currentNode = endNode;

		while (currentNode != startNode) 
        {
			path.Add(currentNode);
            currentNode = currentNode.Parent;
		}

		path.Reverse();

        return path;
	}

	private int GetDistance(Node nodeA, Node nodeB) 
    {
        int distance;

        int xDistance = Mathf.Abs(nodeA.X - nodeB.X);
        int yDistance = Mathf.Abs(nodeA.Y - nodeB.Y);

        if (xDistance == 0 || yDistance == 0)
        {
            distance = (xDistance + yDistance) * 10;
        }
        else if (xDistance > yDistance)
        {
            distance = 14 * yDistance + 10 * (xDistance - yDistance);
        }
        else
        {
            distance = 14 * xDistance + 10 * (yDistance - xDistance);
        }

        return distance;
	}
}
