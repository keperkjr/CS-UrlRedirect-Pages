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
    }
}
