using System;
using System.ComponentModel.DataAnnotations;

namespace Bank.Models
{
    
    public class ErrorViewModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}