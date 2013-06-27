using System;
using NUnit.Framework;
using SeekYouRS.Tests.TestObjects;
using SeekYouRS.Utilities;

namespace SeekYouRS.Tests
{
    [TestFixture]
    public class ReflectorTests
    {
        [SetUp]
        public void SetupTest()
        {
            Reflector.ClearCache();
        }

        [Test]
        public void ReadValueOrDefault_liest_Guid_Property_Id_aus_einer_Klasse()
        {
            var id = Guid.NewGuid();
            var obj = new KlasseMitEigenschaft(id);

            var result = Reflector.ReadValueOrDefault(obj, "Id", Guid.Empty);
            Assert.AreEqual(id, result, "liefert nicht das richtige Ergegnis");
        }

        [Test]
        public void ReadValueOrDefault_funktioniert_mehrmals_auf_unterschiedlichen_Objekten()
        {
            // Note: teste den Cache
            var obj1 = new KlasseMitEigenschaft(Guid.NewGuid());
            var obj2 = new KlasseMitEigenschaft(Guid.NewGuid());

            var result1 = Reflector.ReadValueOrDefault(obj1, "Id", Guid.Empty);
            Assert.AreEqual(obj1.Id, result1, "liefert das erste Mal nicht das richtige Ergegnis");

            var result2 = Reflector.ReadValueOrDefault(obj2, "Id", Guid.Empty);
            Assert.AreEqual(obj2.Id, result2, "liefert das zweite Mal nicht das richtige Ergegnis");
        }

        [Test]
        public void ReadValueOrDefault_ist_unabhängig_von_GroßKleinSchreibung()
        {
            var id = Guid.NewGuid();
            var obj = new KlasseMitEigenschaft(id);

            var result = Reflector.ReadValueOrDefault(obj, "iD", Guid.Empty);
            Assert.AreEqual(id, result, "liefert nicht das richtige Ergegnis");
        }

        [Test]
        public void ReadValueOrDefault_liest_Guid_Field_Id_aus_einer_Klasse()
        {
            var id = Guid.NewGuid();
            var obj = new KlasseMitFeld(id);

            var result = Reflector.ReadValueOrDefault(obj, "Id", Guid.Empty);
            Assert.AreEqual(id, result, "liefert nicht das richtige Ergegnis");
        }

        [Test]
        public void ReadValueOrDefault_liest_generisches_Guid_Field_Value_aus_einer_generischen_Klasse()
        {
            var id = Guid.NewGuid();
            var obj = new GenerischeKlasseMitEigenschaft<Guid>(id);

            var result = Reflector.ReadValueOrDefault(obj, "Value", Guid.Empty);
            Assert.AreEqual(id, result, "liefert nicht das richtige Ergegnis");
        }

        [Test]
        public void ReadValueOrDefault_liest_aus_generischen_Typen_mit_gleichen_Eigenschaftsnamen_aber_unterschiedlichen_Typen()
        {
            var guidObj = new GenerischeKlasseMitEigenschaft<Guid>(Guid.NewGuid());
            var stringObj = new GenerischeKlasseMitEigenschaft<string>("Hallo");

            var guidResult = Reflector.ReadValueOrDefault(guidObj, "Value", Guid.Empty);
            Assert.AreEqual(guidObj.Value, guidResult, "liefert nicht das richtige Ergegnis für den Guid");

            var stringResult = Reflector.ReadValueOrDefault(stringObj, "Value", string.Empty);
            Assert.AreEqual(stringObj.Value, stringResult, "liefert nicht das richtige Ergegnis für den String");
        }

        [Test]
        public void ReadValueOrDefault_liefert_den_DefaultWert_falls_die_Eigenschaft_im_Objekt_nicht_vorhanden_ist()
        {
            var obj = new KlasseMitEigenschaft(Guid.NewGuid());

            var result = Reflector.ReadValueOrDefault(obj, "NichtDa", "---");
            Assert.AreEqual("---", result, "liefert nicht das richtige Ergegnis");
        }

        [Test]
        public void ReadValueOrDefault_liefert_den_DefaultWert_falls_die_Eigenschaft_im_Objekt_den_falschen_Typ_hat()
        {
            var obj = new KlasseMitEigenschaft(Guid.NewGuid());

            var result = Reflector.ReadValueOrDefault(obj, "Id", "---");
            Assert.AreEqual("---", result, "liefert nicht das richtige Ergegnis");
        }
    }
}