using EtzJaimWebPage.Models;
using System;
using System.ServiceModel;
using System.Web;

namespace EtzJaimWebPage
{
    public static class service
    {
        public static bool IsAdminSession(HttpSessionStateBase session)
        {
            if (session == null)
            {
                return false;
            }

            Usuario user = session["user"] as Usuario;
            return user != null && user.tipo == 0;
        }

        public static T CallSoap<T>(Func<WebService.WebServiceSoapClient, T> action, T fallback = default(T))
        {
            WebService.WebServiceSoapClient client = null;
            try
            {
                client = new WebService.WebServiceSoapClient();
                T result = action(client);
                client.Close();
                return result;
            }
            catch
            {
                if (client != null)
                {
                    try { client.Abort(); } catch { }
                }
                return fallback;
            }
        }
    }
}
