using Newtonsoft.Json;

namespace RestaurantManagement.UI.wwwroot.admin.Utility
{
    public static class SessionHelper
    {
        /// <summary>
        /// create a session to save data
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="session"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void Set<T>(this ISession session, string key, T value) //extension method
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        //session['_Time'] = DatTime.Now;

        public static T? Get<T>(this ISession session, string key) //_Time
        {
            var value = session.GetString(key);
            return value == null ? default : JsonConvert.DeserializeObject<T>(value);
        }
    }
}
