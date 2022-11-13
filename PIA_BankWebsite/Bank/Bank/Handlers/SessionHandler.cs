using Bank.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bank.Handlers
{
    /// <summary>
    /// Handle sissions of loged users and identity keys
    /// </summary>
    public class SessionHandler
    {
        private static Dictionary<string, User> sessions = new Dictionary<string, User>();

        /// <summary>
        /// Add new user to the sessions list
        /// Generates identity key
        /// </summary>
        /// <param name="u">User</param>
        /// <returns>Identity key</returns>
        public static string NewSession(User u)
        {
            string g = Guid.NewGuid().ToString();
            sessions.Add(g, u);
            return g;
        }

        /// <summary>
        /// Returns user if in sessions list
        /// </summary>
        /// <param name="g">Identity key</param>
        /// <param name="u">Output for User</param>
        /// <returns>True user in the list, false if not</returns>
        public static bool GetUser(string g, out User u)
        {
            return sessions.TryGetValue(g, out u);
        }

        /// <summary>
        /// Sets new user for identity key (E.g. User edit)
        /// </summary>
        /// <param name="g"></param>
        /// <param name="u"></param>
        public static void SetUser(string g, User u)
        {
            sessions[g] = u;
        }

        /// <summary>
        /// Remove user from sessins list
        /// </summary>
        /// <param name="g">Identity key</param>
        public static void DestroySession(string g)
        {
            sessions.Remove(g);
        }
    }
}
