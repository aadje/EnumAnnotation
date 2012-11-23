using System;
using System.Linq;
using System.Collections.Generic;
using EnumAnnotations.Test.Data;
using NUnit.Framework;
using Assert = NUnit.Framework.Assert;
using EnumAnnotations;

namespace EnumAnnotations.Test
{
    [TestFixture]
    public class EnumAnnotationTest
    {
        [Test]
        public void EnumAnnotationConstruct()
        {
            EnumAnnotation enumAnnotation = new EnumAnnotation(SomeStatus.Fine);

            Assert.IsNotNull(enumAnnotation);
            Assert.AreEqual(SomeStatus.Fine, enumAnnotation.Value);
            Assert.AreEqual("Fine Name", enumAnnotation.Name);
            Assert.AreEqual("Fine ShortName", enumAnnotation.ShortName);
            Assert.AreEqual("Fine GroupName", enumAnnotation.GroupName);
            Assert.AreEqual("Fine Description", enumAnnotation.Description);
            Assert.AreEqual(1, enumAnnotation.Order);
        }

        [Test]
        public void EnumAnnotationEquals()
        {
            EnumAnnotation annotation = new EnumAnnotation(SomeStatus.Good);
            EnumAnnotation annotation2 = new EnumAnnotation(SomeStatus.Good);
            Assert.AreEqual(annotation, annotation2);
        }

        [Test]
        public void GetDisplays()
        {
            List<EnumAnnotation> displayAnnotations = EnumAnnotation.GetDisplays<SomeStatus>();
            var enumNames = Enum.GetNames(typeof (SomeStatus)).AsEnumerable();
            var enumValues = Enum.GetValues(typeof (SomeStatus)).Cast<int>();
            Assert.IsNotNull(displayAnnotations);
            Assert.IsTrue(enumNames.SequenceEqual(displayAnnotations.Select(x => x.ToString())));
            Assert.IsTrue(enumValues.SequenceEqual(displayAnnotations.Select(x => x.UnderlyingValue)));
        }

        [Test, ExpectedException(typeof(ArgumentException))]
        public void GetDisplaysWitNonEnum()
        {
            List<EnumAnnotation> enumAnnotations = EnumAnnotation.GetDisplays<int>();
            Assert.IsTrue(enumAnnotations.Any());
        }

        [Test]
        public void GetDisplaysByParamsIgnoresOrdering()
        {
            List<EnumAnnotation> displayAnnotations = EnumAnnotation.GetDisplays(SomeStatus.Good, SomeStatus.Ok);
            List<EnumAnnotation> annotations = new List<EnumAnnotation> { new EnumAnnotation(SomeStatus.Good), new EnumAnnotation(SomeStatus.Ok) };

            for (int i = 0; i < displayAnnotations.Count; i++)
                Assert.IsTrue(displayAnnotations[i].Equals(annotations[i]));    
        }

        [Test]
        public void GetDisplaysFiltered()
        {
            List<EnumAnnotation> displayAnnotations = EnumAnnotation.GetDisplays<SomeStatus>(x => (SomeStatus)x.Value != SomeStatus.Good);

            Assert.IsFalse(displayAnnotations.Any(x => (SomeStatus)x.Value == SomeStatus.Good));
            Assert.IsTrue(displayAnnotations.Any(x => (SomeStatus)x.Value == SomeStatus.Fine));
            Assert.IsTrue(displayAnnotations.Any(x => (SomeStatus)x.Value == SomeStatus.Ok));
        }

        [Test]
        public void GetDisplaysAreOrdered()
        {
            List<EnumAnnotation> displayAnnotations = EnumAnnotation.GetDisplays<OrderedStatus>();

            Assert.AreEqual(OrderedStatus.Fine, displayAnnotations[0].Value);
            Assert.AreEqual(OrderedStatus.Good, displayAnnotations[1].Value);
            Assert.AreEqual(OrderedStatus.Ok, displayAnnotations[2].Value);
        }

        [Test]
        public void GetDisplaysWithoutAnnotations()
        {
            List<EnumAnnotation> displayAnnotations = EnumAnnotation.GetDisplays<NotAnnotatedStatus>();

            Assert.IsNotNull(displayAnnotations);

            Assert.AreEqual(NotAnnotatedStatus.Fine, displayAnnotations[0].Value);
            Assert.AreEqual(NotAnnotatedStatus.Ok, displayAnnotations[1].Value);
            Assert.AreEqual(NotAnnotatedStatus.Good, displayAnnotations[2].Value);
        }
        
