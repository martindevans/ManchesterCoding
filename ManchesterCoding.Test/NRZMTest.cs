using ManchesterCoding.Schemes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ManchesterCoding.Test
{
    [TestClass]
    public class NRZMTest
    {
        [TestMethod]
        public void EncodeDecode()
        {
            Harness.TestEncodeDecode<NRZM.Encoder, NRZM.Decoder>();
        }
    }
}
