using MachineLearningBP.Entities.Movies.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace MachineLearningBP.Framework
{
    #region EnumExtensions
    public static class EnumExtensions
    {
        private static readonly String enumPath = "C:\\Users\\csandfort\\Documents\\Visual Studio 2017\\Projects\\MachineLearningBP\\MachineLearningBP.Core\\CodeGen\\Enums\\";

        #region GetDisplay
        public static String GetDisplay(this Enum value)
        {
            // Get the type
            Type type = value.GetType();

            // Get fieldinfo for this type
            FieldInfo fieldInfo = type.GetField(value.ToString());

            // Get the stringvalue attributes
            DisplayAttribute[] attribs = fieldInfo.GetCustomAttributes(
                typeof(DisplayAttribute), false) as DisplayAttribute[];

            // Return the first if there was a match.
            return attribs.Length > 0 ? attribs[0].Display : String.Empty;
        }
        #endregion

        #region ParseEnum
        public static T ParseEnum<T>(string value, T defaultValue) where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum) throw new ArgumentException("T must be an enumerated type");
            if (string.IsNullOrEmpty(value)) return defaultValue;

            foreach (T item in Enum.GetValues(typeof(T)))
            {
                if (item.ToString().ToLower().Equals(value.Trim().Enumify().ToLower())) return item;
            }
            return defaultValue;
        } 
        #endregion

        #region Enum CodeGen
        public static String Enumify(this String val)
        {
            if (val.StartsWith("3")) val = val.Replace("3", "Three");
            return val.Replace(": ", "_").Replace(". ", "_").Replace(".", "_").Replace("-", "_").Replace("'", String.Empty).Replace(" / ", "_").Replace("/", "_").Replace(" ", "_");
        }

        public static void GenEnum(this List<String> vals, String ns, String enumType)
        {
            using (StreamWriter enumFile = new StreamWriter($"{enumPath}{enumType}.cs", false))
            {
                enumFile.WriteLine("using MachineLearningBP.Framework;");
                enumFile.WriteLine();
                enumFile.WriteLine($"namespace {ns}");
                enumFile.WriteLine("{");
                enumFile.WriteLine($"   public enum {enumType}");
                enumFile.WriteLine("   {");
                enumFile.WriteLine($"       [Display(\"None\")]");
                enumFile.WriteLine($"       None,");

                foreach (String val in vals)
                {
                    enumFile.WriteLine($"       [Display(\"{val}\")]");
                    enumFile.WriteLine($"       {val.Enumify()},");
                }

                enumFile.WriteLine("   }");
                enumFile.WriteLine("}");

                enumFile.Close();
            }
        }
        #endregion

        #region IsMegaBrand
        public static bool IsMegaBrand(this MovieBrands brand)
        {
            bool isMegaBrand = false;

            switch (brand)
            {
                case MovieBrands.Lucasfilm:
                case MovieBrands.Marvel_Comics:
                    isMegaBrand = true;
                    break;
            }

            return isMegaBrand;
        }
        #endregion

        #region IsAnimationBrand
        public static bool IsAnimationBrand(this MovieBrands brand)
        {
            bool isAnimationBrand = false;

            switch (brand)
            {
                case MovieBrands.Pixar:
                case MovieBrands.DreamWorks_Animation:
                case MovieBrands.Walt_Disney_Animation_Studios:
                    isAnimationBrand = true;
                    break;
            }

            return isAnimationBrand;
        }
        #endregion
    }
    #endregion

    #region Attributes
    #region DisplayAttribute
    /// <summary>
    /// This attribute is used to represent a string value
    /// for a value in an enum.
    /// </summary>
    public class DisplayAttribute : Attribute
    {
        #region Properties
        /// <summary>
        /// Holds the stringvalue for a value in an enum.
        /// </summary>
        public String Display { get; protected set; }
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor used to init a StringValue Attribute
        /// </summary>
        /// <param name="value"></param>
        public DisplayAttribute(String value)
        {
            this.Display = value;
        }
        #endregion
    }
    #endregion
    #endregion
}
