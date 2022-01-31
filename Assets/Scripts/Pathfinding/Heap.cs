using System;

namespace Pathfinding
{
	public class Heap<T> where T : IHeapItem<T>
	{
		public int Count => CurrentItemsCount;
	
		private T[] Items { get; set; }
		private int CurrentItemsCount { get; set; }

		public Heap(int maxHeapSize)
		{
			Items = new T[maxHeapSize];
		}

		public void Add(T item)
		{
			item.HeapIndex = CurrentItemsCount;
			Items[CurrentItemsCount] = item;
			SortUp(item);
			CurrentItemsCount++;
		}

		public T RemoveFirst()
		{
			T firstItem = Items[0];
			CurrentItemsCount--;
			Items[0] = Items[CurrentItemsCount];
			Items[0].HeapIndex = 0;
			SortDown(Items[0]);
			return firstItem;
		}

		public void Update(T item)
		{
			SortUp(item);
		}

		public bool Contains(T item)
		{
			return Equals(Items[item.HeapIndex], item);
		}

		private void SortDown(T item)
		{
			while (true) {
				int leftChildIndex = item.HeapIndex * 2 + 1;
				int rightChildIndex = item.HeapIndex * 2 + 2;

				if (leftChildIndex < CurrentItemsCount) {
					int swapIndex = leftChildIndex;

					if (rightChildIndex < CurrentItemsCount) {
						if (Items[leftChildIndex].CompareTo(Items[rightChildIndex]) < 0) {
							swapIndex = rightChildIndex;
						}
					}

					if (item.CompareTo(Items[swapIndex]) < 0) {
						SwapItems(item, Items[swapIndex]);
					}
					else {
						return;
					}
				}
				else {
					return;
				}
			}
		}

		private void SortUp(T item)
		{
			int parentIndex = (CurrentItemsCount - 1) / 2;

			while (true) {
				T parentItem = Items[parentIndex];
				if (item.CompareTo(parentItem) > 0) {
					SwapItems(item, parentItem);
				}
				else {
					break;
				}
			}
		}

		private void SwapItems(T itemA, T itemB)
		{
			Items[itemA.HeapIndex] = itemB;
			Items[itemB.HeapIndex] = itemA;
			int itemAIndex = itemA.HeapIndex;
			itemA.HeapIndex = itemB.HeapIndex;
			itemB.HeapIndex = itemAIndex;
		}
	
	
	}

	public interface IHeapItem<in T> : IComparable<T>
	{
		int HeapIndex { get; set; }
	}
}