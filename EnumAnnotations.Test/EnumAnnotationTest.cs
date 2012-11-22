using System;
using System.Linq;
using System.Collections.Generic;
using EnumAnnotations.Test.Data;
using NUnit.Framework;
using Assert = NUnit.Framework.Assert;

namespace EnumAnnotations.Test
{
    [TestFixture]
    public class EnumAnnotationTest
    {
        [Test]
        public void EnumAnnotationConstruct()
        {
            EnumAnnotation<SomeStatus> enumAnnotation = new EnumAnnotation<SomeStatus>(SomeStatus.Fine);

            Assert.IsNotNull(enumAnnotation);
            Assert.AreEqual(SomeStatus.Fine, enumAnnotation.EnumValue);
            Assert.AreEqual("Fine Name", enumAnnotation.Name);
            Assert.AreEqual("Fine ShortName", enumAnnotation.ShortName);
            Assert.AreEqual("Fine GroupName", enumAnnotation.GroupName);
            Assert.AreEqual("Fine Description", enumAnnotation.Description);
            Assert.AreEqual(1, enumAnnotation.Order);
        }

        [Test]
        public void EnumAnnotationEquals()
        {
            IDisplayAnnotation annotation = new EnumAnnotation<SomeStatus>(SomeStatus.Good);
            IDisplayAnnotation annotation2 = new EnumAnnotation<SomeStatus>(SomeStatus.Good);
            Assert.AreEqual(annotation, annotation2);
        }

        [Test]
        public void GetDisplays()
        {
            List<IDisplayAnnotation> displayAnnotations = EnumAnnotation.GetDisplays<SomeStatus>();
            var enumNames = Enum.GetNames(typeof (SomeStatus)).AsEnumerable();
            var enumValues = Enum.GetValues(typeof (SomeStatus)).Cast<int>();
            Assert.IsNotNull(displayAnnotations);
            Assert.IsTrue(enumNames.SequenceEqual(displayAnnotations.Select(x => x.ToString())));
            Assert.IsTrue(enumValues.SequenceEqual(displayAnnotations.Select(x => x.UnderlyingValue)));
        }

        [Test]
        public void GetDisplaysByParamsIgnoresOrdering()
        {
            List<IDisplayAnnotation> displayAnnotations = EnumAnnotation.GetDisplays(SomeStatus.Good, SomeStatus.Ok);
            List<IDisplayAnnotation> annotations = new List<IDisplayAnnotation> {new EnumAnnotation<SomeStatus>(SomeStatus.Good), new EnumAnnotation<SomeStatus>(SomeStatus.Ok)};

            for (int i = 0; i < displayAnnotations.Count; i++)
                Assert.IsTrue(displayAnnotations[i].Equals(annotations[i]));    
        }

        [Test]
        public void GetDisplaysFiltered()
        {
            List<IDisplayAnnotation> displayAnnotations = EnumAnnotation.GetDisplays<SomeStatus>(x => x.EnumValue != SomeStatus.Good);

            Assert.IsFalse(displayAnnotations.Any(x => (SomeStatus)x.Value == SomeStatus.Good));
            Assert.IsTrue(displayAnnotations.Any(x => (SomeStatus)x.Value == SomeStatus.Fine));
            Assert.IsTrue(displayAnnotations.Any(x => (SomeStatus)x.Value == SomeStatus.Ok));
        }

        [Test]
        public void GetDisplaysAreOrdered()
        {
            List<IDisplayAnnotation> displayAnnotations = EnumAnnotation.GetDisplays<OrderedStatus>();

            Assert.AreEqual(OrderedStatus.Fine, displayAnnotations[0].Value);
            Assert.AreEqual(OrderedStatus.Good, displayAnnotations[1].Value);
            Assert.AreEqual(OrderedStatus.Ok, displayAnnotations[2].Value);
        }

        [Test]
        public void GetDisplaysWithoutAnnotations()
        {
            List<IDisplayAnnotation> displayAnnotations = EnumAnnotation.GetDisplays<NotAnnotatedStatus>();

            Assert.IsNotNull(displayAnnotations);

            Assert.AreEqual(NotAnnotatedStatus.Fine, displayAnnotations[0].Value);
            Assert.AreEqual(NotAnnotatedStatus.Ok, displayAnnotations[1].Value);
            Assert.AreEqual(NotAnnotatedStatus.Good, displayAnnotations[2].Value);
        }
        
        [Test]
        public void GetDisplaysWithoutAnnotationsReturnsDefaults()
        {
            List<IDisplayAnnotation> displayAnnotations = EnumAnnotation.GetDisplays<NotAnnotatedStatus>();

            IDisplayAnnotation displayAnnotation = displayAnnotations[0];

            Assert.AreEqual(NotAnnotatedStatus.Fine, displayAnnotation.Value);
            Assert.AreEqual(1, displayAnnotation.UnderlyingValue);

            Assert.AreEqual("Fine", displayAnnotation.Name);
            Assert.AreEqual("Fine", displayAnnotation.ShortName);
            Assert.AreEqual("Fine", displayAnnotation.ToString());

            Assert.AreEqual(string.Empty, displayAnnotation.Description);
            Assert.AreEqual(string.Empty, displayAnnotation.GroupName);
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
            IDisplayAnnotation fine = LocalizedStatus.Fine.GetDisplay();
            Assert.AreEqual(LocalizedStatus.Fine, fine.Value);
            Assert.AreEqual("LocalizedStatus Fine Name", fine.Name);
            Assert.AreEqual("LocalizedStatus Fine ShortName", fine.ShortName);
            Assert.AreEqual("LocalizedStatus Fine Description", fine.Description);
            Assert.AreEqual("LocalizedStatus Fine GroupName", fine.GroupName);
        }

        [Test]
        public void ExtensionGetDisplay()
        {
            IDisplayAnnotation fine = SomeStatus.Fine.GetDisplay();
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
            Assert.AreEqual("not found", nullStatus.GetName("not found"));
            Assert.IsNull(nullStatus.GetName());
            Assert.AreEqual("Good", notAnnotatedStatus.GetName());
        }

        [Test, ExpectedException(typeof(NotSupportedException), ExpectedMessage="System.Int32")]
        public void NonEnumTypeThrowNotsupportedException()
        {
            string name = new EnumAnnotation<int>(3).Name;
        }
    }
}
