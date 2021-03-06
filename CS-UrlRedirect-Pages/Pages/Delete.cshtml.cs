using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CS_UrlRedirect_Pages.Data;
using CS_UrlRedirect_Pages.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace CS_UrlRedirect_Pages.Pages
{
    public class DeleteModel : PageModel
    {
        private readonly ILogger<DeleteModel> _logger;
        private readonly DatabaseDBContext _context;
        private readonly IRedirectService _redirectService;

        public DeleteModel(ILogger<DeleteModel> logger, DatabaseDBContext context, IRedirectService redirectService)
        {
            _logger = logger;
            _context = context;
            _redirectService = redirectService;
        }

        public IActionResult OnGet()
        {
            return NotFound();
        }

        // POST: /Delete/5
        public async Task<IActionResult> OnPostAsync(int id, string redirectTo = "")
        {
            if (!await _redirectService.ExistsAsync(id))
            {
                return NotFound();
            }

            await _redirectService.DeleteAsync(id);
            if (string.IsNullOrWhiteSpace(redirectTo))
            {
                return RedirectToPage("Index");
            }
            return new RedirectResult(redirectTo, false);
        }
    }
}
