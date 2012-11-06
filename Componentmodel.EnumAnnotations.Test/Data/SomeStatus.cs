using System.ComponentModel.DataAnnotations;

namespace ComponentModel.EnumAnnotations.Test.Data
{
    public enum SomeStatus
    {
        [Display(Name = "Fine Name", ShortName = "Fine ShortName", GroupName = "Fine GroupName", Description = "Fine Description", Order=1)]
        Fine = 1,

        [Display(Name = "Ok Name", Order = 2)]
        Ok = 2,

        [Display(Name = "Good Name", Order = 3)]
        Good = 3
    }
}
