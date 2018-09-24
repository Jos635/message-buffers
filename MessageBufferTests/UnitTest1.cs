using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MessageBuffers;
using System.IO;

namespace MessageBufferTests
{
	[TestClass]
	public class UnitTest1
	{
		[TestMethod]
		public void TestInt()
		{
			int[] data = { 
				-423747349, 
				1, 
				10, 
				100,
				1000,
				10000,
				100000,
				-50, 
				2147483640, 
				-2147483640, 
				2147640,
				47483640 
			};

			MessageWriter w = new MessageWriter();
			foreach(int i in data)
			{
				w.writeInt(i);
			}

			MessageReader r = new MessageReader(w);
			foreach (int i in data)
			{
				Assert.AreEqual(i, r.readInt());
			}
		}

		[TestMethod]
		public void TestUInt()
		{
			uint[] data = {
				4237449343, 
				1, 
				10, 
				100,
				1000,
				10000,
				100000,
				50, 
				2147483640, 
				2147483640, 
				2147640,
				47483640 
			};

			MessageWriter w = new MessageWriter();
			foreach (uint i in data)
			{
				w.writeUInt(i);
			}

			MessageReader r = new MessageReader(w);
			foreach (uint i in data)
			{
				Assert.AreEqual(i, r.readUInt());
			}
		}

		[TestMethod]
		public void TestByte()
		{
			MessageWriter w = new MessageWriter();
			for (byte b = 0; b < 255; b++ )
			{
				w.writeByte(b);
			}

			MessageReader r = new MessageReader(w);
			for (byte b = 0; b < 255; b++)
			{
				Assert.AreEqual(b, r.readByte());
			}
		}

		[TestMethod]
		public void TestUShort()
		{
			MessageWriter w = new MessageWriter();
			for (ushort b = 0; b < 65535; b++)
			{
				w.writeUShort(b);
			}

			MessageReader r = new MessageReader(w);
			for (ushort b = 0; b < 65535; b++)
			{
				Assert.AreEqual(b, r.readUShort());
			}
		}

		[TestMethod]
		[ExpectedException(typeof(EndOfStreamException))]
		public void TestOutOfBoundsRead()
		{
			MessageReader r = new MessageReader(new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 });
			r.readDouble();
			r.readDouble();
			r.readByte();
		}
	}
}
