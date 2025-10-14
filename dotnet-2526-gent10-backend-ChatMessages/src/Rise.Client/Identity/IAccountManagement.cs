﻿using System.Threading.Tasks;
using Ardalis.Result;

namespace Rise.Client.Identity
{
    /// <summary>
    /// Account management services.
    /// </summary>
    public interface IAccountManager
    {
        /// <summary>
        /// Login service.
        /// </summary>
        /// <param name="email">User's email.</param>
        /// <param name="password">User's password.</param>
        /// <returns>The result of the request serialized to <see cref="FormResult"/>.</returns>
        public Task<Result> LoginAsync(string email, string password);

        /// <summary>
        /// Log out the logged in user.
        /// </summary>
        /// <returns>The asynchronous task.</returns>
        public Task LogoutAsync();

        /// <summary>
        /// Registration service.
        /// </summary>
        /// <param name="model">The registration model.</param>
        /// <returns>The result of the request serialized to <see cref="FormResult"/>.</returns>
        public Task<Result> RegisterAsync(Rise.Shared.Identity.Accounts.AccountRequest.Register model);

        public Task<bool> CheckAuthenticatedAsync();
    }
}
