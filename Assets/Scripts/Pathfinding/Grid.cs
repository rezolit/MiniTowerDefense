using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding
{
	public class Grid : MonoBehaviour
	{
		[SerializeField]
		private Vector2 gridSize;

		[SerializeField]
		private float nodeSize;

		[SerializeField]
		private float distanceFromObstacles;
		
		[SerializeField]
		private float agentsHeight;

		[SerializeField]
		private LayerMask unwalkableLayer;

		[Header("Debug")]
		[SerializeField]
		private bool showGrid;

		private GridNode[,] _grid;
		private float _nodeHalfSize;
		private Vector2Int _nodesCount;

		public List<GridNode> Path { get; set; }
		public int MaxSize => _nodesCount.x * _nodesCount.y;
		
		private void Start()
		{
			_nodeHalfSize = nodeSize / 2.0f;
			_nodesCount = new Vector2Int(
				Mathf.RoundToInt(gridSize.x / nodeSize),
				Mathf.RoundToInt(gridSize.y / nodeSize));
			
			CreateGrid();
		}

		private void CreateGrid()
		{
			_grid = new GridNode[_nodesCount.x, _nodesCount.y];
			var position = transform.position;
			Vector3 leftHigherBorder = new Vector3(
				position.x - gridSize.x / 2,
				position.y,
				position.z - gridSize.y / 2);
			

			for (int i = 0; i < _nodesCount.x; i++) {
				for (int j = 0; j < _nodesCount.y; j++) {
					Vector3 worldPos = leftHigherBorder + new Vector3(i * nodeSize + _nodeHalfSize, 0.0f, j * nodeSize + _nodeHalfSize);
					bool isWalkable = Physics.CheckCapsule(worldPos, worldPos + Vector3.up * agentsHeight,_nodeHalfSize + distanceFromObstacles, unwalkableLayer) == false;
					_grid[i, j] = new GridNode(isWalkable, worldPos, new Vector2Int(i, j));
				}
			}
		}

		public GridNode GetGridNodeFromWorldPosition(Vector3 worldPosition)
		{
			Vector2 positionPercentage = new Vector2(
				(worldPosition.x + gridSize.x / 2) / gridSize.x,
				(worldPosition.z + gridSize.y / 2) / gridSize.y);

			int x = Mathf.RoundToInt((_nodesCount.x - 1) * positionPercentage.x);
			int y = Mathf.RoundToInt((_nodesCount.y - 1) * positionPercentage.y);
			
			return _grid[x,y];
		}

		public List<GridNode> GetNodeNeighbours(GridNode node)
		{
			List<GridNode> neighbours = new List<GridNode>();
			
			for (int i = -1; i <= 1; i++) {
				for (int j = -1; j <= 1; j++) {
					if (i == j) {
						continue;
					}
					int x = node.GridPos.x + i;
					int y = node.GridPos.y + j;
					if (x >= 0 && x < _nodesCount.x &&
					    y >= 0 && y < _nodesCount.y) {
						
						neighbours.Add(_grid[x, y]);
					}
				}
			}

			return neighbours;
		}

		private void OnDrawGizmos()
		{
			Gizmos.color = Color.blue;
			Gizmos.DrawWireCube(transform.position, new Vector3(gridSize.x, 1.0f, gridSize.y));
			
			if (showGrid && _grid != null) {
				
				foreach (var node in _grid) {
					Gizmos.color = node.IsWalkable ? Color.green : Color.red;
					if (Path != null && Path.Contains(node)) {
						Gizmos.color = Color.magenta;
					}
					Gizmos.DrawCube(node.WorldPosition, Vector3.one * nodeSize);
				}
			}
		}
	}
}
