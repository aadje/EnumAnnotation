using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MontfoortIT.EnumAnnotation.ComponentModel;

namespace MontfoortIT.EnumAnnotation.Test
{
    [TestClass]
    public class EnumAnnotationTest
    {
        [TestMethod]
        public void ConstructEnumAnnotation()
        {
            EnumAnnotation<SomeStatus> enumAnnotation = new EnumAnnotation<SomeStatus>(SomeStatus.Good);

            Assert.IsNotNull(enumAnnotation);
            Assert.AreEqual(SomeStatus.Good, enumAnnotation.EnumValue);
        }
    }
}
