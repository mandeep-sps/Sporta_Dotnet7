using System;

namespace Sporta.WebApp.Models.ViewModel
{
    public class ErrorRequest
    {
        public Guid Id { get; set; }
        public string Information { get; set; }
        public int UserId { get; set; }
        public string Username { get; internal set; }
        public DateTime LogTime { get; internal set; }
    }
}