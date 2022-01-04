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
        private readonly ILogger<IndexModel> _logger;
        private readonly DatabaseDBContext _context;
        private readonly IRedirectService _redirectService;

        public DeleteModel(ILogger<IndexModel> logger, DatabaseDBContext context, IRedirectService redirectService)
        {
            _logger = logger;
            _context = context;
            _redirectService = redirectService;
        }

        public IActionResult OnGet()
        {
            return RedirectToPage("Index");
        }

        // POST: /Delete/5
        public async Task<IActionResult> OnPostAsync(int id)
        {
            await _redirectService.DeleteAsync(id);
            return RedirectToPage("Index");
        }
    }
}
