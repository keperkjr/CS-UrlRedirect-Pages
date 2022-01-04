using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CS_UrlRedirect_Pages.Models
{
    public class RedirectsTableViewModel
    {
        public IList<Models.Redirect> redirects { get; set; }
        public int highlightId { get; set; }
    }

    public class RedirectViewModel
    {
        public enum Action
        {
            Create,
            Update
        }

        [Display(Name = "Id #")]
        public int Id { get; set; }

        [Display(Name = "Redirect Short Code")]
        [Required(ErrorMessage = "A short code is required")]
        public string ShortCode { get; set; }

        [Display(Name = "Destination Url")]
        [Required(ErrorMessage = "A redirect url is required")]
        public string Url { get; set; }
        public Action action { get; set; }

        public RedirectViewModel()
        {
            this.action = Action.Create;
        }

        public RedirectViewModel(Action action) {
            this.action = action;
        }
    }
}
