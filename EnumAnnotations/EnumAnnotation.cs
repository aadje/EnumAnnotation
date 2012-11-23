using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace EnumAnnotations
{
    /// <summary>
    /// Enum wrapper for more conviently accessing the Data Annotations Attributes (only the Display Attribute is supported) 
    /// </summary>
    public class EnumAnnotation
    {
        private readonly Enum _enumValue;
        private DisplayAttribute _display;

        /// <summary>
        /// Cached and Lazy loaded DisplayAttribute
        /// </summary>
        private DisplayAttribute Display
        {
            get
            {
                return _display ?? (_display = GetDisplayAttribute());
            }
        }

        /// <summary>
        /// DisplayAttribute.Name annotation. Returns Enum.ToString() if not specified.
        /// </summary>
        public string Name
        {
            get
            {
                return Display == null || string.IsNullOrEmpty(Display.Name) ? ToString() : Display.GetName();
            }
        }

        /// <summary>
        /// DisplayAttribute.ShortName annotation. Returns Enum.ToString() if not specified.
        /// </summary>
        public string ShortName
        {
            get { return Display == null || string.IsNullOrEmpty(Display.ShortName) ? null : Display.GetShortName(); }
        }

        /// <summary>
        /// DisplayAttribute.ShortName annotation. Returns an empty string if not specified.
        /// </summary>
        public string GroupName
        {
            get { return Display == null || string.IsNullOrEmpty(Display.GroupName) ? null : Display.GetGroupName(); }
        }

        /// <summary>
        /// DisplayAttribute.Description annotation. Returns an empty string if not specified.
        /// </summary>
        public string Description
        {
            get { return Display == null || string.IsNullOrEmpty(Display.Description) ? null : Display.GetDescription(); }
        }

        /// <summary>
        /// DisplayAttribute.Order annotation. Returns 0 if not specified.
        /// </summary>
        public int Order
        {
            get
            {
                int? order = null;
                if (Display != null)
                    order = Display.GetOrder();
                return Display == null || !order.HasValue ? default(int) : order.Value;
            }
        }

        /// <summary>
        /// Enum original value as object
        /// </summary>
        public object Value
        {
            get { return _enumValue; }
        }

        /// <summary>
        /// Enum Underlying numeric value casted to int
        /// </summary>
        public int UnderlyingValue
        {
            get
            {
                if (_enumValue == null)
                    return default(int);
                return (int)Value;
            }
        }

        /// <summary>
        /// Wrap an Enum in a EnumAnnotation for more conviently accessing the Annotations Attributes (only the Display Attribute is supported) 
        /// </summary>
        /// <param name="enumValue">An Enum value</param>
        public EnumAnnotation(Enum enumValue)
        {
            _enumValue = enumValue;
        }

        /// <summary>
        /// Enum value casted to string
        /// </summary>
        public override string ToString()
        {
            if (_enumValue == null)
                return string.Empty;
            return _enumValue.ToString();
        }

        public override bool Equals(object obj)
        {
            var compareObj = obj as EnumAnnotation;
            if (compareObj == null || _enumValue == null)
                return false;
            return Value.Equals(compareObj._enumValue);    
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        private DisplayAttribute GetDisplayAttribute()
        {
            if(_enumValue == null)
                return null;
            Type type = _enumValue.GetType();
            string name = Enum.GetName(type, _enumValue);
            if(name == null)
                return null;
            var field = type.GetField(name);
            return field.GetCustomAttributes(true).OfType<DisplayAttribute>().SingleOrDefault();
        }

        /// <summary>
        /// Get a Sorted list of all the Display Attribute Annotations for the values in Enum Type of T. Usefull for datasources in databound controls. 
        /// </summary>
        /// <param name="predicate">Optional filter expression parameter for removing values</param>
        /// <returns>A sorted list of EnumAnnotations for Enum Type of T </returns>
        public static List<EnumAnnotation> GetDisplays<T>(Func<EnumAnnotation, bool> predicate = null) where T : struct
        {
            return Enum.GetValues(typeof(T))
                .OfType<Enum>()
                .Select(v => new EnumAnnotation(v))
                .Where(predicate ?? (x => true))
                .OrderBy(a => a.Order)
                .ToList();
        }

        /// <summary>
        /// Get a Sorted list of Display Attribute Annotations of Enum Type of T for the supplied Enum values.
        /// </summary>
        /// <returns>A sorted list of EnumAnnotations for Enum Type of T </returns>
        /// <param name="args">Enum Type of T values</param>
        public static List<EnumAnnotation> GetDisplays(params Enum[] args)
        {
            return args.Select(v => new EnumAnnotation(v)).ToList();
        }

        /// <summary>
        /// Try find a single Enum value wrapped in an EnumAnnotation by its Display attribute fields with a filter expression for Enum Type of T
        /// </summary>
        /// <param name="predicate">filter expression</param>
        /// <returns>EnumAnnotations for Enum Type of T or Null</returns>
        public static EnumAnnotation FindDisplay<T>(Func<EnumAnnotation, bool> predicate)
        {
            return Enum.GetValues(typeof(T))
                .OfType<Enum>()
                .Select(v => new EnumAnnotation(v))
                .SingleOrDefault(predicate);
        }

        /// <summary>
        /// Get a list of Enums
        /// </summary>
        public static List<T> GetEnums<T>() where T : struct
        {
            return Enum.GetValues(typeof(T)).OfType<T>().ToList();
        }
    }

    /// <summary>
    /// Static Extension methods for getting DisplayAttribute values
    /// </summary>
    public static class EnumAnnotationExtension
    {
        /// <summary>
        /// Extension method which wraps a single enum value in an EnumAnnotation
        /// </summary>
        /// <param name="value">Enum value of Type of T</param>
        /// <returns>EnumAnnotation for Enum Type of T</returns>
        public static EnumAnnotation GetDisplay(this Enum value)
        {
            return new EnumAnnotation(value);
        }

        /// <summary>
        /// Extension method which returns the DisplayAttributes Name value
        /// </summary>
        /// <param name="value">Enum value of Type of T</param>
        /// <returns>EnumAnnotation.Name for the Enum value</returns>
        public static string GetName(this Enum value)
        {
            if (value == null)
                return null;
            return new EnumAnnotation(value).Name;
        }

        /// <summary>
        /// Extension method which returns the DisplayAttributes Name value or the the defaultName if the Enum value is null
        /// </summary>
        /// <param name="value">Nullable Enum value of Type of T</param>
        /// <param name="defaultName">The name to use when this nullable value has no value</param>
        /// <returns>EnumAnnotation.Name or the defaultName for the Enum value</returns>
        public static string GetName(this Enum value, string defaultName)
        {
            return value == null ? defaultName : GetName(value);
        }
    }
}