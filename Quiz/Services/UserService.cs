using Quiz.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiz.Services
{
    public static class UserService
    {
        public static async Task<User> GetCurrentUserAsync()
        {
            string currenctUserId = Preferences.Get("my_deviceId",Guid.NewGuid().ToString());
            QuizContext context = await QuizContextFactory.GetContextAsync();
            var user=context.Users.FirstOrDefault(u => u.DeviceId == currenctUserId);
            if (user == default)
            {
                user = new User() { DeviceId = currenctUserId };
                context.Users.Add(user);
                await context.SaveChangesAsync();
            }
            return user;

        }

    }
}
