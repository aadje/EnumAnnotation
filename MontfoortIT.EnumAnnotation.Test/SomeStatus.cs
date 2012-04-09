using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace MontfoortIT.EnumAnnotation.Test
{
    public enum SomeStatus
    {
        [Display(Name="Fine Name")]
        Fine = 1,

        [Display(Name = "Ok Name")]
        Ok = 2,

        [Display(Name = "Good Name")]
        Good = 3
    }
}
