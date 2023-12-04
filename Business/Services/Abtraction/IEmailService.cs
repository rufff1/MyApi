using Business.Helpers;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services.Abtraction
{
    public interface IEmailService
    {
        Task SendEmailAsync(Mailrequest mailrequest, string email);
    }
}
