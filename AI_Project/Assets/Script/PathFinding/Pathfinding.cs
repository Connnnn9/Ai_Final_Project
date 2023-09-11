using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    public Transform target, seeker;
    public GameObject player;
    mGrid grid;
    public List<Node> path;
    public float speed = 5.0f;
    int currentPathIndex;

    void Awake()
    {
        grid = GetComponent<mGrid>();
    }

    void Start()
    {
        currentPathIndex = 0;
    }

    void Update()
    {
        FindPath(seeker.position, target.position);
        MovePlayer();
    }

    void MovePlayer()
    {
        if (path == null || path.Count == 0 || currentPathIndex >= path.Count)
            return;

        Vector3 currentWaypoint = path[currentPathIndex].worldPosition;

        if (Vector3.Distance(player.transform.position, currentWaypoint) < 0.1f)
        {
            currentPathIndex++;
            if (currentPathIndex >= path.Count)
            {
                return;
            }
            currentWaypoint = path[currentPathIndex].worldPosition;
        }

        Vector3 direction = (currentWaypoint - player.transform.position).normalized;
        player.transform.position += direction * speed * Time.deltaTime;
    }
    void FindPath(Vector3 startPos, Vector3 targetPos)
    {
        Node startNode = grid.NodeFromWorldPoint(startPos);
        Node targetNode = grid.NodeFromWorldPoint(targetPos);

        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();
        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            Node currentNode = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].fCost < currentNode.fCost ||
                    (openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost))
                {
                    currentNode = openSet[i];
                }
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            if (currentNode == targetNode)
            {
                RetracePath(startNode, targetNode);
                return;
            }

            foreach (Node neighbor in grid.GetNeighbors(currentNode))
            {
                if (!neighbor.walkable || closedSet.Contains(neighbor))
                {
                    continue;
                }

                int newMovementCostToNeighbor = currentNode.gCost + GetDistance(currentNode, neighbor);
                if (newMovementCostToNeighbor < neighbor.gCost || !openSet.Contains(neighbor))
                {
                    neighbor.gCost = newMovementCostToNeighbor;
                    neighbor.hCost = GetDistance(neighbor, targetNode);
                    neighbor.parent = currentNode;

                    if (!openSet.Contains(neighbor))
                    {
                        openSet.Add(neighbor);
                    }
                }
            }
        }
    }

    void RetracePath(Node startNode, Node endNode)
    {
        List<Node> newPath = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            newPath.Add(currentNode);
            currentNode = currentNode.parent;
        }
        newPath.Reverse();
        this.path = newPath; // assign to class-level path variable
    }

    int GetDistance(Node nodeA, Node nodeB)
    {
        int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if (dstX > dstY)
            return 14 * dstY + 10 * (dstX - dstY);

        return 14 * dstX + 10 * (dstY - dstX);
    }
}
