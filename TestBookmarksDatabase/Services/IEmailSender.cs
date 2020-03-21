using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestBookmarksDatabase.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string text);
        string HtmlMessage { get; set; }
    }
}
