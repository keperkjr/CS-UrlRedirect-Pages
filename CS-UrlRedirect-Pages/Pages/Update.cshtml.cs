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

        public RedirectsTableViewModel redirectsTableVM { get; set; } = new RedirectsTableViewModel();
        public CS_UrlRedirect_Pages.Models.RedirectViewModel redirectVM { get; set; } = new RedirectViewModel();

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

            redirectsTableVM.redirects = await _context.Redirects.ToListAsync();
            redirectsTableVM.highlightId = id;
            var redirectDB = redirectsTableVM.redirects.First((x) => x.Id == id);
            var redirectVM = new RedirectViewModel(RedirectViewModel.Action.Update);
            redirectDB.CopyPropsTo(ref redirectVM);
            this.redirectVM = redirectVM;

            return Page();
        }
    }
}
