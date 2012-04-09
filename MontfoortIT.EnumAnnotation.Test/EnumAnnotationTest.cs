using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using MontfoortIT.EnumAnnotation.ComponentModel;
using NUnit.Framework;
using Assert = NUnit.Framework.Assert;

namespace MontfoortIT.EnumAnnotation.Test
{
    [TestFixture]
    public class EnumAnnotationTest
    {
        [Test]
        public void EnumAnnotation_Construct()
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
        public void EnumAnnotation_GetDisplays()
        {
            IList<IDisplayAnnotation> displayAnnotations = EnumAnnotation<SomeStatus>.GetDisplays();
            var enumNames = Enum.GetNames(typeof (SomeStatus)).AsEnumerable();
            var enumValues = Enum.GetValues(typeof (SomeStatus)).Cast<int>();

            Assert.IsNotNull(displayAnnotations);
            Assert.IsTrue(enumNames.SequenceEqual(displayAnnotations.Select(x => x.ToString())));
            Assert.IsTrue(enumValues.SequenceEqual(displayAnnotations.Select(x => x.UnderlyingValue)));
        }

        [Test]
        public void EnumAnnotation_Equals()
        {
            IDisplayAnnotation annotation = new EnumAnnotation<SomeStatus>(SomeStatus.Good);
            IDisplayAnnotation annotation2 = new EnumAnnotation<SomeStatus>(SomeStatus.Good);
            Assert.AreEqual(annotation, annotation2);
        }

        [Test]
        public void EnumAnnotation_GetDisplays_By_Params_Ignores_Ordering()
        {
            List<IDisplayAnnotation> displayAnnotations = EnumAnnotation<SomeStatus>.GetDisplays(SomeStatus.Good, SomeStatus.Ok).ToList();
            List<IDisplayAnnotation> annotations = new List<IDisplayAnnotation> {new EnumAnnotation<SomeStatus>(SomeStatus.Good), new EnumAnnotation<SomeStatus>(SomeStatus.Ok)};

            for (int i = 0; i < displayAnnotations.Count; i++)
                Assert.IsTrue(displayAnnotations[i].Equals(annotations[i]));    
        }

        [Test]
        public void EnumAnnotation_GetDisplays_Filtered()
        {
            IList<IDisplayAnnotation> displayAnnotations = EnumAnnotation<SomeStatus>.GetDisplays(x => x.EnumValue != SomeStatus.Good);

            Assert.IsFalse(displayAnnotations.Any(x => (SomeStatus)x.Value == SomeStatus.Good));
            Assert.IsTrue(displayAnnotations.Any(x => (SomeStatus)x.Value == SomeStatus.Fine));
            Assert.IsTrue(displayAnnotations.Any(x => (SomeStatus)x.Value == SomeStatus.Ok));
        }

        [Test]
        public void EnumAnnotation_GetDisplays_AreOrdered()
        {
            IList<IDisplayAnnotation> displayAnnotations = EnumAnnotation<OrderedStatus>.GetDisplays();

            Assert.AreEqual(displayAnnotations[0].Value, OrderedStatus.Fine);
            Assert.AreEqual(displayAnnotations[1].Value, OrderedStatus.Good);
            Assert.AreEqual(displayAnnotations[2].Value, OrderedStatus.Ok);
        }
    }
}
