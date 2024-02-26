using ASP.NET_Boilerplate.Debugging;

namespace ASP.NET_Boilerplate
{
    public class NET_BoilerplateConsts
    {
        public const string LocalizationSourceName = "NET_Boilerplate";

        public const string ConnectionStringName = "Default";

        public const bool MultiTenancyEnabled = true;


        /// <summary>
        /// Default pass phrase for SimpleStringCipher decrypt/encrypt operations
        /// </summary>
        public static readonly string DefaultPassPhrase =
            DebugHelper.IsDebug ? "gsKxGZ012HLL3MI5" : "5a2fda92d1d64a9082895e1d036a4d99";
    }
}
