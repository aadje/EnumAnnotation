using System.ComponentModel.DataAnnotations;

namespace MontfoortIT.EnumAnnotation.Test.Data
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
