using System;
using System.Configuration;

namespace BlobUploader.Infrastructure {

   public class ApplicationSettings : ApplicationSettingsBase {
      public string Get(string key) {
         try {
            return (string)this[key];
         }
         catch {
            return String.Empty;
         }
      }

      public bool GetBoolean(string key) {
         try {
            return (bool)this[key];
         }
         catch {
            return false;
         }
      }

      public string Get(string key, string defaultValue) {
         try {
            if (this[key] == null) {
               return defaultValue;
            }
            return (string)this[key];
         }
         catch {
            return defaultValue;
         }
      }

      public int GetIntegerValue(string key, int defaultValue) {
         var stringValue = Get(key, String.Empty);

         int days;
         if (int.TryParse(stringValue, out days) == false) {
            days = defaultValue;
         }

         return days;
      }

      public void Set(String key, String value) {
         this[key] = value;
         this.Save();
      }

      public void Set(String key, Boolean value) {
         this[key] = value;
         this.Save();
      }

      [UserScopedSettingAttribute]
      public String AccountName {
         get { return Get("AccountName"); }
         set { Set("AccountName", value); }
      }

      [UserScopedSettingAttribute]
      public String AccountKey {
         get { return Get("AccountKey"); }
         set { Set("AccountKey", value); }
      }

      [UserScopedSettingAttribute]
      public String JavaLocation {
         get { return Get("JavaLocation"); }
         set { Set("JavaLocation", value); }
      }

      [UserScopedSettingAttribute]
// ReSharper disable InconsistentNaming
      public String Neo4jLocation {
// ReSharper restore InconsistentNaming
         get { return Get("Neo4jLocation"); }
         set { Set("Neo4jLocation", value); }
      }

      [UserScopedSettingAttribute]
      public Boolean UseLocalStorage {
         get { return GetBoolean("UseLocalStorage"); }
         set { Set("UseLocalStorage", value); }
      }
   }
}
