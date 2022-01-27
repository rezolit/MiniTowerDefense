using UnityEngine;

namespace Pathfinding
{
	public class GridNode
	{
		public bool       IsWalkable { get; }
		public Vector3    WorldPosition { get; }
		public int        GCost { get; set; }
		public int        HCost { get; set; }
		public GridNode   Parent { get; set; }
		public Vector2Int GridPos { get; set; }
		public int        FCost => GCost + HCost;

		public GridNode(bool _isWalkable, Vector3 _worldPosition, Vector2Int _gridPos)
		{
			IsWalkable = _isWalkable;
			WorldPosition = _worldPosition;
			GridPos = _gridPos;
		}
	}
}