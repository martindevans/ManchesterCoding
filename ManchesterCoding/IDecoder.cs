
namespace ManchesterCoding
{
    public interface IDecoder
    {
        /// <summary>
        /// Decode the next bit of data from the stream
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        bool Decode(IBitReader reader);

        /// <summary>
        /// Read data from the stream until confident a bit of data has been read
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="writer"></param>
        /// <returns></returns>
        void Recover(IBitReader reader, IBitWriter writer);
    }
}
