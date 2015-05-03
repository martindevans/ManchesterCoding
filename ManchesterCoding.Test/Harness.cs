using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;

namespace ManchesterCoding.Test
{
    public static class Harness
    {
        private static readonly List<KeyValuePair<string, bool[]>> _testCases = new List<KeyValuePair<string, bool[]>> {
            new KeyValuePair<string, bool[]>("all true", new bool[] { true, true, true, true, true }),
            new KeyValuePair<string, bool[]>("all false", new bool[] { false, false, false, false, false }),

            new KeyValuePair<string, bool[]>("alternate", new bool[] { false, true, false, true, false }),
            new KeyValuePair<string, bool[]>("alternate2", new bool[] { true, false, true, false, true }),

            new KeyValuePair<string, bool[]>("true run", new bool[] { true, true, true, true, false }),
            new KeyValuePair<string, bool[]>("false run", new bool[] { false, false, false, false, true }),

            new KeyValuePair<string, bool[]>("0", new bool[] { false, false, false }),
            new KeyValuePair<string, bool[]>("1", new bool[] { false, false, true }),
            new KeyValuePair<string, bool[]>("2", new bool[] { false, true, false }),
            new KeyValuePair<string, bool[]>("3", new bool[] { false, true, true }),
            new KeyValuePair<string, bool[]>("4", new bool[] { true, false, false }),
            new KeyValuePair<string, bool[]>("5", new bool[] { true, false, true }),
            new KeyValuePair<string, bool[]>("6", new bool[] { true, true, false }),
            new KeyValuePair<string, bool[]>("7", new bool[] { true, true, true }),
        };

        public static void TestEncodeDecode<E, D>()
            where E : IEncoder, new()
            where D : IDecoder, new()
        {
            foreach (var testCase in _testCases)
                EncodeDecode(new E(), new D(), testCase.Value, testCase.Key);
        }

        private static void EncodeDecode(IEncoder enc, IDecoder dec, IList<bool> data, string msg)
        {
            var w = new Writer();
            foreach (bool d in data)
                enc.Encode(d, w);

            bool[] decoded = new bool[data.Count];
            var r = new Reader(w.Bits);
            for (int i = 0; i < data.Count; i++)
                decoded[i] = dec.Decode(r);

            for (int i = 0; i < data.Count; i++)
                Assert.AreEqual(data[i], decoded[i], "TEST = " + msg);
        }

        public static void TestRecover<E, D>()
            where E : IEncoder, new()
            where D : IDecoder, new()
        {
            foreach (var testCase in _testCases)
            {
                Recover(new E(), new D(), testCase.Value, false, testCase.Key);
                Recover(new E(), new D(), testCase.Value, true, testCase.Key);
            }
        }

        private static void Recover(IEncoder enc, IDecoder dec, IList<bool> data, bool throw2, string msg)
        {
            var w = new Writer();
            foreach (bool d in data)
                enc.Encode(d, w);

            var r = new Reader(w.Bits);
            var decoded = new Writer();

            //Read a single bit of data to throw off the stream
            r.Read();

            //Read a second bit, which actually resyncs the stream
            if (throw2)
                r.Read();
            try
            {
                //Recover the stream
                dec.Recover(r, decoded);

                //Read the rest of the data
                for (int i = 1; i < data.Count; i++)
                    decoded.Write(dec.Decode(r));
            }
            catch (EndOfStreamException) { }

            for (int i = decoded.Bits.Count - 1; i >= 0; i--)
                Assert.AreEqual(data[data.Count - 1 - i], decoded.Bits[decoded.Bits.Count - 1 - i], msg);
        }

        private class Writer
            : IBitWriter
        {
            public readonly List<bool> Bits = new List<bool>();

            public void Write(bool bit)
            {
                Bits.Add(bit);
            }
        }

        private class Reader
            : IBitReader
        {
            private int _index = 0;
            private readonly List<bool> _bits;

            public Reader(List<bool> bits)
            {
                _bits = bits;
            }

            public bool Read()
            {
                if (EndOfStream)
                    throw new EndOfStreamException();

                return _bits[_index++];
            }

            private bool EndOfStream
            {
                get
                {
                    return _index == _bits.Count;
                }
            }
        }
    }
}
