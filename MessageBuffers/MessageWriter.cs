using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace MessageBuffers
{
    public class MessageWriter
    {
        MemoryStream content;
        public byte[] bytes
        {
            get
            {
                return content.ToArray();
            }
        }

        public int Length
        {
            get
            {
                return (int)content.Length;
            }
        }
        
        public MessageWriter()
        {
            content = new MemoryStream();
        }

        public MessageWriter(byte b)
        {
            content = new MemoryStream();
            writeByte(b);
        }

        public void writeByte(byte b)
        {
            content.WriteByte(b);
        }

        public void writeString(string s)
        {
			writeUShort((ushort)UTF8Encoding.UTF8.GetByteCount(s));
			writeChars(s);
        }

		public void writeChars(string s)
		{
			writeBytes(UTF8Encoding.UTF8.GetBytes(s));
		}

        public void writeBytes(byte[] b)
        {
            content.Write(b, 0, b.Length);
        }

        public void Clear()
        {
            content.SetLength(0);
        }

        public void writeUShort(ushort u)
        {
            content.WriteByte((byte)u);
            content.WriteByte((byte)(u >> 8));
        }

        public void writeDouble(double d)
        {
            content.Write(BitConverter.GetBytes(d), 0, 8);
        }

        public void writeFloat(float f)
        {
            content.Write(BitConverter.GetBytes(f), 0, 4);
        }

        public void writeInt(int i)
        {
            content.WriteByte((byte)(i));
            content.WriteByte((byte)(i >> 8));
            content.WriteByte((byte)(i >> 16));
            content.WriteByte((byte)(i >> 24));
        }

        public void writeUInt(uint i)
        {
            content.WriteByte((byte)(i));
            content.WriteByte((byte)(i >> 8));
            content.WriteByte((byte)(i >> 16));
            content.WriteByte((byte)(i >> 24));
        }

		public void writeUInt64(ulong i)
		{
			content.WriteByte((byte)(i));
			content.WriteByte((byte)(i >> 8));
			content.WriteByte((byte)(i >> 16));
			content.WriteByte((byte)(i >> 24));
			content.WriteByte((byte)(i >> 32));
			content.WriteByte((byte)(i >> 40));
			content.WriteByte((byte)(i >> 48));
			content.WriteByte((byte)(i >> 56));
		}

        public void write4D(int x, int y, double speed, double direction)
        {
            writeInt((int)x);
            writeInt((int)y);
            write24BitValue((ushort)Math.Round(direction * 11), (ushort)Math.Round((speed + 25.6) * 80));
            //writeBytes(Encode24BitValue((ushort)Math.Round(direction * 11), (ushort)Math.Round((speed + 25.6) * 80)));
        }

        /// <summary>
        /// DEPRECATED. Use MessageWriter.Write24BitValue instead. It's so much faster!
        /// </summary>
        /// <param name="val1"></param>
        /// <param name="val2"></param>
        /// <returns></returns>
        public static byte[] Encode24BitValue(ushort val1, ushort val2)
        {
            if (val1 > 4096) val1 = 4096;
            if (val2 > 4096) val2 = 4096;

            int comb = (val1 + (val2 << 12));
            byte[] b = new byte[3];
            b[0] = (byte)(comb >> 16);
            b[1] = (byte)((comb >> 8));//If this gives problems with sending 4d messages, a % 256 has been removed for thses two statements.
            b[2] = (byte)((comb));//* Same for write24BitValue ;)
            return b;
        }

        public void write24BitValue(ushort val1, ushort val2)
        {
            if (val1 > 4096) val1 = 4096;
            if (val2 > 4096) val2 = 4096;

            int comb = (val1 + (val2 << 12));
            content.WriteByte((byte)(comb >> 16));
            content.WriteByte((byte)((comb >> 8)));
            content.WriteByte((byte)((comb)));
        }

        public void writeUIntV(uint x)
        {
            if(x < 0x80)
            {
		        uint a = (x << 1) | 1;
		        writeByte((byte)a);
	        } else if(x < 0x4080) {
		        uint a = ((x - 0x80) << 2) | 2;
		        writeUInt(a);
	        } else if(x < 0x204080) {
		        uint a = ((x - 0x4080) << 3) | 4;
		        writeByte((byte)a);
                writeUShort((ushort)(a >> 8));
	        } else {
		        uint a = (x - 0x204080) << 3;
                writeUInt(a);
	        }
        }

        public static implicit operator byte[](MessageWriter m)
        {
            return m.bytes;
        }

    }
}
