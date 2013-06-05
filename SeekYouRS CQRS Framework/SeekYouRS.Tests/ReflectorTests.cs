using System;
using NUnit.Framework;

namespace SeekYouRS.Tests
{
    [TestFixture]
    public class ReflectorTests
    {
        [Test]
        public void ReadValueOrDefault_liest_Guid_Property_Id_aus_einer_Klasse()
        {
            var id = Guid.NewGuid();
            var obj = new KlasseMitEigenschaft(id);

            var result = Reflector.ReadValueOrDefault(obj, "Id", Guid.Empty);
            Assert.AreEqual(id, result, "liefert nicht das richtige Ergegnis");
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
        public void ReadValueOrDefault_liefert_den_DefaultWert_falls_die_Eigenschaft_im_Objekt_nicht_vorhanden_ist()
        {
            var obj = new KlasseMitEigenschaft(Guid.NewGuid());

            var result = Reflector.ReadValueOrDefault(obj, "NichtDa", "---");
            Assert.AreEqual("---", result, "liefert nicht das richtige Ergegnis");
        }
    }

    class KlasseMitEigenschaft
    {
        public Guid Id { get; set; }

        public KlasseMitEigenschaft(Guid id)
        {
            Id = id;
        }
    }

    class KlasseMitFeld
    {
        public Guid Id;

        public KlasseMitFeld(Guid id)
        {
            Id = id;
        }
    }

    class GenerischeKlasseMitEigenschaft<tEigenschaft>
    {
        public tEigenschaft Value { get; set; }

        public GenerischeKlasseMitEigenschaft(tEigenschaft value)
        {
            Value = value;
        }
    }
}
