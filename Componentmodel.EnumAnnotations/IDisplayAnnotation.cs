namespace ComponentModel.EnumAnnotations
{
    /// <summary>
    /// Wrapper around the System.ComponentModel.DataAnnotations.DisplayAttribute and Enum Properties
    /// </summary>
    public interface IDisplayAnnotation
    {
        /// <summary>
        /// DisplayAttribute.Name annotation
        /// </summary>
        string Name { get; }

        /// <summary>
        /// DisplayAttribute.ShortName annotation
        /// </summary>
        string ShortName { get; }

        /// <summary>
        /// DisplayAttribute.GroupName annotation
        /// </summary>
        string GroupName { get; }

        /// <summary>
        /// DisplayAttribute.Description annotation
        /// </summary>
        string Description { get; }

        /// <summary>
        /// DisplayAttribute.Order annotation
        /// </summary>
        int Order { get; }

        /// <summary>
        /// Enum original value
        /// </summary>
        object Value { get; }

        /// <summary>
        /// Enum Underlying numeric value casted to int
        /// </summary>
        int UnderlyingValue { get; }
    }
}
