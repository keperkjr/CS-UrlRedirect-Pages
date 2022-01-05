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

        public RedirectsTableViewModel redirectsTableVM { get; set; } = new RedirectsTableViewModel();
        public CS_UrlRedirect_Pages.Models.RedirectViewModel redirectVM { get; set; } = new RedirectViewModel();

        public IndexModel(ILogger<IndexModel> logger, DatabaseDBContext context, IRedirectService redirectService)
        {
            _logger = logger;
            _context = context;
            _redirectService = redirectService;
        }

        // GET: /mpn
        public async Task<IActionResult> OnGetAsync(string code = null)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                redirectsTableVM.redirects = await _context.Redirects.ToListAsync();
                redirectVM = new RedirectViewModel();

                return Page();
            }            
            return await DoRedirect(code);
        }
        
        public async Task<IActionResult> DoRedirect(string code)
        {
            var redirect = await _redirectService.MarkAsVisitedAsync(code);
            if (redirect == null)
            {
                return NotFound();
            }
            var destination = redirect.Url;
            if (!destination.StartsWith("http://") && !destination.StartsWith("https://"))
            {
                destination = $"http://{redirect.Url}";
            }
            _logger.Log(LogLevel.Information, $"Redirecting to url: {destination}, code: {code}");
            return new RedirectResult(destination, false);
        }
    }
}