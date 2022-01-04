using CS_UrlRedirect_Pages.Models;
using CS_UrlRedirect_Pages.Services;
using CS_UrlRedirect_Pages.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utils;
using System.Diagnostics;

namespace CS_UrlRedirect_Pages.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly DatabaseDBContext _context;
        private readonly IRedirectService _redirectService;

        public IEnumerable<CS_UrlRedirect_Pages.Models.Redirect> redirects { get; set; }
        public CS_UrlRedirect_Pages.Models.RedirectViewModel redirect { get; set; }

        public IndexModel(ILogger<IndexModel> logger, DatabaseDBContext context, IRedirectService redirectService)
        {
            _logger = logger;
            _context = context;
            _redirectService = redirectService;
        }

        private async Task<IActionResult> ShowIndex(int? id = null)
        {
            redirects = await _context.Redirects.ToListAsync();
            redirect = new RedirectViewModel();

            if (id.HasValue)
            {
                var redirectDB = await _redirectService.GetAsync(id.Value);
                var redirectVM = new RedirectViewModel(RedirectViewModel.Action.Update);
                redirectDB.CopyPropsTo(ref redirectVM);
                redirect = redirectVM;
            }
            return Page();
        }

        public async Task<IActionResult> OnGetAsync(string code = null)
        {
            if (string.IsNullOrEmpty(code))
            {
                return await ShowIndex();
            }
            return await DoRedirect(code);
        }

        // GET: /mpn
        public async Task<IActionResult> DoRedirect(string code)
        {
            var redirect = await _redirectService.MarkAsVisitedAsync(code);
            if (redirect == null)
            {
                return NotFound();
            }
            var destination = redirect.Url;
            if (!destination.StartsWith("http://"))
            {
                destination = $"http://{redirect.Url}";
            }
            return new RedirectResult(destination, false);
        }

        // GET: /Edit/5
        public async Task<IActionResult> OnGetEditAsync(int id)
        {
            if (!await _redirectService.ExistsAsync(id))
            {
                return NotFound();
            }
            return await ShowIndex(id);
        }

        // POST: /Create/
        public async Task<IActionResult> OnPostCreateAsync([Bind("Id,ShortCode,Url,action")] RedirectViewModel redirectVM)
        {
            if (string.IsNullOrWhiteSpace(redirectVM.ShortCode))
            {
                ModelState.AddModelError(nameof(redirectVM.ShortCode), "A short code is required");
            }
            else
            {
                redirectVM.ShortCode = redirectVM.ShortCode.Trim();
                if (redirectVM.action == RedirectViewModel.Action.Create && await _redirectService.ExistsAsync(redirectVM.ShortCode))
                {
                    ModelState.AddModelError(nameof(redirectVM.ShortCode), "The following short code is unavailable and cannot be used");
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
                return new JsonResult(new { redirectTo = Url.Page(nameof(Index)) });
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
