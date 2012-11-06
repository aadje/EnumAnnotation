using System.ComponentModel.DataAnnotations;

namespace ComponentModel.EnumAnnotations.Test.Data
{
    public enum LocalizedStatus
    {
        [Display(ResourceType = typeof(Resources.Enums), Name = "LocalizedStatus_Fine_Name", ShortName = "LocalizedStatus_Fine_ShortName", GroupName = "LocalizedStatus_Fine_GroupName", Description = "LocalizedStatus_Fine_Description")]
        Fine = 1,

        [Display(ResourceType = typeof(Resources.Enums), Name = "LocalizedStatus_Ok_Name")]
        Ok = 2,

        [Display(ResourceType = typeof(Resources.Enums), Name = "LocalizedStatus_Good_Name")]
        Good = 3
    }
}
