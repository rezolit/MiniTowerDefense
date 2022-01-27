using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding
{
	public class PathfindingAlgorithm : MonoBehaviour
	{
		[SerializeField]
		private int diagonalCost;

		[SerializeField]
		private int directCost;
		
		private Grid _grid;

		[Header("Debug")]
		[SerializeField]
		private Transform targetFrom;
		
		[SerializeField]
		private Transform targetTo;

		[SerializeField]
		private bool showPath;

		private void Awake()
		{
			_grid = GetComponent<Grid>();
		}

		private void Update()
		{
			_grid.Path = FindPath(targetFrom.position, targetTo.position);
		}

		private List<GridNode> FindPath(Vector3 from, Vector3 to)
		{
			GridNode nodeFrom = _grid.GetGridNodeFromWorldPosition(from);
			GridNode nodeTo = _grid.GetGridNodeFromWorldPosition(to);

			List<GridNode> openNodes = new List<GridNode>();
			HashSet<GridNode> closedNodes = new HashSet<GridNode>();
			openNodes.Add(nodeFrom);

			while (openNodes.Count > 0) {
				
				GridNode currentNode = openNodes[0];
				for (int i = 1; i < openNodes.Count; ++i) {
					if (openNodes[i].FCost < currentNode.FCost ||
					    openNodes[i].FCost == currentNode.FCost &&
					    openNodes[i].HCost < currentNode.HCost) {
						
						currentNode = openNodes[i];
					}
				}
				openNodes.Remove(currentNode);
				closedNodes.Add(currentNode);

				if (currentNode == nodeTo) {
					return RetracePath(nodeFrom, nodeTo);
				}

				foreach (GridNode neighbour in _grid.GetNodeNeighbours(currentNode)) {
					if (neighbour.IsWalkable == false || closedNodes.Contains(neighbour)) {
						continue;
					}

					int newPathLength = currentNode.GCost + PathLength(currentNode, neighbour);
					
					if (newPathLength < neighbour.GCost || openNodes.Contains(neighbour) == false) {
						neighbour.GCost = newPathLength;
						neighbour.HCost = PathLength(neighbour, nodeTo);
						neighbour.Parent = currentNode;
						
						if (openNodes.Contains(neighbour) == false) {
							openNodes.Add(neighbour);
						}
					}
				}
			}

			return null;
		}

		private List<GridNode> RetracePath(GridNode from, GridNode to)
		{
			List<GridNode> path = new List<GridNode>();
			GridNode currentNode = to;
			while (currentNode != from) {
				path.Add(currentNode);
				currentNode = currentNode.Parent;
			}

			return path;
		}

		private int PathLength(GridNode from, GridNode to)
		{
			int distanceX = Math.Abs(from.GridPos.x - to.GridPos.x);
			int distanceY = Math.Abs(from.GridPos.y - to.GridPos.y);

			if (distanceX > distanceY) {
				return diagonalCost * distanceY + directCost * (distanceX - distanceY);
			}
			return diagonalCost * distanceX + directCost * (distanceY - distanceX);
		}
	}
}
