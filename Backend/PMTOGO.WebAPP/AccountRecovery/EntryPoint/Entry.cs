using AA.PMTOGO.SqlUserDAO;
using AdminManager;



public static class Program
{
    public static async Task Main(string[] args)
    {
        var entry = new Entry();

        await entry.RecoverAccount();

        await entry.AdminRecovery();
    }
}


public class Entry
{ 
    public async Task RecoverAccount() 
    {
        string email = "";
        AccountRecovery accountRecovery = new AccountRecovery();
        do
        {
            Console.WriteLine("Enter the Email of the account you wish to recover");
            email = Console.ReadLine();

            if (email.Length < 8)
            {
                Console.WriteLine("Error try again.");
            }

        } while (email.Length < 8);

        var recoveryTask = accountRecovery.Recovery(email);
        var timeoutTask = Task.Delay(5000);

        var completedTask = await Task.WhenAny(recoveryTask, timeoutTask);

        if (completedTask == recoveryTask & recoveryTask.Result)
        {
            Console.WriteLine("Account Recovery request sent.");
        }
        else
        {
            Console.WriteLine("Account Recovery request failed.");
            return;
        }

        Console.WriteLine("Enter the OTP");

        var otpTask = accountRecovery.ValidateOTP(email, Console.ReadLine());


        completedTask = await Task.WhenAny(otpTask, timeoutTask);

        if (completedTask == otpTask && otpTask.Result)
        {
            Console.WriteLine("Your account Recovery is Successful");
        }
        else
        {
            Console.WriteLine("Your account Recovery is a Failure");
        }
    }

    public async Task AdminRecovery()
    {
        AdminRecoveryRequest adminManager = new AdminRecoveryRequest();

        List<User> users = await adminManager.GetRecoveryRequests();

        if (users.Count == 0)
        {
            Console.WriteLine("No requests pending.");
            return;
        }

        Console.WriteLine($"There are {users.Count} account Recovery requests pending.");
        foreach (User user in users)
        {
            Console.WriteLine($"Username: {user.username}, RecoveryRequest: {user.RecoveryRequest}, SuccessfulOTP: {user.successfulOTP}");

            bool isValidResponse = false;
            while (!isValidResponse)
            {
                Console.WriteLine("Do you want to unlock this account? (yes/no)");
                string response = Console.ReadLine().ToLower();

                if (response == "yes")
                {
                    var responseTask = adminManager.AccountResponse(true, user.username);
                    var completedTask = await Task.WhenAny(responseTask, Task.Delay(5000));

                    if (completedTask == responseTask & responseTask.Result)
                    {
                        Console.WriteLine($"Account for {user.username} has been unlocked.");
                    }
                    else
                    {
                        Console.WriteLine($"Failed to unlock account for {user.username}.");
                    }

                    isValidResponse = true;
                }
                else if (response == "no")
                {
                    var responseTask = adminManager.AccountResponse(false, user.username);
                    var completedTask = await Task.WhenAny(responseTask, Task.Delay(5000));

                    if (completedTask == responseTask & responseTask.Result)
                    {
                        Console.WriteLine($"Account for {user.username} has been rejected.");
                    }
                    else
                    {
                        Console.WriteLine($"Failed to reject account for {user.username}.");
                    }

                    isValidResponse = true;
                }
                else
                {
                    Console.WriteLine("Invalid response. Please enter 'yes' or 'no'.");
                }
            }
        }
    }
}