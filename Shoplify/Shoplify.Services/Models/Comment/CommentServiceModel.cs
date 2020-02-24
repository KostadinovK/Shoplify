using System;
using System.Collections.Generic;
using System.Text;

namespace Shoplify.Services.Models.Comment
{
    public class CommentServiceModel
    {
        public string Id { get; set; }

        public string UserId { get; set; }

        public string Text { get; set; }

        public DateTime WrittenOn { get; set; }

        public DateTime? EditedOn { get; set; }
    }
}
