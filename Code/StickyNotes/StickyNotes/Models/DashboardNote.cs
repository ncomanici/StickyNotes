using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StickyNotes.Models
{
    public class DashboardNote
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string ApplicationUserId { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}