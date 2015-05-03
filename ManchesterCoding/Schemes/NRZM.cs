

using System;

namespace ManchesterCoding.Schemes
{
    /// <summary>
    /// Non Return To Zero Mark
    /// One is represented by change in level, Zero is represented by no change in level
    /// </summary>
    public class NRZM
    {
        public class Encoder
            : IEncoder
        {
            private bool _previous = false;

            public void Encode(bool data, IBitWriter writer)
            {
                if (data)
                    _previous = !_previous;

                writer.Write(_previous);
            }
        }

        public class Decoder
            : IDecoder
        {
            private bool _previous = false;

            public bool Decode(IBitReader reader)
            {
                var previous = _previous;
                _previous = reader.Read();

                return _previous != previous;
            }


            public void Recover(IBitReader reader, IBitWriter writer)
            {
                throw new NotSupportedException();
            }
        }
    }
}
