
using System;
using System.Collections;
using System.Collections.Specialized;
using System.Configuration.Provider;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;

namespace Horeb.Infrastructure.Data.Security
{
  public static class SecureUtility
  {


    public static bool ValidatePasswordParameter(ref string param, int maxSize)
    {
      return param != null && param.Length >= 1 && (maxSize <= 0 || param.Length <= maxSize);
    }

    public static bool ValidateParameter(
      ref string param,
      bool checkForNull,
      bool checkIfEmpty,
      bool checkForCommas,
      int maxSize)
    {
      if (param == null)
        return !checkForNull;
      param = param.Trim();
      return (!checkIfEmpty || param.Length >= 1) && (maxSize <= 0 || param.Length <= maxSize) && (!checkForCommas || !param.Contains(","));
    }

    public static void CheckPasswordParameter(ref string param, int maxSize, string paramName)
    {
      if (param == null)
        throw new ArgumentNullException(paramName);
      if (param.Length < 1)
        throw new ArgumentException(SuperResource.GetString("Parameter_can_not_be_empty", (object) paramName), paramName);
      if (maxSize > 0 && param.Length > maxSize)
        throw new ArgumentException(SuperResource.GetString("Parameter_too_long", (object) paramName, (object) maxSize.ToString((IFormatProvider) CultureInfo.InvariantCulture)), paramName);
    }

    public static void CheckParameter(
      ref string param,
      bool checkForNull,
      bool checkIfEmpty,
      bool checkForCommas,
      int maxSize,
      string paramName)
    {
      if (param == null)
      {
        if (checkForNull)
          throw new ArgumentNullException(paramName);
      }
      else
      {
        param = param.Trim();
        if (checkIfEmpty && param.Length < 1)
          throw new ArgumentException(SuperResource.GetString("Parameter_can_not_be_empty", (object) paramName), paramName);
        if (maxSize > 0 && param.Length > maxSize)
          throw new ArgumentException(SuperResource.GetString("Parameter_too_long", (object) paramName, (object) maxSize.ToString((IFormatProvider) CultureInfo.InvariantCulture)), paramName);
        if (checkForCommas && param.Contains(","))
          throw new ArgumentException(SuperResource.GetString("Parameter_can_not_contain_comma", (object) paramName), paramName);
      }
    }

    public static void CheckArrayParameter(
      ref string[] param,
      bool checkForNull,
      bool checkIfEmpty,
      bool checkForCommas,
      int maxSize,
      string paramName)
    {
      if (param == null)
        throw new ArgumentNullException(paramName);
      if (param.Length < 1)
        throw new ArgumentException(SuperResource.GetString("Parameter_array_empty", (object) paramName), paramName);
      Hashtable hashtable = new Hashtable(param.Length);
      for (int index = param.Length - 1; index >= 0; --index)
      {
                SecureUtility.CheckParameter(ref param[index], checkForNull, checkIfEmpty, checkForCommas, maxSize, paramName + "[ " + index.ToString((IFormatProvider) CultureInfo.InvariantCulture) + " ]");
        if (hashtable.Contains((object) param[index]))
          throw new ArgumentException(SuperResource.GetString("Parameter_duplicate_array_element", (object) paramName), paramName);
        hashtable.Add((object) param[index], (object) param[index]);
      }
    }

    public static bool GetBooleanValue(
      NameValueCollection config,
      string valueName,
      bool defaultValue)
    {
      string str = config[valueName];
      if (str == null)
        return defaultValue;
      bool result;
      if (bool.TryParse(str, out result))
        return result;
      throw new ProviderException(SuperResource.GetString("Value_must_be_boolean", (object) valueName));
    }

    public static int GetIntValue(
      NameValueCollection config,
      string valueName,
      int defaultValue,
      bool zeroAllowed,
      int maxValueAllowed)
    {
      string s = config[valueName];
      if (s == null)
        return defaultValue;
      int result;
      if (!int.TryParse(s, out result))
      {
        if (zeroAllowed)
          throw new ProviderException(SuperResource.GetString("Value_must_be_non_negative_integer", (object) valueName));
        throw new ProviderException(SuperResource.GetString("Value_must_be_positive_integer", (object) valueName));
      }
      if (zeroAllowed && result < 0)
        throw new ProviderException(SuperResource.GetString("Value_must_be_non_negative_integer", (object) valueName));
      if (!zeroAllowed && result <= 0)
        throw new ProviderException(SuperResource.GetString("Value_must_be_positive_integer", (object) valueName));
      if (maxValueAllowed > 0 && result > maxValueAllowed)
        throw new ProviderException(SuperResource.GetString("Value_too_big", (object) valueName, (object) maxValueAllowed.ToString((IFormatProvider) CultureInfo.InvariantCulture)));
      return result;
    }

    public static int? GetNullableIntValue(NameValueCollection config, string valueName)
    {
      string s = config[valueName];
      int result;
      if (s == null || !int.TryParse(s, out result))
        return new int?();
      return new int?(result);
    }

    public static void CheckSchemaVersion(
      ProviderBase provider,
      SqlConnection connection,
      string[] features,
      string version,
      ref int schemaVersionCheck)
    {
      if (connection == null)
        throw new ArgumentNullException(nameof (connection));
      if (features == null)
        throw new ArgumentNullException(nameof (features));
      if (version == null)
        throw new ArgumentNullException(nameof (version));
      if (schemaVersionCheck == -1)
        throw new ProviderException(SuperResource.GetString("Provider_Schema_Version_Not_Match", (object) provider.ToString(), (object) version));
      if (schemaVersionCheck != 0)
        return;
      lock (provider)
      {
        if (schemaVersionCheck == -1)
          throw new ProviderException(SuperResource.GetString("Provider_Schema_Version_Not_Match", (object) provider.ToString(), (object) version));
        if (schemaVersionCheck != 0)
          return;
        foreach (string feature in features)
        {
          SqlCommand sqlCommand = new SqlCommand("dbo.aspnet_CheckSchemaVersion", connection);
          sqlCommand.CommandType = CommandType.StoredProcedure;
          SqlParameter sqlParameter1 = new SqlParameter("@Feature", (object) feature);
          sqlCommand.Parameters.Add(sqlParameter1);
          SqlParameter sqlParameter2 = new SqlParameter("@CompatibleSchemaVersion", (object) version);
          sqlCommand.Parameters.Add(sqlParameter2);
          SqlParameter sqlParameter3 = new SqlParameter("@ReturnValue", SqlDbType.Int);
          sqlParameter3.Direction = ParameterDirection.ReturnValue;
          sqlCommand.Parameters.Add(sqlParameter3);
          sqlCommand.ExecuteNonQuery();
          if ((sqlParameter3.Value != null ? (int) sqlParameter3.Value : -1) != 0)
          {
            schemaVersionCheck = -1;
            throw new ProviderException(SuperResource.GetString("Provider_Schema_Version_Not_Match", (object) provider.ToString(), (object) version));
          }
        }
        schemaVersionCheck = 1;
      }
    }

        public static bool IsValidInteger(string intCandidate)
        {
            int intParam;
            return int.TryParse(intCandidate, out intParam);
        }
  }
}
