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
        public async Task<IActionResult> OnGetAsync(string code = null)
        {
            if (string.IsNullOrEmpty(code))
            {
                redirects = await _context.Redirects.ToListAsync();
                redirect = new RedirectViewModel();

                return Page();
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
    }
}