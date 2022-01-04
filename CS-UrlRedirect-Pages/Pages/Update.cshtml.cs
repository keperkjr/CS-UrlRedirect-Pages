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
    public class UpdateModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly DatabaseDBContext _context;
        private readonly IRedirectService _redirectService;

        public IEnumerable<CS_UrlRedirect_Pages.Models.Redirect> redirects { get; set; }
        public CS_UrlRedirect_Pages.Models.RedirectViewModel redirect { get; set; }

        public UpdateModel(ILogger<IndexModel> logger, DatabaseDBContext context, IRedirectService redirectService)
        {
            _logger = logger;
            _context = context;
            _redirectService = redirectService;
        }

        // GET: /Update/1
        public async Task<IActionResult> OnGetAsync(int id)
        {
            if (!await _redirectService.ExistsAsync(id))
            {
                return NotFound();
            }

            redirects = await _context.Redirects.ToListAsync();
            var redirectDB = redirects.First((x) => x.Id == id);
            var redirectVM = new RedirectViewModel(RedirectViewModel.Action.Update);
            redirectDB.CopyPropsTo(ref redirectVM);
            redirect = redirectVM;

            return Page();
        }
    }
}
