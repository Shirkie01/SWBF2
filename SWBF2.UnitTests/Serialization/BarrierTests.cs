using Microsoft.VisualStudio.TestTools.UnitTesting;
using SWBF2.Serialization;
using System.Collections.Generic;
using System.IO;

namespace SWBF2.UnitTests.Serialization
{
    [TestClass]
    [DeploymentItem("Data")]
    public class BarrierTests
    {
        private ITypedFormatter<IList<Barrier>> formatter = new BarrierFormatter();

        [TestMethod]
        public void BarrierCorTest()
        {
            BarrierDeserializeSerializeTest("assets/worlds/COR/world1/cor1.BAR");
        }

        [TestMethod]
        public void BarrierDagTest()
        {
            BarrierDeserializeSerializeTest("assets/worlds/DAG/world1/dag1.BAR");
        }

        [TestMethod]
        public void BarrierDeaTest()
        {
            BarrierDeserializeSerializeTest("assets/worlds/DEA/world1/dea1.BAR");
        }

        [TestMethod]
        public void BarrierEndorTest()
        {
            BarrierDeserializeSerializeTest("assets/worlds/END/world1/end1.BAR");
        }

        [TestMethod]
        public void BarrierFelTest()
        {
            BarrierDeserializeSerializeTest("assets/worlds/FEL/world1/fel1.BAR");
        }

        [TestMethod]
        public void BarrierGeoTest()
        {
            BarrierDeserializeSerializeTest("assets/worlds/GEO/world1/geo1.BAR");
        }

        [TestMethod]
        public void BarrierHothTest()
        {
            BarrierDeserializeSerializeTest("assets/worlds/HOT/world1/hoth.BAR");
        }

        [TestMethod]
        public void BarrierKamTest()
        {
            BarrierDeserializeSerializeTest("assets/worlds/KAM/world1/kamino1.BAR");
        }

        [TestMethod]
        public void BarrierKasTest()
        {
            BarrierDeserializeSerializeTest("assets/worlds/KAS/world2/kas2.BAR");
        }

        [TestMethod]
        public void BarrierMusTest()
        {
            BarrierDeserializeSerializeTest("assets/worlds/MUS/world1/mus1.BAR");
        }

        [TestMethod]
        public void BarrierMygTest()
        {
            BarrierDeserializeSerializeTest("assets/worlds/MYG/world1/myg1.BAR");
        }

        [TestMethod]
        public void BarrierNabTest()
        {
            BarrierDeserializeSerializeTest("assets/worlds/NAB/world2/naboo2.BAR");
        }

        [TestMethod]
        public void BarrierPolTest()
        {
            BarrierDeserializeSerializeTest("assets/worlds/POL/world1/pol1.BAR");
        }

        [TestMethod]
        public void BarrierTanTest()
        {
            BarrierDeserializeSerializeTest("assets/worlds/TAN/world1/tan1.BAR");
        }

        [TestMethod]
        public void BarrierTat2Test()
        {
            BarrierDeserializeSerializeTest("assets/worlds/TAT/world2/tat2.BAR");
        }

        [TestMethod]
        public void BarrierTat3Test()
        {
            BarrierDeserializeSerializeTest("assets/worlds/TAT/world3/tat3.BAR");
        }

        [TestMethod]
        public void BarrierUtaTest()
        {
            BarrierDeserializeSerializeTest("assets/worlds/UTA/world/uta1.BAR");
        }

        [TestMethod]
        public void BarrierYavin1Test()
        {
            BarrierDeserializeSerializeTest("assets/worlds/YAV/world1/yavin1.BAR");
        }

        [TestMethod]
        public void BarrierYavin2Test()
        {
            BarrierDeserializeSerializeTest("assets/worlds/YAV/world2/yav2.BAR");
        }

        private void BarrierDeserializeSerializeTest(string path)
        {
            IList<Barrier> original = null;
            using (var fs = new FileStream(path, FileMode.Open))
            {
                original = formatter.Deserialize(fs);
            }

            using (var fs = new FileStream(path + ".new", FileMode.CreateNew))
            {
                formatter.Serialize(fs, original);
            }

            byte[] expected = File.ReadAllBytes(path);
            byte[] actual = File.ReadAllBytes(path + ".new");

            var max = expected.Length > actual.Length ? actual.Length : expected.Length;

            for (int i = 0; i < max; i++)
            {
                Assert.AreEqual(expected[i], actual[i]);
            }

            Assert.AreEqual(expected.Length, actual.Length);
        }
    }
}