using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CS_UrlRedirect_Pages.Models;
using Microsoft.AspNetCore.Identity;

namespace CS_UrlRedirect_Pages.Services
{
    public interface IRedirectService
    {
        Task<bool> ExistsAsync(int id);
        Task<bool> ExistsAsync(string code);
        Task<Redirect> GetAsync(int id);
        Task<Redirect> GetAsync(string code);

        Task<bool> AddAsync(Redirect newItem);
        Task<bool> UpdateAsync(int id, object newItem);
        Task<bool> DeleteAsync(int id);

        Task<Redirect> MarkAsVisitedAsync(string code);
    }
}