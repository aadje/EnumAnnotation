using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace MontfoortIT.EnumAnnotation.Test
{
    public enum SomeStatus
    {
        [Display(Name = "Fine Name", ShortName = "Fine ShortName", GroupName = "Fine GroupName", Description = "Fine Description", Order=1)]
        Fine = 1,

        [Display(Name = "Ok Name")]
        Ok = 2,

        [Display(Name = "Good Name")]
        Good = 3
    }
}
