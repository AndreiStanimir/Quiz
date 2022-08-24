using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiz.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        public string DeviceId { get; }
        public string Username { get; }
        public string Password { get; }

        public ICollection<QuizAttempt> QuizAttempts { get; set; }
    }
}