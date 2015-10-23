﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
	public class BinaryHeapNode<TKey, TValue>
	{
		int _index;
		TKey _key;
		TValue _value;

		public int Parent { get { return (_index + 1) / 2 - 1; }}
		public int Left { get { return _index * 2 + 1; }}
		public int Right { get { return _index * 2 + 2; }}
		public int Index{ get { return _index; } set { _index = value; } }
		public TKey Key { get { return _key; } set { _key = value; } }
		public TValue Value { get { return _value; } set { _value = value; } }

		public BinaryHeapNode(int index, TKey key)
		{
			_index = index;
			_key = key;
		}

		public BinaryHeapNode(int index, TKey key, TValue value)
			: this(index, key)
		{
			_value = value;
		}
	}

	public class BinaryHeap<TKey, TValue> : IMaxHeap<TKey, TValue>, IMinHeap<TKey, TValue>
		where TKey : IComparable<TKey>
	{
		#region Fields

		List<BinaryHeapNode<TKey, TValue>> _nodes;
		int _heapSize;

		#endregion

		#region Constructors

		public BinaryHeap()
		{
			_nodes = new List<BinaryHeapNode<TKey, TValue>>();
		}

		public BinaryHeap(List<BinaryHeapNode<TKey, TValue>> nodes)
		{
			_nodes = nodes;
		}

		#endregion

		#region Properties

		public BinaryHeapNode<TKey, TValue> this[int index]
		{
			get { return _nodes[index]; }
		}

		public int Length
		{
			get { return _nodes.Count; }
		}

		public int HeapSize
		{
			get { return _heapSize; }
			set { _heapSize = value; }
		}

		#endregion

		#region IMaxHeap

		void IMaxHeap<TKey, TValue>.BuildMax()
		{
			int n = _nodes.Count;
			_heapSize = n;
			for(int i = n / 2 - 1; i >= 0; i--)
			{
				((IMaxHeap<TKey, TValue>)this).MaxHeapify(_nodes[i]);
			}
		}

		void IMaxHeap<TKey, TValue>.BuildMaxTail()
		{
			int n = _nodes.Count;
			_heapSize = n;
			for (int i = n / 2 - 1; i >= 0; i--)
				((IMaxHeap<TKey, TValue>)this).MaxHeapifyTail(_nodes[i]);
		}

		void IMaxHeap<TKey, TValue>.MaxHeapify(BinaryHeapNode<TKey, TValue> node)
		{
			int left = node.Left;
			int right = node.Right;
			int largest = node.Index;

			if (left < HeapSize && _nodes[left].Key.CompareTo(node.Key) > 0)
				largest = left;
			if (right < HeapSize && _nodes[right].Key.CompareTo(_nodes[largest].Key) > 0)
				largest = right;

			if (largest != node.Index)
			{
				Swap(node, _nodes[largest]);
				((IMaxHeap<TKey, TValue>)this).MaxHeapify(_nodes[largest]);
			}
		}

		void IMaxHeap<TKey, TValue>.MaxHeapifyTail(BinaryHeapNode<TKey, TValue> node)
		{
			int left = node.Left;
			int right = node.Right;
			int largest = -1;
			var currentNode = node;

			Console.WriteLine(ToStringTree());
			while (largest != currentNode.Index)
			{
				if (largest > -1)
					currentNode = _nodes[largest];
				largest = currentNode.Index;
				if (left < HeapSize && _nodes[left].Key.CompareTo(currentNode.Key) > 0)
					largest = left;
				if (right < HeapSize && _nodes[right].Key.CompareTo(_nodes[largest].Key) > 0)
					largest = right;

				if (largest != currentNode.Index)
				{
					Swap(currentNode, _nodes[largest]);
					Console.WriteLine(ToStringTree());
				}

				left = _nodes[largest].Left;
				right = _nodes[largest].Right;
			}
		}

		BinaryHeapNode<TKey, TValue> IMaxHeap<TKey, TValue>.Max()
		{
			if (_heapSize < 1)
				throw new Exception("heap is empty");

			return _nodes[0];
		}

		BinaryHeapNode<TKey, TValue> IMaxHeap<TKey, TValue>.ExtractMax()
		{
			if (_heapSize < 1)
				throw new Exception("heap is empty");

			var maxNode = _nodes[0];
			_nodes[0] = _nodes[_heapSize - 1];
			_nodes[0].Index = 0;
			_heapSize--;
			((IMaxHeap<TKey, TValue>)this).MaxHeapify(_nodes[0]);
			return maxNode;
		}

		BinaryHeapNode<TKey, TValue> IMaxHeap<TKey, TValue>.ExtractMaxTail()
		{
			if (_heapSize < 1)
				throw new Exception("heap is empty");

			var maxNode = _nodes[0];
			_nodes[0] = _nodes[_heapSize - 1];
			_nodes[0].Index = 0;
			_heapSize--;
			((IMaxHeap<TKey, TValue>)this).MaxHeapifyTail(_nodes[0]);
			return maxNode;
		}

		void IMaxHeap<TKey, TValue>.IncreaseKey(BinaryHeapNode<TKey, TValue> node, TKey newKey)
		{
			if (newKey.CompareTo(node.Key) < 0)
				throw new Exception("new key is lower then current");

			var currentNode = node;
			currentNode.Key = newKey;
			while (currentNode.Index > 0 && _nodes[currentNode.Parent].Key.CompareTo(currentNode.Key) < 0)
			{
				Swap(_nodes[currentNode.Parent], currentNode);
				currentNode = _nodes[currentNode.Parent];
			}
		}

		void IMaxHeap<TKey, TValue>.MaxInsert(TKey key, TValue value)
		{
			_heapSize++;
			_nodes.Add(new BinaryHeapNode<TKey, TValue>(_heapSize - 1, key, value));
			((IMaxHeap<TKey, TValue>)this).IncreaseKey(_nodes[_heapSize - 1], key);
		}

		void IMaxHeap<TKey, TValue>.MaxDelete(BinaryHeapNode<TKey, TValue> node)
		{
			if (_heapSize < 1)
				throw new Exception("heap is empty");

			var deletedIndex = node.Index;
			node = _nodes[_heapSize - 1];
			node.Index = deletedIndex;
			_heapSize--;
			((IMaxHeap<TKey, TValue>)this).MaxHeapify(node);
		}

		#endregion

		#region IMinHeap

		void IMinHeap<TKey, TValue>.BuildMin()
		{
			int n = _nodes.Count;
			_heapSize = n;
			for (int i = n / 2 - 1; i >= 0; i--)
			{
				((IMinHeap<TKey, TValue>)this).MinHeapify(_nodes[i]);
			}
		}

		void IMinHeap<TKey, TValue>.BuildMinTail()
		{
			int n = _nodes.Count;
			_heapSize = n;
			for (int i = n / 2 - 1; i >= 0; i--)
			{
				((IMinHeap<TKey, TValue>)this).MinHeapifyTail(_nodes[i]);
			}
		}

		void IMinHeap<TKey, TValue>.MinHeapify(BinaryHeapNode<TKey, TValue> node)
		{
			int left = node.Left;
			int right = node.Right;
			int lowest = node.Index;

			if (left < HeapSize && _nodes[left].Key.CompareTo(node.Key) < 0)
				lowest = left;
			if (right < HeapSize && _nodes[right].Key.CompareTo(_nodes[lowest].Key) < 0)
				lowest = right;

			if (lowest != node.Index)
			{
				Swap(node, _nodes[lowest]);
				((IMinHeap<TKey, TValue>)this).MinHeapify(_nodes[lowest]);
			}
		}

		void IMinHeap<TKey, TValue>.MinHeapifyTail(BinaryHeapNode<TKey, TValue> node)
		{
			int left = node.Left;
			int right = node.Right;
			int lowest = -1;
			var currentNode = node;

			while (lowest != currentNode.Index)
			{
				if (lowest > -1)
					currentNode = _nodes[lowest];
				lowest = currentNode.Index;
				if (left < HeapSize && _nodes[left].Key.CompareTo(currentNode.Key) < 0)
					lowest = left;
				if (right < HeapSize && _nodes[right].Key.CompareTo(_nodes[lowest].Key) < 0)
					lowest = right;

				if (lowest != currentNode.Index)
					Swap(currentNode, _nodes[lowest]);

				left = _nodes[lowest].Left;
				right = _nodes[lowest].Right;
			}
		}

		BinaryHeapNode<TKey, TValue> IMinHeap<TKey, TValue>.Min()
		{
			if (_heapSize < 1)
				throw new Exception("heap is empty");

			return _nodes[0];
		}

		BinaryHeapNode<TKey, TValue> IMinHeap<TKey, TValue>.ExtractMin()
		{
			if (_heapSize < 1)
				throw new Exception("heap is empty");

			var minNode = _nodes[0];
			_nodes[0] = _nodes[_heapSize - 1];
			_nodes[0].Index = 0;
			_heapSize--;
			((IMinHeap<TKey, TValue>)this).MinHeapify(_nodes[0]);
			return minNode;
		}

		BinaryHeapNode<TKey, TValue> IMinHeap<TKey, TValue>.ExtractMinTail()
		{
			if (_heapSize < 1)
				throw new Exception("heap is empty");

			var minNode = _nodes[0];
			_nodes[0] = _nodes[_heapSize - 1];
			_nodes[0].Index = 0;
			_heapSize--;
			((IMinHeap<TKey, TValue>)this).MinHeapifyTail(_nodes[0]);
			return minNode;
		}

		void IMinHeap<TKey, TValue>.DecreaseKey(BinaryHeapNode<TKey, TValue> node, TKey newKey)
		{
			if (newKey.CompareTo(node.Key) > 0)
				throw new Exception("new key is greater then current");

			var currentNode = node;
			currentNode.Key = newKey;
			while (currentNode.Index > 0 && _nodes[currentNode.Parent].Key.CompareTo(currentNode.Key) > 0)
			{
				Swap(_nodes[currentNode.Parent], currentNode);
				currentNode = _nodes[currentNode.Parent];
			}
		}

		void IMinHeap<TKey, TValue>.MinInsert(TKey key, TValue value)
		{
			_heapSize++;
			_nodes.Add(new BinaryHeapNode<TKey, TValue>(_heapSize - 1, key, value));
			((IMinHeap<TKey, TValue>)this).DecreaseKey(_nodes[_heapSize - 1], key);
		}

		void IMinHeap<TKey, TValue>.MinDelete(BinaryHeapNode<TKey, TValue> node)
		{
			if (_heapSize < 1)
				throw new Exception("heap is empty");

			var deletedIndex = node.Index;
			node = _nodes[_heapSize - 1];
			node.Index = deletedIndex;
			_heapSize--;
			((IMinHeap<TKey, TValue>)this).MinHeapify(node);
		}

		#endregion

		#region Methods

		static public void Swap(BinaryHeapNode<TKey, TValue> left, BinaryHeapNode<TKey, TValue> right)
		{
			TKey tmpKey = left.Key;
			left.Key = right.Key;
			right.Key = tmpKey;

			TValue tmpValue = left.Value;
			left.Value = right.Value;
			right.Value = tmpValue;
		}

		private string ToStringTree()
		{
			var treeBuilder = new StringBuilder();
			int h = (int)Math.Floor(Math.Log(_heapSize, 2));
			int levelN = (int)Math.Ceiling(_heapSize / Math.Pow(2, h + 1));
			int currLevelN = 0;
			for (int i = 0; i < _heapSize; i++)
			{
				currLevelN++;
				treeBuilder.Append(string.Format("[{0}]", _nodes[i].Key.ToString().PadLeft(2)));
				if(currLevelN == levelN)
				{
					h--;
					levelN = (int)Math.Floor(_heapSize / Math.Pow(2, h));
					currLevelN = 0;
					treeBuilder.AppendLine();
				}
			}
			return treeBuilder.ToString();
		}

		#endregion
	}
}