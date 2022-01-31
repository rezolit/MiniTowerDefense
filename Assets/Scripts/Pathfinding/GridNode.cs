using UnityEngine;

namespace Pathfinding
{
	public class GridNode : IHeapItem<GridNode>
	{
		public bool       IsWalkable { get; }
		public int        GCost { get; set; }
		public int        HCost { get; set; }
		public int        HeapIndex { get; set; }
		public Vector3    WorldPosition { get; }
		public Vector2Int GridPos { get; private set; }
		public GridNode   Parent { get; set; }
		
		private int       FCost => GCost + HCost;

		public GridNode(bool isWalkable, Vector3 worldPosition, Vector2Int gridPos)
		{
			IsWalkable = isWalkable;
			WorldPosition = worldPosition;
			GridPos = gridPos;
		}

		public int CompareTo(GridNode other)
		{
			int compareResult = FCost.CompareTo(other.FCost);
			if (compareResult == 0) {
				compareResult = HCost.CompareTo(other.HCost);
			}

			return -compareResult;
		}
	}
}