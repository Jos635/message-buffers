using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace MessageBuffers
{
    public class MessageReader
    {
        MemoryStream content;
        public byte[] bytes
        {
            get
            {
                return content.ToArray();
            }
        }

        public byte[] remainingBytes
        {
            get
            {
                long oldPosition = content.Position;
                byte[] buffer = new byte[content.Length - content.Position];
                content.Read(buffer, 0, buffer.Length);
                content.Seek(oldPosition, SeekOrigin.Begin);
                return buffer;
            }
        }

        public int remainingLength
        {
            get
            {
                return (int)(content.Length - content.Position);
            }
        }

		public long Length
		{
			get
			{
				return content.Length;
			}
		}

        public int pos
        {
            get
            {
                return (int)content.Position;
            }
            set
            {
                content.Position = value;
            }
        }

        public MessageReader(byte[] data)
        {
            content = new MemoryStream(data);
        }

        public byte readByte()
        {
			int b = content.ReadByte();
			if(b == -1)
			{
				throw new EndOfStreamException("An attempt was made to read past the end of the stream.");
			}

			return (byte)b;
        }

        public byte[] peekBytes(int length)
        {
            byte[] buffer = new byte[length];
            content.Read(buffer, 0, length);
            content.Seek(-length, SeekOrigin.Current);
            return buffer;
        }

        public int readUShort()
        {
            return readByte() | (readByte() << 8);
        }

        public double readDouble()
        {
            byte[] buffer = new byte[8];
            content.Read(buffer, 0, 8);
            return BitConverter.ToDouble(buffer, 0);
        }

        public uint readUInt()
        {
            return (uint)readByte() | (uint)(readByte() << 8) | (uint)(readByte() << 16) | (uint)(readByte() << 24);//BitConverter.ToUInt32(buffer, 0);
        }

        public int readInt()
        {
			return readByte() | (readByte() << 8) | (readByte() << 16) | (readByte() << 24);
        }

        public float readFloat()
        {
            byte[] buffer = new byte[4];
            content.Read(buffer, 0, 4);
            return BitConverter.ToSingle(buffer, 0);
        }

        public string readString()
        {
			return readChars(readUShort());
        }

		public string readChars(int length)
		{
			return Encoding.UTF8.GetString(readBytes(length));
		}

        public byte[] readBytes(int length)
        {
            byte[] temp = new byte[length];
            content.Read(temp, 0, length);
            return temp;
        }

        public long readUIntV()
        {
            byte[] tmp = peekBytes(1);
            int a = tmp[0];
            if ((a & 1) > 0)
            {
                return a >> 1;
            }
            else if ((a & 2) > 0)
            {
                if (remainingLength < 1)
                {
                    return 0;
                }
                a = a | (tmp[1] << 8);
                return (a >> 2) + 0x80;
            }
            else if ((a & 4) > 0)
            {
                if (remainingLength < 1)
                {
                    return -1;
                }
                a = a | (tmp[1] << 8) | (tmp[2] << 16);
                return (a >> 3) + 0x4080;
            }
            return -1;
        }

        public int readIUShort()
        {
            return (readByte() << 8) | readByte();
        }

        public ulong readIUInt64()
        {
            return ((ulong)readByte() << 56) +
                ((ulong)readByte() << 48) +
                ((ulong)readByte() << 40) +
                ((ulong)readByte() << 32) +
                ((ulong)readByte() << 24) +
                ((ulong)readByte() << 16) +
                ((ulong)readByte() << 8) +
                (ulong)readByte();
        }

		public ulong readUInt64()
		{
			return ((ulong)readByte() << 0) +
				((ulong)readByte() << 8) +
				((ulong)readByte() << 16) +
				((ulong)readByte() << 24) +
				((ulong)readByte() << 32) +
				((ulong)readByte() << 40) +
				((ulong)readByte() << 48) +
				((ulong)readByte() << 56);
		}
    }
}
