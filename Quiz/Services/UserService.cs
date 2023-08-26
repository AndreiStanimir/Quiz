using Quiz.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiz.Services
{
    public class UserService
    {
        public static User GetCurrentUser()
        {
            string currenctUserId = Preferences.Get("my_deviceId",Guid.NewGuid().ToString());
            QuizContext context = QuizContextFactory.GetContext();
            var user=context.Users.FirstOrDefault(u => u.DeviceId == currenctUserId);
            if (user == default)
            {
                user = new User() { DeviceId = currenctUserId };
                context.Users.Add(user);
                context.SaveChangesAsync();
            }
            return user;

        }

    }
}
