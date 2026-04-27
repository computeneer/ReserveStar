namespace ReserveStar.Helper.Constants;

public class ApplicationContants
{
   public const string DefaultLanguageId = "e45fe615-eac6-4b2e-bc19-3be15308b152";
   public const string SuperAdminId = "ed26678e-8c3f-4492-82dd-191cc7b13ae6";
   public const string SuperCompany = "5b24f9f1-f6f3-4386-b856-ed5941600c1a";

   /*   CACHE _ KEYS */

   public const string CACHE_PREFIX = "cache";
   public const string LANGUAGE_CACHEKEY = $"{CACHE_PREFIX}_languages";
   public const string COMPANY_CACHEKEY = $"{CACHE_PREFIX}_companies";
   public const string RESOURCE_CACHEKEY = $"{CACHE_PREFIX}_resource_{{0}}";
   public const string REFRESHTOKEN_CACHEKEY = $"{CACHE_PREFIX}_refreshtoken_{{0}}_{{1}}";

   /**  SETTINGS **/

   public const int OTP_CODE_LENGTH = 6;
   public const int OTP_CODE_VALID_MINS = 15;

}