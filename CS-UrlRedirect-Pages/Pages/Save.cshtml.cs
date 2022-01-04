using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using CS_UrlRedirect_Pages.Data;
using CS_UrlRedirect_Pages.Models;
using CS_UrlRedirect_Pages.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Utils;

namespace CS_UrlRedirect_Pages.Pages
{
    public class SaveModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly DatabaseDBContext _context;
        private readonly IRedirectService _redirectService;

        public SaveModel(ILogger<IndexModel> logger, DatabaseDBContext context, IRedirectService redirectService)
        {
            _logger = logger;
            _context = context;
            _redirectService = redirectService;
        }

        public IActionResult OnGet()
        {
            return RedirectToPage("Index");
        }

        // POST: /Save/
        public async Task<IActionResult> OnPostAsync([Bind("Id,ShortCode,Url,action")] RedirectViewModel redirectVM, string redirectTo = "")
        {
            if (string.IsNullOrWhiteSpace(redirectVM.ShortCode))
            {
                ModelState.AddModelError(nameof(redirectVM.ShortCode), "A short code is required");
            }
            else
            {
                redirectVM.ShortCode = redirectVM.ShortCode.Trim();
                if (await _redirectService.ExistsAsync(redirectVM.ShortCode) && (await _redirectService.GetAsync(redirectVM.ShortCode)).Id != redirectVM.Id)
                {
                    ModelState.AddModelError(nameof(redirectVM.ShortCode), "The following short code already exists on another entry and cannot be used");
                }
            }

            if (string.IsNullOrWhiteSpace(redirectVM.Url))
            {
                ModelState.AddModelError(nameof(redirectVM.Url), "A redirect url is required");
            }
            else
            {
                redirectVM.Url = redirectVM.Url.Trim();
                if (!Http.IsValidURL(redirectVM.Url))
                {
                    ModelState.AddModelError(nameof(redirectVM.Url), "A valid destination url is required");
                }
            }

            if (ModelState.IsValid)
            {
                switch (redirectVM.action)
                {
                    case RedirectViewModel.Action.Create:
                        await CreateEntry(redirectVM);
                        break;
                    case RedirectViewModel.Action.Update:
                        try
                        {
                            await UpdateEntry(redirectVM);
                        }
                        catch (DbUpdateConcurrencyException)
                        {
                            if (await _redirectService.ExistsAsync(redirectVM.Id))
                            {
                                throw;
                            }
                            return NotFound();
                        }
                        break;
                }
                return new JsonResult(new { redirectTo = string.IsNullOrWhiteSpace(redirectTo) ? Url.Page("Index") : redirectTo });
            }
            else
            {
                var errors = ModelState
                    .Where(x => x.Value.Errors.Count > 0)
                    .Select(x => new { x.Key, x.Value.Errors })
                    .ToList();

                Debug.Print("");
            }
            return Partial("_RedirectForm", redirectVM);
        }

        public async Task CreateEntry(RedirectViewModel redirectVM)
        {
            var redirect = new Redirect();
            redirectVM.CopyPropsTo(ref redirect);
            await _redirectService.AddAsync(redirect);
        }

        // https://docs.microsoft.com/en-us/ef/core/saving/disconnected-entities
        public async Task UpdateEntry(RedirectViewModel redirectVM)
        {
            await _redirectService.UpdateAsync(redirectVM.Id, redirectVM);
        }
    }
}