        [Test]
        public void GetDisplaysWithoutAnnotationsReturnsDefaults()
        {
            List<EnumAnnotation> displayAnnotations = EnumAnnotation.GetDisplays<NotAnnotatedStatus>();

            EnumAnnotation displayAnnotation = displayAnnotations[0];

            Assert.AreEqual(NotAnnotatedStatus.Fine, displayAnnotation.Value);
            Assert.AreEqual(1, (int)displayAnnotation.Value);

            Assert.AreEqual("Fine", displayAnnotation.Name);
            Assert.AreEqual("Fine", displayAnnotation.ToString());

            Assert.IsNull(displayAnnotation.Description);
            Assert.IsNull(displayAnnotation.GroupName);
        }

        [Test]
        public void GetEnumsReturnsEnums()
        {
            IEnumerable<NotAnnotatedStatus> statusses = EnumAnnotation.GetEnums<NotAnnotatedStatus>();

            Assert.AreEqual(NotAnnotatedStatus.Fine, statusses.First());
            Assert.AreEqual(3, statusses.Count());
        }

        [Test]
        public void GetDisplayReturnsLocalizedValues()
        {
            EnumAnnotation fine = LocalizedStatus.Fine.GetDisplay();
            Assert.AreEqual(LocalizedStatus.Fine, fine.Value);
            Assert.AreEqual("LocalizedStatus Fine Name", fine.Name);
            Assert.AreEqual("LocalizedStatus Fine ShortName", fine.ShortName);
            Assert.AreEqual("LocalizedStatus Fine Description", fine.Description);
            Assert.AreEqual("LocalizedStatus Fine GroupName", fine.GroupName);
        }

        [Test]
        public void ExtensionGetDisplay()
        {
            EnumAnnotation fine = SomeStatus.Fine.GetDisplay();
            Assert.AreEqual(SomeStatus.Fine, fine.Value);
            Assert.AreEqual("Fine Name", fine.Name);
        }

        [Test]
        public void ExtensionGetName()
        {
            Assert.AreEqual("Fine Name", SomeStatus.Fine.GetName());
            Assert.AreEqual("Fine", NotAnnotatedStatus.Fine.GetName());
        }

        [Test]
        public void ExtensionGetNameNullable()
        {
            SomeStatus? status = SomeStatus.Fine;
            SomeStatus? nullStatus = null;
            NotAnnotatedStatus? notAnnotatedStatus = NotAnnotatedStatus.Good;
            Assert.AreEqual("Fine Name", status.GetName());
            Assert.IsNull(nullStatus.GetName());
            Assert.AreEqual("not found", nullStatus.GetName("not found"));
            Assert.AreEqual(string.Empty, nullStatus.GetDisplay().Name);
            Assert.AreEqual("Good", notAnnotatedStatus.GetName());
        }

        [Test]
        public void EnumAnnotationNull()
        {
            EnumAnnotation nullAnnotation = new EnumAnnotation(null);
            Assert.AreEqual(string.Empty, nullAnnotation.Name);
            Assert.IsNull(nullAnnotation.ShortName);
            Assert.IsNull(nullAnnotation.GroupName);
            Assert.IsNull(nullAnnotation.Description);
            Assert.IsNull(nullAnnotation.Value);
            Assert.AreEqual(0, nullAnnotation.Order);
            Assert.AreEqual(0, nullAnnotation.UnderlyingValue);
            Assert.AreEqual(string.Empty, nullAnnotation.ToString());
        }

        [Test]
        public void GetDisplaysForDifferentTypes()
        {
            var annotations = EnumAnnotation.GetDisplays(SomeStatus.Good, OrderedStatus.Fine);
            Assert.AreEqual(annotations[0].Value, SomeStatus.Good);
            Assert.AreEqual(annotations[1].Value, OrderedStatus.Fine);
        }

        [Test]
        public void FlaggedEnum()
        {
            FlaggedStatus flaggedStatus = FlaggedStatus.Fine | FlaggedStatus.Good;
            var flaggedAnnotation = new EnumAnnotation(flaggedStatus);

            Assert.AreEqual("Fine, Good", flaggedAnnotation.Name);
            Assert.AreEqual("Fine, Good", flaggedAnnotation.ToString());
            Assert.IsTrue(flaggedStatus.HasFlag(FlaggedStatus.Good));
            Assert.IsTrue(flaggedStatus.HasFlag(FlaggedStatus.Good | FlaggedStatus.Fine));
            Assert.IsFalse(flaggedStatus.HasFlag(FlaggedStatus.Ok));
            Assert.IsFalse(flaggedStatus.HasFlag(FlaggedStatus.Good | FlaggedStatus.Ok));
        }
    }
}
