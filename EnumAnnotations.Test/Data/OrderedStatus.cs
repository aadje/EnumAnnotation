using System.ComponentModel.DataAnnotations;

namespace EnumAnnotations.Test.Data
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
