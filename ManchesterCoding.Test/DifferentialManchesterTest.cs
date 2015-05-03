using ManchesterCoding.Schemes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ManchesterCoding.Test
{
    [TestClass]
    public class DifferentialManchesterTest
    {
        [TestMethod]
        public void EncodeDecode()
        {
            Harness.TestEncodeDecode<DifferentialManchester.Encoder, DifferentialManchester.Decoder>();
        }

        [TestMethod]
        public void Recover()
        {
            Harness.TestRecover<DifferentialManchester.Encoder, DifferentialManchester.Decoder>();
        }
    }
}
