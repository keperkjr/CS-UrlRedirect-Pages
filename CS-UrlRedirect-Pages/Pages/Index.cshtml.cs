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

        private async Task ShowIndex(int? id = null)
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
        }

        public async Task OnGetAsync()
        {
            await ShowIndex();
        }

        public async Task<IActionResult> OnPostCreateAsync()
        {
            await Task.Delay(500);
            return RedirectToPage();
        }
    }
}
