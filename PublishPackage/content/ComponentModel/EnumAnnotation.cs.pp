using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace $rootnamespace$.ComponentModel
{
    /// <summary>
    /// Enum wrapper for more conviently accessing the Data Annotations Attributes (only the Display Attribute is supported) 
    /// </summary>
    public class EnumAnnotation<T> : IDisplayAnnotation where T : struct
    {
        private readonly T _enumValue;

        private DisplayAttribute _display;
        private DisplayAttribute Display
        {
            get
            {
                return _display ?? (_display = GetDisplayAttribute());
            }
        }

        /// <summary>
        /// DisplayAttribute.Name annotation
        /// </summary>
        public string Name
        {
            get { return Display == null || string.IsNullOrEmpty(Display.Name) ? ToString() : Display.Name; }
        }

        /// <summary>
        /// DisplayAttribute.ShortName annotation
        /// </summary>
        public string ShortName
        {
            get { return Display == null || string.IsNullOrEmpty(Display.ShortName) ? ToString() : Display.ShortName; }
        }

        /// <summary>
        /// DisplayAttribute.ShortName annotation
        /// </summary>
        public string GroupName
        {
            get { return Display == null ? string.Empty : Display.GroupName; }
        }

        /// <summary>
        /// DisplayAttribute.Description annotation
        /// </summary>
        public string Description
        {
            get { return Display == null || string.IsNullOrEmpty(Display.Description) ? ToString() : Display.Description; }
        }

        /// <summary>
        /// DisplayAttribute.Order annotation
        /// </summary>
        public int Order
        {
            get
            {
                int? order = Display.GetOrder();
                return Display == null || !order.HasValue ? int.MaxValue : order.Value;
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
        /// Enum original value 
        /// </summary>
        public T EnumValue
        {
            get { return _enumValue; }
        }

        /// <summary>
        /// Enum Underlying numeric value casted to int
        /// </summary>
        public int UnderlyingValue
        {
            get { return (int)Value; }
        }

        /// <summary>
        /// Wrap an Enum in a EnumAnnotation for more conviently accessing the Annotations Attributes (only the Display Attribute is supported) 
        /// </summary>
        /// <param name="enumvalue">An Enum value of Type T</param>
        public EnumAnnotation(T enumvalue)
        {
            _enumValue = enumvalue;
        }

        private DisplayAttribute GetDisplayAttribute()
        {
            Type type = typeof(T);
            if (!type.IsEnum)
                throw new NotSupportedException();

            string name = Enum.GetName(type, _enumValue);
            
            var field = type.GetField(name);

            return field.GetCustomAttributes(true).OfType<DisplayAttribute>().SingleOrDefault();
        }

        /// <summary>
        /// Enum value casted to string
        /// </summary>
        public override string ToString()
        {
            return Value.ToString();
        }

        public override bool Equals(object obj)
        {
            return obj != null && Value.Equals(((EnumAnnotation<T>)obj).Value);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        /// <summary>
        /// Get a Sorted list of all the Display Attribute Annotations for the values in Enum Type of T. Usefull for datasources in databound controls. 
        /// </summary>
        /// <param name="predicate">Optional filter expression parameter for removing values</param>
        /// <returns>A sorted list of EnumAnnotations for Enum Type of T </returns>
        public static IList<IDisplayAnnotation> GetDisplays(Func<EnumAnnotation<T>, bool> predicate = null)
        {
            return Enum.GetValues(typeof(T))
                .Cast<T>()
                .Select(v => new EnumAnnotation<T>(v))
                .Where(predicate ?? (x => true))
                .Cast<IDisplayAnnotation>()
                .OrderBy(a => a.Order)
                .ToList();
        }

        /// <summary>
        /// Get a Sorted list of Display Attribute Annotations of Enum Type of T for the supplied Enum values.
        /// </summary>
        /// <returns>A sorted list of EnumAnnotations for Enum Type of T </returns>
        /// <param name="args">Enum Type of T values</param>
        public static IList<IDisplayAnnotation> GetDisplays(params T[] args)
        {
            return args
                .Select(v => new EnumAnnotation<T>(v))
                .Cast<IDisplayAnnotation>()
                .ToList();
        }

        /// <summary>
        /// Try find a single Enum value wrapped in an EnumAnnotation by its Display attribute fields with a filter expression for Enum Type of T
        /// </summary>
        /// <param name="predicate">filter expression</param>
        /// <returns>EnumAnnotations for Enum Type of T or Null</returns>
        public static IDisplayAnnotation FindDisplay(Func<EnumAnnotation<T>, bool> predicate)
        {
            return Enum.GetValues(typeof(T))
                .Cast<T>()
                .Select(v => new EnumAnnotation<T>(v))
                .SingleOrDefault(predicate);
        }
    }
}    
