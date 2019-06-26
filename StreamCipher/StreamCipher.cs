using System;

namespace LearnSomeCode
{
	public class StreamCipher
	{
		// ----------------------------------------------------------------------------------------
		// Responsible for generating a certain cipher key
		public readonly int Seed;

		// Dictates how large the cipher key should be (16kb in this case)
		private const int KEYSIZE = 16384;
		private byte[] key;

		// Keeps track of which byte from the cipher key is currently
		// being used for encryption / decryption
		private int encIndex = 0;
		private int decIndex = 0;
		// ----------------------------------------------------------------------------------------



		// ----------------------------------------------------------------------------------------
		public StreamCipher()
		{
			Seed = CreateSeed();
			CreateKey();
		}

		public StreamCipher(int seed)
		{
			Seed = seed;
			CreateKey();
		}
		// ----------------------------------------------------------------------------------------



		// ----------------------------------------------------------------------------------------
		public byte[] Encrypt(byte[] data)
		{
			byte[] output = new byte[data.Length];
			for (int i = 0; i < data.Length; i++)
			{ output[i] = (byte)(data[i] ^ NextEByte()); }

			return output;
		}

		public byte[] Decrypt(byte[] data)
		{
			byte[] output = new byte[data.Length];
			for (int i = 0; i < data.Length; i++)
			{ output[i] = (byte)(data[i] ^ NextDByte()); }

			return output;
		}
		// ----------------------------------------------------------------------------------------



		// ----------------------------------------------------------------------------------------
		private int CreateSeed()
		{
			return (new Random().Next());
		}

		private void CreateKey()
		{
			key = new byte[KEYSIZE];
			new Random(Seed).NextBytes(key);
		}

		private byte NextEByte()
		{
			if (encIndex == KEYSIZE) { encIndex = 0; }
			return key[encIndex++];
		}

		private byte NextDByte()
		{
			if (decIndex == KEYSIZE) { decIndex = 0; }
			return key[decIndex++];
		}
		// ----------------------------------------------------------------------------------------
	}
}
