﻿using Algorithms.Common;
using System;
using System.Collections.Generic;
using Xunit;

namespace Algorithms.AlgorithmsTests.Common
{
	public class QueueTestCaseNode<T>
	{
		public int Direction { get; set; }
		public T Value { get; set; }
		public int Count { get; set; }
	}

	public class QueueTestCase<T>
	{
		public string Name;
		public List<QueueTestCaseNode<T>> Nodes { get; set; }
	}

	public class QueueTestsFixture
	{
		readonly List<QueueTestCase<string>> _testCases;
		public List<QueueTestCase<string>> TestCases => _testCases;

		public QueueTestsFixture()
		{
			_testCases = new List<QueueTestCase<string>>
			{
				new QueueTestCase<string>
				{
					Name = "test case 1",
					Nodes = new List<QueueTestCaseNode<string>>
					{
						new QueueTestCaseNode<string>
						{
							Direction = 1,
							Value = "value_1",
							Count = 1
						},
						new QueueTestCaseNode<string>
						{
							Direction = 1,
							Value = "value_2",
							Count = 2
						},
						new QueueTestCaseNode<string>
						{
							Direction = 1,
							Value = "value_3",
							Count = 3
						},
						new QueueTestCaseNode<string>
						{
							Direction = -1,
							Value = "value_1",
							Count = 2
						}
					}
				}
			};
		}
	}

	public class QueueTests : IClassFixture<QueueTestsFixture>
	{
		readonly QueueTestsFixture _queueTestsFixture;

		public QueueTests(QueueTestsFixture queueTestsFixture)
		{
			_queueTestsFixture = queueTestsFixture;
		}

		[Fact]
		public void QueueTest()
		{
			var queue = new Queue<string>();
			foreach (var testCase in _queueTestsFixture.TestCases)
			{
				int n = testCase.Nodes.Count;

				Console.WriteLine("--------------------------------------------------------");
				Console.WriteLine("[Name:{0}]", testCase.Name);
				for (int i = 0; i < n; i++)
				{
					var node = testCase.Nodes[i];
					Console.WriteLine("Node{0}:[Direction:{1}]", i, node.Direction == 1 ? "enqueue" : "Dequeue");
					if (node.Direction == 1)
						queue.Enqueue(node.Value);
					else
					{
						var actualValue = queue.Dequeue();
						Console.WriteLine("Node{0}:[ExpectedValue:{1}], [ActualValue:{2}]", i, node.Value, actualValue);
						Assert.Equal(node.Value, actualValue);
					}
					int actualCount = queue.Count;
					Console.WriteLine("Node{0}:[ExpectedCount:{1}], [ActualCount:{2}]", i, node.Count, actualCount);
					Assert.Equal(node.Count, actualCount);
				}
			}
		}
	}
}