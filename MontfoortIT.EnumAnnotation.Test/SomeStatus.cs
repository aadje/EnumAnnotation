﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace MontfoortIT.EnumAnnotation.Test
{
    public enum OrderedStatus
    {
        [Display(Order=1)]
        Fine = 1,

        [Display(Order=3)]
        Ok = 2,

        [Display(Order=2)]
        Good = 3
    }
}
