
namespace ManchesterCoding
{
    public interface IEncoder
    {
        void Encode(bool data, IBitWriter writer);
    }
}
