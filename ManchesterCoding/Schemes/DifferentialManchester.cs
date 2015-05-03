using System.Collections.Generic;

namespace ManchesterCoding.Schemes
{
    /// <summary>
    /// Differential Manchester Encoding
    /// Two bits per bit of data. Clock signal always ivnerts previous value and data then inverts if it was a zero
    /// </summary>
    public class DifferentialManchester
    {
        public class Encoder
            : IEncoder
        {
            private bool _level = true;

            public void Encode(bool data, IBitWriter writer)
            {
                //Encode clock signal (clock always inverts previous value)
                _level = !_level;
                writer.Write(_level);

                //Encode data (0 means transition)
                if (!data)
                    _level = !_level;
                writer.Write(_level);
            }
        }

        public class Decoder
            : IDecoder
        {
            public bool Decode(IBitReader reader)
            {
                bool clock = reader.Read();
                bool data = reader.Read();

                return clock == data;
            }

            public void Recover(IBitReader reader, IBitWriter writer)
            {
                //There is a guaranteed transition every clock cycle
                //Keep reading the stream until you do *not* encounter a transition, which will give you the necessary information to infer the clock

                //Keep filling this buffer until we find 2 consecutive bits which are the same
                List<bool> buffer = new List<bool> {
                    reader.Read(),
                    reader.Read()
                };

                while (true)
                {
                    //...Continue filling the buffer
                    buffer.Add(reader.Read());

                    //...are they the same?
                    if (buffer[buffer.Count - 1] == buffer[buffer.Count - 2])
                    {
                        //We have found out data, work back through the buffer decoding it
                        Stack<bool> decoded = new Stack<bool>();

                        //Decode the buffer (backwards)
                        for (int i = buffer.Count - 1; i > 0; i -= 2)
                        {
                            var x = buffer[i];
                            var y = buffer[i - 1];
                            decoded.Push(x == y);
                        }

                        //Empty the stack to the writer (which does things in the right order)
                        while (decoded.Count > 0)
                            writer.Write(decoded.Pop());

                        return;
                    }
                }
            }
        }
    }
}
